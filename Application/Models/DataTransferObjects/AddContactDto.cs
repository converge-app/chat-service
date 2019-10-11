using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class AddContactDTO
    {
    
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string RecieverId { get; set; }

    }
}