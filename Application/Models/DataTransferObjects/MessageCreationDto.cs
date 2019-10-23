using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class MessageCreationDto
    {

        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Content { get; set; }

    }
}