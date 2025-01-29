using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class UserHelper
    {
       
        [StringLength(255)]
        public string Username { get; set; } = null!;

        [StringLength(255)]
        public string Password { get; set; } = null!;

        [StringLength(255)]
        public string Email { get; set; } = null!;
    }
}
