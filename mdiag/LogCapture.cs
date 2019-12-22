using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdiag
{
    /// <summary>
    /// Captures several log files of morphic.
    /// </summary>
    public class LogCapture
    {
        private Logger logger;

        public string MorphicDirectory { get; set; }
        public string ServiceDirectory { get; set; }
        public List<string> ExtraFiles { get; private set; } = new List<string>();

        public LogCapture(Logger logger)
        {
            this.logger = logger;
        }

        public void ToZip(string zipFile)
        {
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh.mm.ss");
            string workDir = Path.Combine(Path.GetTempPath(), "mdiag-zip", timestamp);
            Directory.CreateDirectory(workDir);

            this.logger.Write("Saving logs to", zipFile);

            string roboOptions = String.Join(" ",
                "/s", // subdirectories
                "/max:5000000" // smaller than 5mb
                );

            int result;
            this.logger.Write("Collecting service logs", this.ServiceDirectory);
            this.robocopy(this.ServiceDirectory, Path.Combine(workDir, "service"), roboOptions);

            this.logger.Write("Collecting morphic logs", this.MorphicDirectory);
            result = this.robocopy(this.MorphicDirectory, Path.Combine(workDir, "morphic"), roboOptions);

            this.ExtraFiles.ForEach(f =>
            {
                this.logger.Write("Collecting ", f);
                try
                {
                    File.Copy(f, Path.Combine(workDir, Path.GetFileName(f)), true);
                } catch (IOException e)
                {
                    this.logger.Write(e);
                }
            });

            this.logger.Write("Compressing logs");
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            ZipFile.CreateFromDirectory(workDir, zipFile);

            this.logger.Write();

            try
            {
                Directory.Delete(workDir, true);
            }
            catch (IOException) { }

            this.logger.Write("Logs saved in", zipFile);
        }

        private int robocopy(string from, string to, string options)
        {
            string command = string.Format("robocopy \"{0}\" \"{1}\" {2}", from, to, options);
            return this.logger.Command(command);
        }
    }
}
