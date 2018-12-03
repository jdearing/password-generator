using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PasswordGenerator
{
    /// <summary>
    /// Cross paltform copy to clipboard, adapted from https://stackoverflow.com/questions/44205260/net-core-copy-to-clipboard
    /// http://www.robvanderwoude.com/escapechars.php
    /// </summary>
    public static class Clipboard
    {
        public static void SetText(string val)
        {
            string cmd, escapedArgs;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                val = val.Replace("%", "%%");
                val = val.Replace("^", "^^");
                val = val.Replace("&", "^&");
                val = val.Replace("<", "^<");
                val = val.Replace(">", "^>");
                val = val.Replace("|", "^|");

                cmd = "cmd.exe";
                escapedArgs = $"echo {val} | clip";
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
            
            string value = Run(cmd, escapedArgs);
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