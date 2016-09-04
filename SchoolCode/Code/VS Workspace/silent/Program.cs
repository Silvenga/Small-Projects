using System.Diagnostics;

namespace silent {
    public static class Program {

        static void Main(string[] args) {

            args = Parse(args);

            ProcessStartInfo startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = args[0],
                Arguments = args[1],
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process process = new Process {
                StartInfo = startInfo
            };

            process.Start();
        }

        public static string[] Parse(string[] args) {

            string[] parsed = new string[2];

            parsed[0] = args[0];

            for(int i = 1; i < args.Length; i++)
                parsed[1] += args[i] + " ";

            return parsed;
        }
    }
}
