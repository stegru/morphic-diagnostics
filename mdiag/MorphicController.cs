using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdiag
{
    internal class MorphicController
    {
        public readonly string Build = BuildDate.Timestamp.ToString("yyyy-MM-dd");
        private bool environmentLogged;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int hMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);

        public Logger Logger { get; private set; }
        public string InstallDir { get; private set; }
        public bool IsInstalled => InstallDir != null;

        public bool ServiceInstalled { get; private set; }
        public string MorphicExe { get; private set; }
        public string MorphicLogDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gpii");
        public string ServiceLogDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Morphic");

        public string SourceDir { get; private set; }
        public string GpiiWindowsDir { get; private set; }
        public string GpiiUniversalDir { get; private set; }

        // Log for this application
        public string LogFile { get; private set; }

        public MorphicController()
        {
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh.mm.ss");
            Directory.CreateDirectory(this.MorphicLogDir);
            this.LogFile = Path.Combine(this.MorphicLogDir, "mdiag-" + timestamp + ".log");
            this.Logger = new Logger();
            this.Logger.Log += (s,e) => File.AppendAllText(this.LogFile, e + Environment.NewLine);
        }

        
        /// <summary>
        /// Capture some information about the environment.
        /// </summary>
        public void LogEnvironment(bool always = false)
        {
            if (this.environmentLogged && !always)
            {
                return;
            }
            this.environmentLogged = true;

            this.Logger.Write();
            this.Logger.Write("System Info:");
            this.Logger.Command("systeminfo");

            this.Logger.Properties(typeof(Environment), new string[] {
                nameof(Environment.CurrentManagedThreadId),
                nameof(Environment.StackTrace),
                nameof(Environment.NewLine),
                nameof(Environment.ExitCode),
                nameof(Environment.TickCount)
            });
            this.Logger.Write();

            this.Logger.Write("Environment:");
            foreach (DictionaryEntry item in Environment.GetEnvironmentVariables())
            {
                this.Logger.WritePair((string)item.Key, item.Value);
            }
            this.Logger.Write();

            this.Logger.Write("Special Folders:");
            foreach (Environment.SpecialFolder value in Enum.GetValues(typeof(Environment.SpecialFolder))) {
                this.Logger.WritePair(value.ToString(), Environment.GetFolderPath(value, Environment.SpecialFolderOption.DoNotVerify));
            }

            this.Logger.Write();
            this.Logger.Write("Languages:");
            this.Logger.Command("wmic os get muilanguages");
            this.Logger.Command("wmic os");

            this.Logger.Write();
            this.Logger.Write("Software (64 bit):");
            this.Logger.Command(@"powershell ""Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | Select-Object DisplayName, DisplayVersion, Publisher, InstallDate | Format-Table -AutoSize | Out-String -Width 5000""");

            this.Logger.Write("Software (32 bit):");
            this.Logger.Command(@"powershell ""Get-ItemProperty HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\* | Select-Object DisplayName, DisplayVersion, Publisher, InstallDate | Format-Table -AutoSize | Out-String -Width 5000""");

            this.Logger.Write();
            this.Logger.Write("Processes");
            this.Logger.Command("tasklist");

            this.Logger.Write();
            this.Logger.Write("Service status");
            int ret = this.Logger.Command("sc query morphic-service");
            this.ServiceInstalled = (ret == 0);
            this.Logger.Command("sc query morphic-filebeat");

        }

        /// <summary>
        /// Mounts a dummy USB with the given gpii key.
        /// </summary>
        /// <param name="keyText"></param>
        /// <returns>Drive letter.</returns>
        internal string MountUsb(string keyText)
        {
            char[] drives = Environment.GetLogicalDrives().Select(d => d.ToUpperInvariant()[0]).ToArray();
            if (drives.Length > 25)
            {
                this.Logger.Write("Too many drives mounted");
                return null;
            }

            // Pick an arbitrary drive letter that's unused.
            int n = Environment.TickCount;
            char ch;
            do
            {
                ch = (char)('A' + (n++ % 26));
            } while (drives.Contains(ch));

            string letter = new string(ch, 1);

            // Use a temporary directory for the content.
            string dir = Path.Combine(Path.GetTempPath(), @"mdiag-usb\" + letter);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, ".gpii-user-token.txt"), keyText);

            // Mount it
            this.Logger.Command("subst " + letter + ": " + dir);
            // Tell the system a new USB drive has been added (morphic listens to this)
            FakeUsb.SendNotification(letter, true);

            return letter;
        }

        /// <summary>
        /// Unmounts a dummy usb drive that was mounted via MountUsb.
        /// </summary>
        /// <param name="letter">The drive letter, returned by MountUsb</param>
        internal void UnmountUsb(string letter)
        {
            // Unmount it
            this.Logger.Command("subst /d " + letter + ":");
            try
            {
                // Remove the temporary directory.
                string dir = Path.Combine(Path.GetTempPath(), "mdiag-usb");
                Directory.Delete(Path.Combine(dir, letter), true);
                Directory.Delete(dir);
            }
            catch (IOException e)
            {
                // ignore
            }
            FakeUsb.SendNotification(letter, false);
        }

        /// <summary>
        /// Sends an action to the flowmanager
        /// </summary>
        /// <param name="gpiiKey">The gpiiKey</param>
        /// <param name="action">login, logout, or proximityTriggered</param>
        internal async void FlowManagerAsync(string gpiiKey, string action)
        {
            string url = "http://127.0.0.1:8081/user/" + gpiiKey + "/" + action;

            this.Logger.Write("Requesting", url);

            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader body = new StreamReader(response.GetResponseStream()))
                    {
                        this.Logger.Write(await body.ReadToEndAsync());
                    }
                }
            } catch (WebException e)
            {
                this.Logger.Write(url, e.Message);
            }
        }

        // Capture some initial information.
        public void Start()
        {
            this.Logger.Write("Morphic Diagnostics (built " + this.Build + ")\n");
            this.Logger.Write();
            this.Logger.Write("Logging to", this.LogFile);
            this.Logger.Write("Local Time", DateTime.Now.ToString("s"));
            this.Logger.Write("UTC Time", DateTime.Now.ToString("u"));
            this.Logger.Write();

            // Detect morphic installation
            this.MorphicExe = FirstPathExists(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)??"", @"Morphic\windows\morphic-app.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)??"", @"Morphic\windows\morphic-app.exe"));
            if (this.MorphicExe != null)
            {
                this.InstallDir = Path.GetDirectoryName(Path.GetDirectoryName(this.MorphicExe));
                if (this.InstallDir != null)
                {
                    this.SourceDir = Path.Combine(this.InstallDir, @"windows\resources\app");
                    this.GpiiWindowsDir = Path.Combine(this.SourceDir, @"node_modules\gpii-windows");
                    this.GpiiUniversalDir = Path.Combine(this.SourceDir, @"node_modules\gpii-universal");
                }
            }

            this.Logger.WritePair("Install Directory", this.InstallDir ?? "<not installed>");
            this.Logger.WritePair("Morphic Executable", this.MorphicExe ?? "<not installed>");
            this.Logger.WritePair("Morphic Logs", this.MorphicLogDir);
            this.Logger.WritePair("Service Logs", this.ServiceLogDir);

            if (this.SourceDir != null)
            {
                this.Logger.WritePair("Morphic build", this.GetPackageVersion(Path.Combine(this.SourceDir, "package.json")));
                this.Logger.WritePair("gpii-windows", this.GetPackageVersion(Path.Combine(this.SourceDir, "package.json"), "gpii-windows"));
                this.Logger.WritePair("gpii-universal", this.GetPackageVersion(Path.Combine(this.GpiiWindowsDir, "package.json"), "gpii-universal"));
            }
            this.Logger.Write();
        }

        private string GetPackageVersion(string packageFile, string fieldName = "version")
        {
            try
            {
                string fieldText = "\"" + fieldName + "\":";
                // Search the json file for the first "version" field line
                string s = File.ReadAllLines(packageFile)
                    .Select(line => line.Trim())
                    .Where(line => line.StartsWith(fieldText))
                    .FirstOrDefault();
                if (s != null)
                {
                    return new Regex(fieldText + @" *""([^""]+)"".*").Replace(s, "$1");
                }
            }
            catch (IOException e)
            {
                this.Logger.Write(packageFile, e.Message);
            }

            return "<unknown>";
        }

        /// <summary>
        /// Sends morphic a shutdown signal (so it thinks the windows session is ending).
        /// </summary>
        internal void Shutdown()
        {
            // Find the message window
            IntPtr hwnd = FindWindow("gpii-message-window", IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                this.Logger.Write("Can't find the gpii message window");
            }
            else
            {
                const int WM_QUERYENDSESSION = 0x11;
                const int WM_ENDSESSION = 0x16;
                // Send the shutdown message.
                SendMessage(hwnd, WM_QUERYENDSESSION, 0, 0);
            }
        }

        /// <summary>
        /// Returns the first path in an array that exists.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>null if none exist.</returns>
        private string FirstPathExists(params string[] paths)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of preference sets from the local installation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPreferenceSets()
        {
            if (this.GpiiUniversalDir == null)
            {
                return new string[0];
            }
            else
            {
                string dir = Path.Combine(this.GpiiUniversalDir, @"testData\preferences");
                return Directory.EnumerateFiles(dir, "*.json5").Select(file => Path.GetFileNameWithoutExtension(file));
            }
        }

        /// <summary>
        /// Gets the newest log file (based on the dated filename) in the log directory.
        /// </summary>
        /// <returns></returns>
        internal string GetMorphicLog()
        {
            return Directory.EnumerateFiles(this.MorphicLogDir, "log-????-*.txt")
                .OrderByDescending(f => f)
                .FirstOrDefault();
        }

        /// <summary>
        /// Detect a running morphic process, using its pid file (vulnerable to pid reuse).
        /// </summary>
        /// <returns>null if the process isn't running.</returns>
        public Process GetMorphicProcess()
        {
            // Get the pid from the pid file.
            string pidFile = Path.Combine(this.MorphicLogDir, "gpii.pid");
            int pid = -1;
            try
            {
                pid = int.Parse(File.ReadAllText(pidFile));
            }
            catch (IOException){}
            catch (ArgumentException){}
            catch (FormatException){}

            Process process = null;

            if (pid > 0)
            {
                try
                {
                    process = Process.GetProcessById(pid);
                    process.EnableRaisingEvents = true;
                }
                catch (ArgumentException) { }
                catch (InvalidOperationException) { }
            }

            return process;
        }

        public void CaptureLogs(string saveAs, IEnumerable<string> extraFiles = null)
        {
            LogCapture capture = new LogCapture(this.Logger)
            {
                MorphicDirectory = this.MorphicLogDir,
                ServiceDirectory = this.ServiceLogDir
            };
            if (this.InstallDir != null)
            {
                capture.ExtraFiles.Add(Path.Combine(this.InstallDir, "windows\\service.json5"));
            }
            if (this.SourceDir != null)
            {
                capture.ExtraFiles.Add(Path.Combine(this.SourceDir, "package-lock.json"));
                capture.ExtraFiles.Add(Path.Combine(this.SourceDir, "package.json"));
                capture.ExtraFiles.Add(Path.Combine(this.SourceDir, "siteconfig.json5"));
            }
            if (extraFiles != null)
            {
                capture.ExtraFiles.AddRange(extraFiles);
            }
            capture.ExtraFiles.Add(this.LogFile);
            capture.ToZip(saveAs);
            
        }
    }
}
