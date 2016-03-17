namespace ParseS3Logs
{
    using System;

    public class Operation
    {
        public OperationType Type { get; set; }
        public string Method { get; set; }
        public string ResourceType { get; set; }

        public Operation(string operation)
        {
            var tokens = operation.Split('.');

            OperationType type;
            Enum.TryParse(tokens[0], true, out type);
            Type = type;

            Method = tokens[1];
            ResourceType = tokens.Length > 2 ? tokens[2] : null;
        }

        public override string ToString()
        {
            return $"{Type.ToString().ToUpper()}.{Method}.{ResourceType}";
        }
    }
}