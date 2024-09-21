using EduViz.Common.Payloads.Request;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MentorDetails> MentorDetails { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<StudentQuizScore> StudentQuizScores { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<MentorSubject> MentorSubjects { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UpgradeOrderDetails> UpgradeOrderDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User and Comments
            modelBuilder.Entity<User>()
                .HasMany(u => u.comments)
                .WithOne(c => c.user)
                .HasForeignKey(c => c.userId)
                .OnDelete(DeleteBehavior.Restrict);

            // User and MentorDetails
            modelBuilder.Entity<User>()
                .HasOne(u => u.mentorDetails)
                .WithOne(m => m.user)
                .HasForeignKey<MentorDetails>(m => m.userId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorDetails and Courses
            modelBuilder.Entity<MentorDetails>()
                .HasMany(m => m.courses)
                .WithOne(c => c.mentor)
                .HasForeignKey(c => c.mentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course and Classes
            modelBuilder.Entity<Course>()
                .HasMany(c => c.classes)
                .WithOne(cl => cl.course)
                .HasForeignKey(cl => cl.courseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class and Posts
            modelBuilder.Entity<Class>()
                .HasMany(cl => cl.posts)
                .WithOne(p => p.mentorClass)
                .HasForeignKey(p => p.classId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class and Quizzes
            modelBuilder.Entity<Class>()
                .HasMany(cl => cl.quizzes)
                .WithOne(q => q.mentorClass)
                .HasForeignKey(q => q.classId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post and Comments
            modelBuilder.Entity<Post>()
                .HasMany(p => p.comments)
                .WithOne(c => c.post)
                .HasForeignKey(c => c.postId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quiz and Questions
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.questions)
                .WithOne(qu => qu.quiz)
                .HasForeignKey(qu => qu.quizId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserCourse and User
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.user)
                .WithMany(u => u.userCourses)
                .HasForeignKey(uc => uc.userId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserCourse and Course
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.course)
                .WithMany(c => c.userCourses)
                .HasForeignKey(uc => uc.courseId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorSubject and MentorDetails
            modelBuilder.Entity<MentorSubject>()
                .HasOne(ms => ms.mentor)
                .WithMany(m => m.mentorSubjects)
                .HasForeignKey(ms => ms.mentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorSubject and Subject
            modelBuilder.Entity<MentorSubject>()
                .HasOne(ms => ms.subject)
                .WithMany(s => s.mentorSubjects)
                .HasForeignKey(ms => ms.subjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and Student
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.student)
                .WithMany(u => u.payments)
                .HasForeignKey(p => p.studentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and MentorDetails
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.mentor)
                .WithMany(m => m.payments)
                .HasForeignKey(p => p.mentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and Course
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.course)
                .WithMany(c => c.payments)
                .HasForeignKey(p => p.courseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
