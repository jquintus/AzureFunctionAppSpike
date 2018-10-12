namespace FunctionApp1
{
    public class InternalMessage
    {
        public string Body { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }

        public override string ToString() => $"Internal Message {Subject}";
    }
}