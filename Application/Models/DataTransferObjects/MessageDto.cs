using System;

namespace Application.Models.DataTransferObjects
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string ContactId { get; set; }
        public string message { get; set; }
        public long Timestamp { get; set; }
    }
}