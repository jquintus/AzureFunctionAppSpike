using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class Function1
    {
        public const string INTERNAL_QUEUE = "internal-messages";
        public const string O365_QUEUE = "o365-message";
        public const string PARSED_QUEUE = "parsed-messages";

        public static int _count = 0;

        [FunctionName(nameof(MultipleOutputExample))]
        public static void MultipleOutputExample(
            [QueueTrigger("BBB", Connection = "")]string input,
            [Queue("BBB-1")] ICollector<string> q1,
            [Queue("BBB-2")] ICollector<string> q2,
            [Queue("BBB-3")] out string output
            )
        {
            q1.Add($"ONE {input}");

            q2.Add($"TWO {input}");
            q2.Add($"TWO {input}");

            output = $"Output for {input}";
        }

        [FunctionName(nameof(Run))]
        [return: Queue(O365_QUEUE)]
        public static InternalMessage Run(
            [QueueTrigger(INTERNAL_QUEUE, Connection = "")]O365Message myQueueItem,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            var next = new InternalMessage
            {
                Subject = myQueueItem.Subject,
                Body = myQueueItem.Body,
                From = myQueueItem.From,
                To = myQueueItem.To,
            };

            if (_count++ % 2 == 0 || myQueueItem.Subject == "fail")
            {
                throw new System.Exception("failing!");
            }

            //{
            //    Subject = "sub",
            //    Body = "bod",
            //    From = "from",
            //    To = "to",
            //};

            return next;
        }

        [FunctionName(nameof(Run2))]
        [return: Queue(PARSED_QUEUE)]
        public static ParsedMessage Run2(
            [QueueTrigger(O365_QUEUE, Connection = "")]InternalMessage myQueueItem,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");

            var next = new ParsedMessage
            {
                Subject = myQueueItem.Subject,
                Body = myQueueItem.Body,
                Participants = new List<string>
                {
                    myQueueItem.From,
                    myQueueItem.To,
                },
            };

            return next;
        }
    }
}