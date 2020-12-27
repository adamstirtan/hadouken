using System;

namespace Hadouken.ObjectModel
{
    public class Message : BaseEntity
    {
        public string UserName { get; set; }
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}