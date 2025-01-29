using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; //with jsonignore
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

[Table("TodoItem")]
public partial class TodoItem
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    public int TodoId { get; set; }

    [JsonIgnore]
    [ForeignKey("TodoId")]
    [InverseProperty("TodoItems")]
    public virtual Todo? Todo { get; set; } = null!;
}
