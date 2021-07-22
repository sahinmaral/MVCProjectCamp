using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProjeKampi.Models.ViewModels
{
    public class CountOfMessagesViewModel
    {
        public int ReceivedMessageCount { get; set; }
        public int SentMessageCount { get; set; }
        public int ContactCount { get; set; }
        public int DraftCount { get; set; }
        public int ArchiveCount { get; set; }
    }
}