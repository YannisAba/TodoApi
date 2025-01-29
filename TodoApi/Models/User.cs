using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

[Index("Username", Name = "UQ__Users__536C85E45A048E88", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D10534F0F5D5CD", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
