using System.Collections.Generic;

namespace FunctionApp1
{
    public class ParsedMessage
    {
        public string Body { get; set; }
        public List<string> Participants { get; set; }
        public string Subject { get; set; }

        public override string ToString() => $"Parsed Message {Subject}";
    }
}