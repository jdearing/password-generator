using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace PasswordGenerator
{
    /// <summary>
    /// Cross platform copy to clipboard, adapted from https://stackoverflow.com/questions/44205260/net-core-copy-to-clipboard
    /// </summary>
    public static class Clipboard
    {
        public static void SetText(string val)
        {
            string cmd, escapedArgs, file = null;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                file = Path.GetTempFileName();
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(val);
                }

                cmd = "cmd.exe";
                escapedArgs = $"CLIP < {file}";
                escapedArgs = escapedArgs.Replace("\"", "\\\"");
                escapedArgs = $"/c \"{escapedArgs}\"";
            }
            else
            {
                cmd = "/bin/bash";
                escapedArgs = $"echo \"{val}\" | pbcopy";
                escapedArgs = escapedArgs.Replace("\"", "\\\"");
                escapedArgs = $"-c \"{escapedArgs}\"";
            }

            _ = Run(cmd, escapedArgs);

            if (file != null) File.Delete(file);
        }

        private static string Run(string filename, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };
            process.Start();

            string result = process.StandardOutput.ReadToEnd(); 

            process.WaitForExit();
            
            return result;
        }
    }
}