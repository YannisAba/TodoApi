using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<TodoItem> TodoItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=IOANNIS_DELL\\SQLEXPRESS;Database=TodoApi_db;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Todo__3214EC078008F491");

            entity.HasOne(d => d.User).WithMany(p => p.Todos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Todo__UserId__440B1D61");
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TodoItem__3214EC07024A3943");

            entity.HasOne(d => d.Todo).WithMany(p => p.TodoItems).HasConstraintName("FK__TodoItem__TodoId__46E78A0C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0707A7F78F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
