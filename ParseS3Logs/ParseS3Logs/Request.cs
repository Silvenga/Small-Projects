namespace ParseS3Logs
{
    public class Request
    {
        public string Method { get; set; }
        public string RequestUri { get; set; }
        public string HttpVersion { get; set; }

        public Request(string request)
        {
            if (request == null)
            {
                return;
            }

            var tokens = request.Split(' ');

            Method = tokens[0];
            RequestUri = tokens.Length > 1 ? tokens[1] : null;
            HttpVersion = tokens.Length > 2 ? tokens[2] : null;
        }

        public override string ToString()
        {
            return $"{Method} {RequestUri} {HttpVersion}";
        }
    }
}