
namespace CMS.Infrastructure.Data
{
    public class CMS_DBContext(DbContextOptions<CMS_DBContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Description).HasMaxLength(1000);
                entity.Property(c => c.Price).HasPrecision(18, 2).IsRequired();
                entity.Property(c => c.CourseType).HasConversion<string>().HasMaxLength(50);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(s => s.Email).IsUnique();
            });
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollments");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.EnrolledAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
                // i could make this a composite key but i prefer to keep the surrogate key for simplicity and flexibility
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique(); // Ensure a student can enroll in a course only once
                entity.Property(e => e.Status).HasConversion<string>();
            });

        }
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    }
}
