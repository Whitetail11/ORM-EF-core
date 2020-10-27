using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public int FacultyId { get; set; }
    }
    class Student
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
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
            UniversityContext universityContext = new UniversityContext();
             void CreateUniversity()
            {                
                if(!universityContext.Faculties.Any())
                {
                    using var transaction = universityContext.Database.BeginTransaction();
                    universityContext.Faculties
                       .Add(
                          new Faculty
                          {
                              Name = "Faculty1"
                          });
                    universityContext.SaveChanges();
                    universityContext.Groups
                        .AddRange(
                        new Group
                        {
                            Name = "Group1",
                            FacultyId = 1
                        },
                         new Group
                         {
                             Name = "Group2",
                             FacultyId = 1
                         }
                        );
                    universityContext.SaveChanges();
                    universityContext.Students
                        .AddRange(
                        new Student
                        {
                            Name = "Student from group1",
                            GroupId = 1
                        },
                        new Student
                        {
                            Name = "Student from group1",
                            GroupId = 1
                        },
                         new Student
                         {
                             Name = "Student from group2",
                             GroupId = 2
                         },
                        new Student
                        {
                            Name = "Student from group2",
                            GroupId = 2
                        }
                        );
                    universityContext.SaveChanges();
                    transaction.Commit();
                }              
            }
            CreateUniversity();
            IEnumerable<Faculty> facs = universityContext.Faculties.Include(f => f.Groups).ThenInclude(g => g.Students).ToList();
            Console.WriteLine("Faculty:");
                foreach (Faculty val in facs)
                    {
                        Console.WriteLine(val.Name);
                foreach(Group group in val.Groups)
                {
                    Console.WriteLine("Group:");
                    Console.WriteLine(group.Name);
                    foreach(Student student in group.Students)
                    {
                        Console.WriteLine("Student:");
                        Console.WriteLine(student.Name);
                    }
                }
            }
        }
    }
}
