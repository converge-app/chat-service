using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class MessageCreationDto
    {
    
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string RecieverId { get; set; }
        [Required]
        public string message { get; set; }
       
    }
}