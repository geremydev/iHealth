using iHealth.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Contexts
{
    public class iHealthContext : DbContext
    {
        public iHealthContext(DbContextOptions<iHealthContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabResult> LabResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
                modelBuilder.Entity<Doctor>().ToTable("Doctors");
                modelBuilder.Entity<Patient>().ToTable("Patients");
                modelBuilder.Entity<User>().ToTable("Users");
                modelBuilder.Entity<Appointment>().ToTable("Appointments");
                modelBuilder.Entity<LabTest>().ToTable("LaboratoryTests");
                modelBuilder.Entity<LabResult>().ToTable("LaboratoryResults");
            #endregion

            #region Primary Keys
                modelBuilder.Entity<Patient>().HasKey(p => p.Id);
                modelBuilder.Entity<User>().HasKey(d => d.Id);
                modelBuilder.Entity<Appointment>().HasKey(a => a.Id);
                modelBuilder.Entity<LabTest>().HasKey(lt => lt.Id);
                modelBuilder.Entity<LabResult>().HasKey(lr => lr.Id);
            #endregion

            #region Relationships

                modelBuilder.Entity<Doctor>()
                    .HasMany(d => d.Appointments)
                    .WithOne(a => a.Doctor)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Patient>()
                    .HasMany(p => p.Appointments)
                    .WithOne(a => a.Patient)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<LabTest>()
                    .HasMany(lt => lt.LabResults)
                    .WithOne(lr => lr.LabTest)
                    .HasForeignKey(lr => lr.LabTestId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Appointment>()
                    .HasMany(a => a.LabResult)
                    .WithOne(lr => lr.Appointment)
                    .HasForeignKey(lr => lr.AppointmentId)
                    .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Properties Configuration

            #region Doctor
            modelBuilder.Entity<Doctor>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Doctor>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.IdCard)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.ImageURL)
                .IsRequired()
                .HasMaxLength(100);
            #endregion

            #region Patient
            modelBuilder.Entity<Patient>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Patient>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.BirthDate)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Address)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Smoker)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Allergies)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.IdCard)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.ImageURL)
                .IsRequired()
                .HasMaxLength(100);

            #endregion

            #region User

            modelBuilder.Entity<User>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(64);
            #endregion

            #region Appointment

            modelBuilder.Entity<Appointment>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.PatientId)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.DoctorId)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentDate)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.MedicalConcern)
                .HasColumnType("nvarchar(max)");

            #endregion

            #region LabTest
            modelBuilder.Entity<LabTest>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<LabTest>()
                .Property(lt => lt.Name)
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            #endregion

            #region LabResult
            modelBuilder.Entity<LabResult>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<LabResult>()
                .Property(lr => lr.AppointmentId)
                .IsRequired();

            modelBuilder.Entity<LabResult>()
                .Property(lr => lr.LabTestId)
                .IsRequired();

            modelBuilder.Entity<LabResult>()
                .Property(lr => lr.Result)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<LabResult>()
                .Property(lr => lr.Completed)
                .HasMaxLength(50);
            #endregion

            #endregion
        }
    }
}
