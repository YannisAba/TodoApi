using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; //with jsonignore
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

[Table("Todo")]
public partial class Todo
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

    [InverseProperty("Todo")]
    public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

    [JsonIgnore]
    [ForeignKey("UserId")]
    [InverseProperty("Todos")]
    public virtual User? User { get; set; }
}
