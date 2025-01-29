using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ToDoItemHelperClass
    {
        [StringLength(255)]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        public int TodoId { get; set; }

    }
}
