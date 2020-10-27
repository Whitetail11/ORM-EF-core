using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORM_EF_core
{
    class Faculty
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
    class Group
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
        public Faculty Faculty { get; set; }
    }
    class Student
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public Group Group { get; set; }
    }
    class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-IRISS61;Database=UniversityDataBase;Trusted_Connection=True");
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group);

            modelBuilder.Entity<Faculty>()
                .HasMany(f => f.Groups)
                .WithOne(g => g.Faculty);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
