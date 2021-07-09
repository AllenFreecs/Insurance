using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Insurance.DL.Entities
{
    public partial class InsuranceDBContext : DbContext
    {
        public InsuranceDBContext()
        {
        }

        public InsuranceDBContext(DbContextOptions<InsuranceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<InsuranceInfo> InsuranceInfo { get; set; }
        public virtual DbSet<InsuranceInfoDetail> InsuranceInfoDetail { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistory { get; set; }
        public virtual DbSet<Setup> Setup { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=PCM-FF4R7D3\\SQL2016;initial catalog=InsuranceDB;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<InsuranceInfo>(entity =>
            {
                entity.Property(e => e.BasicSalary).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<InsuranceInfoDetail>(entity =>
            {
                entity.Property(e => e.Benefits).IsUnicode(false);

                entity.Property(e => e.BenefitsAmountQuotation).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.PendedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PasswordHistory>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Setup>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GuaranteedIssue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GUID).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }

        public void AddAuditTimeStamp()
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            //int? userid = _httpContext.HttpContext.User.Identity.Name != null ? Convert.ToInt32(((ClaimsIdentity)_httpContext.HttpContext.User.Identity).FindFirst(ClaimTypes.Name).Value) : (int?)null;


            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity.GetType().GetProperty("CreatedDate") != null)
                    {
                        entity.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    }

                    if (entity.Entity.GetType().GetProperty("UpdatedDate") != null)
                    {
                        entity.Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
                    }

                    if (entity.Entity.GetType().GetProperty("CreatedBy") != null)
                    {
                        //entity.Property("CreatedBy").CurrentValue = userid == null ? entity.Property("CreatedBy").CurrentValue == null ? 1 : entity.Property("CreatedBy").CurrentValue : userid;
                        entity.Property("CreatedBy").CurrentValue = 1;
                    }

                    if (entity.Entity.GetType().GetProperty("UpdatedBy") != null)
                    {
                        entity.Property("UpdatedBy").CurrentValue = 1;
                        //entity.Property("UpdatedBy").CurrentValue = userid == null ? entity.Property("UpdatedBy").CurrentValue == null ? 1 : entity.Property("UpdatedBy").CurrentValue : userid;
                    }
                }

                if (entity.State == EntityState.Modified)
                {
                    if (entity.Entity.GetType().GetProperty("UpdatedDate") != null)
                    {

                        entity.Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
                    }

                    if (entity.Entity.GetType().GetProperty("UpdatedBy") != null)
                    {
                        entity.Property("UpdatedBy").CurrentValue = 1;
                        //entity.Property("UpdatedBy").CurrentValue = userid == null ? entity.Property("UpdatedBy").CurrentValue : userid;
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            AddAuditTimeStamp();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddAuditTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
