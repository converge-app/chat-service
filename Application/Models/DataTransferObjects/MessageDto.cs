using System;

namespace Application.Models.DataTransferObjects
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string ContactId { get; set; }
        public string Content { get; set; }
        public long Timestamp { get; set; }
    }
}