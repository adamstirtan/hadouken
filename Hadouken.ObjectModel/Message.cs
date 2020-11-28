using System;

namespace Hadouken.ObjectModel
{
    public class Message : EntityBase
    {
        public string UserName { get; set; }
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}