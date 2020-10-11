using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.CommonServices.Models
{
    public class QnAKnowledgeBase
    {
        public string Host { get; set; }
        public string Product { get; set; }
        public string KnowledgeBaseId { get; set; }
        public string EndpointKey { get; set; }
    }
}
