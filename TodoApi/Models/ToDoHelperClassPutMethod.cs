using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ToDoHelperClassPutMethod
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; } = null!;

        public int? UserId { get; set; }
    }
}
