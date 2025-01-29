using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ToDoHelperClass
    {
        [StringLength(255)]
        public string Name { get; set; } = null!;

        public int? UserId { get; set; }
    }
}
