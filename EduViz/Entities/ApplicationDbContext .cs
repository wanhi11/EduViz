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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User and Comments
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User and MentorDetails
            modelBuilder.Entity<User>()
                .HasOne(u => u.MentorDetails)
                .WithOne(m => m.User)
                .HasForeignKey<MentorDetails>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorDetails and Courses
            modelBuilder.Entity<MentorDetails>()
                .HasMany(m => m.Courses)
                .WithOne(c => c.Mentor)
                .HasForeignKey(c => c.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course and Classes
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Classes)
                .WithOne(cl => cl.Course)
                .HasForeignKey(cl => cl.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class and Posts
            modelBuilder.Entity<Class>()
                .HasMany(cl => cl.Posts)
                .WithOne(p => p.Class)
                .HasForeignKey(p => p.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class and Quizzes
            modelBuilder.Entity<Class>()
                .HasMany(cl => cl.Quizzes)
                .WithOne(q => q.Class)
                .HasForeignKey(q => q.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post and Comments
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quiz and Questions
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(qu => qu.Quiz)
                .HasForeignKey(qu => qu.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserCourse and User
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserCourse and Course
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorSubject and MentorDetails
            modelBuilder.Entity<MentorSubject>()
                .HasOne(ms => ms.Mentor)
                .WithMany(m => m.MentorSubjects)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorSubject and Subject
            modelBuilder.Entity<MentorSubject>()
                .HasOne(ms => ms.Subject)
                .WithMany(s => s.MentorSubjects)
                .HasForeignKey(ms => ms.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and Student
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and MentorDetails
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Mentor)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment and Course
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
