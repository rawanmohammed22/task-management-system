using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

using Core.Interfaces;

namespace Infrastructure.Data
{
    public class TaskTrackerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<Attacht> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقة ApplicationUser -> TaskItem كـ Creator
            modelBuilder.Entity<TaskItem>()
         .HasOne(t => t.Creator)
         .WithMany(u => u.CreatedTasks)
         .HasForeignKey(t => t.CreatorId)
         .OnDelete(DeleteBehavior.SetNull);

            // علاقة Assignee
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.Project)
         .WithMany(p => p.Tasko)
         .HasForeignKey(t => t.ProjectId)
         .OnDelete(DeleteBehavior.SetNull); // ✅ الحل

            modelBuilder.Entity<Project>()
    .HasOne(p => p.Owner)
    .WithMany()
    .HasForeignKey(p => p.OwnerId)
    .OnDelete(DeleteBehavior.SetNull); // <--- المهم




            modelBuilder.Entity<UserProject>()
       .HasKey(up => new { up.UserId, up.ProjectId });

            // تكوين العلاقات
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(up => up.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);



            modelBuilder.Entity<Attacht>(entity =>
            {
                // التكوين الأساسي (موجود بالفعل من Id)
                entity.HasKey(a => a.Id);

                // علاقة مع TaskItem
                entity.HasOne(a => a.Tasks)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(a => a.TaskId)
                      .OnDelete(DeleteBehavior.SetNull);

                // علاقة مع ApplicationUser
                entity.HasOne(a => a.Uploader)
                      .WithMany(u => u.Attachments) // تأكد من وجود هذه الخاصية في ApplicationUser
                      .HasForeignKey(a => a.UploaderId)
                      .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<SubTask>(entity =>
            {
                entity.HasOne(s => s.Task)
                      .WithMany(t => t.SubTasks)
                      .HasForeignKey(s => s.TaskId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 5. تكوين Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.Task)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(c => c.TaskId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(c => c.Author)
                      .WithMany()
                      .HasForeignKey(c => c.AuthorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });




            // باقي التهيئات:
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskTrackerDbContext).Assembly);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.Priority);
                entity.HasIndex(t => t.DueDate);
            });

            // Seed للRoles
            SeedData(modelBuilder);
        }


        private void SeedData(ModelBuilder modelBuilder)
        {
            // إضافة أدوار النظام
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" }
            );

            // يمكنك إضافة بيانات أولية أخرى هنا
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // معالجة التواريخ التلقائية
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
