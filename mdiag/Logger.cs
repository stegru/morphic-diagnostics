using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace mdiag
{
    public class Logger
    {
        public event EventHandler<string> Log;

        public Logger()
        {
        }

        public int Program(bool elevate, string command, params string[] arguments)
        {
            this.Write("Executing: ", command, string.Join(" ", arguments));

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = string.Join(" ", arguments),

                    UseShellExecute = elevate,
                    RedirectStandardOutput = !elevate,
                    RedirectStandardError = !elevate,
                    CreateNoWindow = true,
                }
            };

            if (elevate)
            {
                p.StartInfo.Verb = "runas";
            }

            p.Start();

            if (!elevate)
            {
                this.OnLog(p.StandardOutput.ReadToEnd());
                this.OnLog(p.StandardError.ReadToEnd());
            }
            p.WaitForExit();
            return p.ExitCode;
        }

        public int Command(string commandLine, bool elevate = false)
        {
            return this.Program(elevate, "cmd.exe", "/C", "\"" + commandLine + "\"");
        }

        public void Write(params string[] text)
        {
            this.OnLog(string.Join(" ", text));
        }

        public void Write(Exception e)
        {
            this.Write(e.Message);
            this.Write(e.ToString());
        }

        public void WritePair(string key, object value)
        {
            this.Write(key, "=", value == null ? "<null>" : value.ToString());
        }

        public void Write<Tk,Tv>(KeyValuePair<Tk, Tv> valuePair)
        {
            string key = valuePair.Key == null ? "<null>" : valuePair.Key.ToString();
            this.WritePair(key, valuePair.Value);
        }

        public void Write<Tk,Tv>(IEnumerable<KeyValuePair<Tk, Tv>> dict)
        {
            dict.ToList().ForEach(item => this.Write(item));
        }

        public void Properties(object obj, BindingFlags binding = BindingFlags.Public)
        {
            this.Properties(obj, null);
        }
        public void Properties(object obj, string[] exclude, BindingFlags binding = BindingFlags.Public)
        {
            Type type;
            if (obj is Type)
            {
                type = (Type)obj;
                obj = null;
                binding |= BindingFlags.Static;
            }
            else
            {
                type = obj.GetType();
                binding |= BindingFlags.Instance;
            }

            foreach (PropertyInfo pi in type.GetProperties(binding))
            {
                if (exclude == null || !exclude.Contains(pi.Name))
                {
                    object value;
                    try
                    {
                        value = pi.GetValue(obj);
                    } catch (Exception e)
                    {
                        value = "<Exception:" + (e.InnerException ?? e).Message + ">";
                    }
                    string str;
                    if (value == null)
                    {
                        str = "(null)";
                    }
                    else
                    {
                        str = value.ToString();
                    }

                    this.Write(pi.Name, "=", str);
                }
            }
        }

        private void OnLog(string text)
        {
            this.Log?.Invoke(this, text);
        }

        internal void Write(object p)
        {
            throw new NotImplementedException();
        }
    }
}
