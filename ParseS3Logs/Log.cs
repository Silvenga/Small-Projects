namespace ParseS3Logs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Log
    {
        public string BucketOwner { get; set; }
        public string Bucket { get; set; }
        public DateTime Time { get; set; }
        public string RemoteIp { get; set; }
        public string Requester { get; set; }
        public string RequestId { get; set; }
        public Operation Operation { get; set; }
        public string Key { get; set; }
        public Request RequestUri { get; set; }
        public string HttpStatus { get; set; }
        public string ErrorCode { get; set; }
        public long BytesSent { get; set; }
        public long ObjectSize { get; set; }
        public long TotalTime { get; set; }
        public long TurnAroundTime { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public string VersionId { get; set; }

        public bool IsSuccess => ErrorCode == null;

        public Log(string str)
        {
            var segs = Tokenize(str.Split(' ')).Select(x => x.Replace("\"", "")).Select(x => x == "-" ? null : x).ToArray();

            var i = 0;
            BucketOwner = segs[i++];
            Bucket = segs[i++];
            var a = segs[i++];
            Time = DateTime.ParseExact(a, "dd/MMM/yyyy:HH:mm:ss zz00", CultureInfo.CurrentCulture); // [06/Feb/2014:00:00:38 +0000]
            RemoteIp = segs[i++];
            Requester = segs[i++];
            RequestId = segs[i++];
            Operation = new Operation(segs[i++]);
            Key = segs[i++];
            RequestUri = segs[i++] == null ? null : new Request(segs[i - 1]);
            HttpStatus = segs[i++];
            ErrorCode = segs[i++];
            BytesSent = long.Parse(segs[i++] ?? "-1");
            ObjectSize = long.Parse(segs[i++] ?? "-1");
            TotalTime = long.Parse(segs[i++] ?? "-1");
            TurnAroundTime = long.Parse(segs[i++] ?? "-1");
            Referrer = segs[i++];
            UserAgent = segs[i++];
            VersionId = segs[i];
        }

        private static IEnumerable<string> Tokenize(IEnumerable<string> strings)
        {
            var stream = strings.GetEnumerator();

            while (stream.MoveNext())
            {
                if (stream.Current.StartsWith("\"") && !stream.Current.EndsWith("\""))
                {
                    yield return UntilEnd(stream.Current, stream);
                }
                else
                {
                    yield return stream.Current;
                }
            }
        }

        private static string UntilEnd(string current, IEnumerator<string> stream)
        {
            var segment = current + " ";
            while (stream.MoveNext() && !stream.Current.EndsWith("\""))
            {
                segment += stream.Current + " ";
            }

            return segment + stream.Current;
        }
    }
}