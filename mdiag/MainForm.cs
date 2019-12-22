using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mdiag
{
    public partial class MainForm : Form
    {
        private MorphicController Morphic;
        private Logger Logger;
        private System.Windows.Forms.Timer processTimer;
        private System.Windows.Forms.Timer serviceTimer;
        private ServiceController morphicService;

        public MainForm()
        {
            InitializeComponent();

            this.morphicLogText.Font = this.serviceLogText.Font = this.metricsLogText.Font = this.logText.Font;

            this.keyInMethod.SelectedIndex = 0;

            this.Morphic = new MorphicController();
            this.Text += " - " + this.Morphic.Build;
            this.Logger = this.Morphic.Logger;
            this.Logger.Log += Logger_Log;
            this.Morphic.Start();

            // Auto-completion list for the preference sets
            string[] prefNames = this.Morphic.GetPreferenceSets().ToArray();
            AutoCompleteStringCollection prefs = new AutoCompleteStringCollection();
            prefs.AddRange(prefNames);
            this.keyInText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.keyInText.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.keyInText.AutoCompleteCustomSource = prefs;
            this.keyInText.Items.AddRange(prefNames);

            // Wait for a morphic process to be detected
            this.processTimer = new System.Windows.Forms.Timer(this.components)
            {
                Enabled = true,
                Interval = 1000
            };

            this.processTimer.Tick += ProcessTimer_Tick;

            // Monitor the morphic service
            this.morphicService = new ServiceController("morphic-service");
            this.serviceTimer = new System.Windows.Forms.Timer(this.components)
            {
                Enabled = true,
                Interval = 1000
            };
            this.serviceTimer.Tick += ServiceTimer_Tick;

            // Monitor the service log
            ContinualFileLog(Path.Combine(this.Morphic.ServiceLogDir, "morphic-service.log"), this.serviceLogText);
        }

        // Updates the state of the morphic process
        private void ProcessTimer_Tick(object sender, EventArgs e)
        {
            if (this.morphicProcess != null)
            {
                if (!this.morphicProcess.HasExited)
                {
                    // still running
                    return;
                }
            }

            this.morphicProcess = this.Morphic.GetMorphicProcess();

            if (this.morphicProcess != null)
            {
                //this.processTimer.Enabled = false;
                morphicProcess.Exited += (s, ev) =>
                {
                    this.Logger.Write("Morphic process has stopped");
                    this.Invoke(new Action(() =>
                    {
                        this.processTimer.Enabled = true;
                        this.ProcessTimer_Tick(sender, e);
                    }));
                };

                this.Logger.Write("Found morphic process", this.morphicProcess.Id.ToString());
                this.processGroup.Text = "Morphic Process: " + this.morphicProcess.Id;
                // Update the log for the morphic process
                logTabs_SelectedIndexChanged(null, null);
            }
            else if (this.processGroup.Enabled)
            {
                this.Logger.Write("Morphic process is not running");
                this.processGroup.Text = "Morphic Process: not running";
            }

            this.processGroup.Enabled = this.morphicProcess != null;
        }

        // Called when something has been logged.
        private void Logger_Log(object sender, string e)
        {
            Action log = new Action(() =>
            {
                this.logText.AppendText(e + Environment.NewLine);
            });

            // Append the log text in the GUI thread
            if (this.logText.InvokeRequired)
            {
                this.logText.Invoke(log);
            }
            else
            {
                log();
            }
        }

        private string lastUsb = null;
        private Process morphicProcess;

        // Perform a key-in
        private void keyInButton_Click(object sender, EventArgs e)
        {
            if (this.keyInMethod.Text == "USB")
            {
                if (this.lastUsb == null)
                {
                    this.lastUsb = this.Morphic.MountUsb(this.keyInText.Text);
                    this.Morphic.Logger.Write("Mounted fake usb '" + this.lastUsb + "'");
                }
                else
                {
                    this.Morphic.Logger.Write("Already mounted fake usb '" + this.lastUsb + "'");
                }
            }
            else
            {
                this.Morphic.FlowManagerAsync(this.keyInText.Text, "login");
            }
        }

        // Perform a key-out
        private void keyOutButton_Click(object sender, EventArgs e)
        {
            if (this.keyInMethod.Text == "USB")
            {
                if (this.lastUsb == null)
                {
                    this.Morphic.Logger.Write("Not mounted a fake usb");
                }
                else
                {
                    this.Morphic.Logger.Write("Unmounting fake usb '" + this.lastUsb + "'");
                    this.Morphic.UnmountUsb(this.lastUsb);
                    this.lastUsb = null;
                }
            }
            else
            {
                this.Morphic.FlowManagerAsync(this.keyInText.Text, "logout");
            }
        }

        // Kill morphic.
        private void killMorphicButton_Click(object sender, EventArgs e)
        {
            if (this.morphicProcess == null)
            {
                this.Logger.Write("Morphic process isn't running");
            }
            else
            {
                this.Logger.Write("Killing morphic");
                this.morphicProcess.Kill();
                this.morphicProcess.Dispose();
                this.morphicProcess = null;
            }
        }

        // Kill the tray button process.
        private void killIconButton_Click(object sender, EventArgs e)
        {
            Process.GetProcessesByName("tray-button").ToList().ForEach(p => p.Kill());
        }

        private void shutdownButton_Click(object sender, EventArgs e)
        {
            this.Morphic.Shutdown();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private ServiceControllerStatus serviceState;
        // Update the service status.
        private void ServiceTimer_Tick(object sender, EventArgs e)
        {
            bool installed = false;
            string status = "Not installed";

            try
            {
                this.morphicService.Refresh();
                installed = true;
                status = this.morphicService.Status.ToString();
            }
            catch (InvalidOperationException)
            {
                installed = false;
            }

            string text = "Morphic Service: " + status;
            if (this.serviceGroup.Text != text)
            {
                this.serviceGroup.Text = "Morphic Service: " + status;
            }

            if (installed)
            {
                this.serviceStartButton.Enabled = this.morphicService.Status == ServiceControllerStatus.Stopped;
                this.serviceRestartButton.Enabled = this.serviceStopButton.Enabled =
                    this.morphicService.CanStop && this.morphicService.Status == ServiceControllerStatus.Running;

                if (this.morphicService.Status != this.serviceState)
                {
                    this.serviceState = this.morphicService.Status;
                    this.Logger.WritePair("Service state", this.serviceState);
                }
            }

            this.serviceGroup.Enabled = installed;
        }

        // Stop and start the service.
        private void serviceRestartButton_Click(object sender, EventArgs e)
        {
            this.ControlService(false);
            Thread t = new Thread(() =>
            {
                this.morphicService.WaitForStatus(ServiceControllerStatus.Stopped);
                this.ControlService(true);
            });
            t.Start();
        }

        // Start or stop the service.
        private void ControlService(bool start, string serviceName = null)
        {
            this.Logger.Write(start ? "Starting" : "Stopping", serviceName ?? "Morphic", "Service");

            bool success = false;
            try
            {
                if (serviceName == null)
                {
                    if (start)
                    {
                        this.morphicService.Start();
                    }
                    else
                    {
                        this.morphicService.Stop();
                    }
                    success = true;
                }
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException?.Message != "Access is denied")
                {
                    this.Logger.Write(e);
                }
                success = false;
            }

            if (!success)
            {
                this.Logger.Command("sc " + (start ? "start" : "stop") + " " + serviceName ?? this.morphicService.ServiceName, true);
            }
        }

        // Start the service.
        private void serviceStartButton_Click(object sender, EventArgs e)
        {
            this.ControlService(true);
        }

        // Stop the service.
        private void serviceStopButton_Click(object sender, EventArgs e)
        {
            this.ControlService(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void logTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable morphic logs if the tab is shown.
            if (this.logTabs.SelectedTab == this.morphicLogTab)
            {
                string logFile = this.Morphic.GetMorphicLog();
                this.ContinualFileLog(logFile, this.morphicLogText);
            }
            else
            {
                this.ContinualFileLog(null, this.morphicLogText);
                if (sender != null)
                {
                    TextBox textBox = this.logTabs.SelectedTab.Controls.Cast<Control>()
                        .Where(c => c is TextBox).SingleOrDefault() as TextBox;
                    if (textBox != null)
                    {
                        textBox.SelectionStart = textBox.Text.Length;
                        textBox.ScrollToCaret();
                    }
                }
            }
        }

        Dictionary<TextBox, Thread> logThreads = new Dictionary<TextBox, Thread>();

        // "tail -f"
        private void ContinualFileLog(string file, TextBox textBox)
        {
            Thread current;
            if (logThreads.TryGetValue(textBox, out current))
            {
                if (textBox.Tag as string == file)
                {
                    // Already logging this file
                    return;
                }
                logThreads.Remove(textBox);
                current.Abort();
                current.Join();
            }

            if (file == null)
            {
                return;
            }

            // Continually read from the file, adding anything new to the textbox.
            Thread thread = new Thread(() =>
            {
                long lastPos = 0;
                while (true)
                {
                    try
                    {
                        // Check the length - only act if it's changed.
                        long fileLength = new FileInfo(file).Length;
                        if (fileLength < lastPos)
                        {
                            this.Logger.Write(file, "truncated");
                            textBox.Invoke(new Action(() => textBox.AppendText("<<truncated>>")));
                            lastPos = 0;
                        }
                        if (fileLength > lastPos)
                        {
                            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                stream.Seek(lastPos, SeekOrigin.Begin);
                                byte[] buffer = new byte[0xffff];
                                int len;
                                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    lastPos += len;
                                    string text = ASCIIEncoding.ASCII.GetString(buffer, 0, len).Replace("\n", "\r\n");
                                    textBox.Invoke(new Action(() => textBox.AppendText(text)));
                                }
                            }
                        }
                    }
                    catch (IOException) { }

                    Thread.Sleep(1000);
                }
            });

            textBox.Tag = file;
            this.logThreads.Add(textBox, thread);
            thread.Start();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.metricsLogThread != null)
            {
                this.metricsLogThread.Abort();
            }
            this.logThreads.ToList().ForEach(kv => this.ContinualFileLog(null, kv.Key));
            if (this.enableMetricsCheckbox.Checked)
            {
                this.enableMetricsCheckbox.Checked = false;
            }
        }

        private void captureLogsButton_Click(object sender, EventArgs e)
        {
            object lockObject = new object();
            DialogResult result = DialogResult.None;

            // Start capturing some information, while the save as dialog is open.
            Thread thread = new Thread(() =>
            {
                this.Morphic.LogEnvironment();

                // Wait until the dialog has been closed
                lock (lockObject)
                {
                    if (result == DialogResult.OK)
                    {
                        string metricsFile = null;
                        try
                        {
                            List<string> moreFiles = new List<string>();
                            // Include any metrics captured
                            metricsFile = Path.Combine(Path.GetTempPath(), "mdiag", "metrics-log.txt");
                            if (string.IsNullOrEmpty(this.metricsLogFile))
                            {
                                if (!string.IsNullOrEmpty(this.metricsLogText.Text))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(metricsFile));
                                    File.WriteAllText(metricsFile, this.metricsLogText.Text);
                                    moreFiles.Add(metricsFile);
                                }
                            }

                            this.Morphic.CaptureLogs(this.saveLogsDialog.FileName, moreFiles);
                        }
                        finally
                        {
                            this.Invoke(new Action(() =>
                            {
                                this.captureLogsButton.Enabled = true;
                                this.Cursor = Cursors.Default;
                                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.saveLogsDialog.FileName));
                            }));
                            if (metricsFile != null && File.Exists(metricsFile))
                            {
                                try
                                {
                                    File.Delete(metricsFile);
                                    Directory.Delete(Path.GetDirectoryName(metricsFile));
                                }
                                catch (IOException) { }
                            }
                        }
                    }
                }
            });

            lock (lockObject)
            {
                thread.Start();

                if (string.IsNullOrEmpty(this.saveLogsDialog.InitialDirectory))
                {
                    this.saveLogsDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    this.saveLogsDialog.FileName = "morphic-logs.zip";
                }

                result = this.saveLogsDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.captureLogsButton.Enabled = false;
                }
            }
        }

        private Regex metricsFilter;
        // Start or stop the metrics logging (also controls the filebeat service).
        private void enableMetricsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = this.enableMetricsCheckbox.Checked;

            if (this.metricsLogThread != null)
            {
                this.metricsLogThread.Abort();
            }

            if (enable)
            {
                if (this.metricsFilter == null)
                {
                    // Build the filter regex
                    string[] fields = { "module", "installID", "level", "version", "timestamp", "sequence" };
                    string re = @"\s*'(" + string.Join("|", fields) + @")'\s*:\s*('[^']*'|[-0-9.]*)\s*,?\s*";
                    this.metricsFilter = new Regex(re.Replace('\'', '"'));
                }

                // stop the filebeat service first, so the port can be bound upon.
                this.ControlService(false, "morphic-filebeat");

                Action<string> log = (string s) => {
                    if (this.saveMetricsLog)
                    {
                        File.AppendAllText(this.metricsLogFile, s);
                    }
                    this.metricsLogText.Invoke(new Action(() => {
                        if (s == null)
                        {
                            this.metricsLogText.Text = string.Empty;
                        }
                        else
                        {
                            if (this.metricsFilterCheckbox.Checked)
                            {
                                // Remove some less interesting fields from the json
                                s = this.metricsFilter.Replace(s, "");
                            }
                            this.metricsLogText.AppendText(s);
                        }
                    }));
                };
                this.metricsLogThread = new Thread((p) => this.MetricsLogServer(log));
                this.metricsLogThread.Start();
            } else
            {
                this.metricsLogThread = null;

                // start the filebeat service
                this.ControlService(true, "morphic-filebeat");
            }
        }

        private Thread metricsLogThread;

        // socket server, acting as the log server for metrics (dummy filebeat).
        private void MetricsLogServer(Action<string> log)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 51481);
            log(null);

            try
            {
                while (true)
                {
                    try
                    {
                        log("listening...");
                        listener.Start();
                        using (TcpClient client = listener.AcceptTcpClient())
                        using (NetworkStream stream = client.GetStream())
                        {
                            listener.Stop();
                            log(null);
                            byte[] buffer = new byte[0xffff];
                            int len;
                            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                string data = Encoding.ASCII.GetString(buffer, 0, len);
                                log(data.Replace("\n", "\r\n"));
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        return;
                    }
                    catch (Exception e)
                    {
                        this.Logger.Write(e);
                    }
                }
            }
            finally
            {
            }
        }

        // Toggle the saving of the metrics logs.
        private void metricsSaveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.metricsSaveFileLink.Enabled = this.metricsSaveCheckbox.Checked;
            if (this.metricsSaveCheckbox.Enabled && !string.IsNullOrEmpty(this.metricsLogFile))
            {
                this.StartMetricsLogSave();
            } else
            {
                this.saveMetricsLog = false;
            }
        }

        private string metricsLogFile = null;
        private bool saveMetricsLog = false;

        // Browse to save the metrics logs
        private void metricsSaveFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.metricsSaveCheckbox.Enabled)
            {
                if (string.IsNullOrEmpty(this.metricsSaveDialog.InitialDirectory))
                {
                    this.metricsSaveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    this.metricsSaveDialog.FileName = "metrics.txt";
                }

                if (this.metricsSaveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.metricsLogFile = this.metricsSaveDialog.FileName;
                    this.metricsSaveFileLink.Text = this.metricsLogFile;
                    this.StartMetricsLogSave();
                }
            } else
            {
                saveMetricsLog = false;
            }
        }

        // Start saving the logs
        private void StartMetricsLogSave()
        {
            try
            {
                this.saveMetricsLog = false;
                File.WriteAllText(this.metricsLogFile, this.metricsLogText.Text);
                this.saveMetricsLog = true;
            } catch (IOException e)
            {
                this.Logger.Write(e);
            }
        }
    }
}
