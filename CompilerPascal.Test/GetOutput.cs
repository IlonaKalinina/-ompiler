using System;
using System.Diagnostics;
using System.IO;

namespace CompilerPascal.Test
{
    public class GetOutput
    {
        public static (string, string) RunAndGetOutputs(string path, string keys)
        {
            var inputPath = $"{path}.in";
            var answerPath = $"{path}.ans";

            var output = RunAndGetOutput(inputPath, keys);
            var rightAnswer = new StreamReader(answerPath).ReadToEnd();

            return (output, rightAnswer);
        }

        public static string RunAndGetOutput(string path, string compileKeys)
        {
            var args = $"\"{path}\" {compileKeys}";
            using var proc = Process.Start(@"..\..\..\..\CompilerPascal\bin\Debug\netcoreapp3.1\CompilerPascal.exe", args);
            if (proc != null)
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                var res = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                return res;
            }
            throw new ArgumentException("path is not correct");
        }
    }
}
