namespace ParseS3Logs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("program <log path>");
                return;
            }
            var path = new DirectoryInfo(args[0]);
            var files = path
                .EnumerateFiles()
                .ToList();

            var logs = files
                .SelectMany(ParseLog)
                .Where(x => x.Operation.Type == OperationType.Website)
                .OrderBy(x => x.Time)
                .ToList();

            var converted = logs
                .ToApacheLogs()
                .ToList();

            foreach (var line in converted)
            {
                Console.WriteLine(line);
            }

            foreach (var file in files)
            {
                file.Delete();
            }
        }

        public static IEnumerable<Log> ParseLog(FileInfo path)
        {
            var lines = File.ReadAllLines(path.FullName).Select(x => new Log(x.Replace("[", "\"").Replace("]", "\"")));
            return lines;
        }

        public static IEnumerable<string> ToApacheLogs(this IEnumerable<Log> logs)
        {
            foreach (var log in logs)
            {
                // %h %l %u %t "%r" %>s %b "%{Referer}i" "%{User-Agent}i"
                //< Remote host > < Remote logname > < Remote user > < Time of the request[19/Sep/2015:22:39:16 -0500 > < First line of request[GET / HTTP / 1.1] > < Http Status > < Size of response in bytes > < Referer > < User - Agent >

                var line = $"{log.RemoteIp} - - [{log.Time.ToString("dd/MMM/yyyy:HH:mm:ss zz00")}] \"{log.RequestUri}\" {log.HttpStatus} {(log.BytesSent == 0 ? "-" : log.BytesSent.ToString())} \"{log.Referrer ?? "-"}\" \"{log.UserAgent ?? "-"}\"";

                yield return line;
            }
        }
    }
}
