using BuildCommon.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace BuildCommon.Execution
{
    public class Executor
    {
        #region Properties
        public string BinaryPath { get; set; }
        public string FileName { get; set; }
        public string FullPath => Path.Combine(BinaryPath, FileName);
        public string LogClass { get; set; }
        public string WorkingDirectory { get; set; }
        #endregion

        #region Constructor
        public Executor()
        {
            WorkingDirectory = Environment.CurrentDirectory;
        }
        #endregion
        public int Run(string Arguments)
        {
            ProcessStartInfo pif = new ProcessStartInfo
            {
                FileName = FullPath,
                WorkingDirectory = WorkingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = Arguments
            };

            var p = Process.Start(pif);
            p.OutputDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Log.Write(e.Data, LogClass); };
            p.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Log.Write(e.Data, LogClass); };

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.WaitForExit();

            int res = p.ExitCode;

            p.Close();

            return res;
        }
    }


}
