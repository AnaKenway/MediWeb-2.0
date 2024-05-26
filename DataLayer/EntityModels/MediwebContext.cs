using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DataLayer;

public partial class MediWebContext : IdentityDbContext<UserAccount, IdentityRole<long>, long>
{
    public MediWebContext()
    {
    }

    public MediWebContext(DbContextOptions<MediWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentSlot> AppointmentSlots { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorClinics> DoctorClinics { get; set; }

    public virtual DbSet<MedicalEmployee> MedicalStaffs { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    //public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=mediweb_code_first;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.HasIndex(e => e.UserAccountId, "admin_user_account_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminType).HasColumnName("admin_type");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

            entity.HasOne(d => d.UserAccount).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_user_account_fkey");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.AppointmentSlotId, "appointment_appointment_slot_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentSlotId).HasColumnName("appointment_slot_id");
            entity.Property(e => e.IsApproved)
                .HasDefaultValueSql("(0)::bit(1)")
                .HasColumnType("bit(1)")
                .HasColumnName("is_approved");
            entity.Property(e => e.Note)
                .HasColumnType("character varying")
                .HasColumnName("note");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");

            entity.HasOne(d => d.AppointmentSlot).WithOne(p => p.Appointment)
                .HasForeignKey<Appointment>(d => d.AppointmentSlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_appointment_slot_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_patient_fkey");
        });

        modelBuilder.Entity<AppointmentSlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_slot_pkey");

            entity.ToTable("appointment_slot");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.DateAndTime).HasColumnName("date_and_time");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.DurationInMinutes).HasColumnName("duration_in_minutes");

            entity.HasOne(d => d.Clinic).WithMany(p => p.AppointmentSlots)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_slot_clinic_fkey");

            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentSlots)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_slot_doctor_fkey");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clinic_pkey");

            entity.ToTable("clinic");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.Pib)
                .HasMaxLength(100)
                .HasColumnName("pib");
            entity.Property(e => e.WorkHours)
                .HasMaxLength(200)
                .HasColumnName("work_hours");

            entity.HasMany(c => c.Doctors)
                .WithMany(d => d.Clinics)
                .UsingEntity<DoctorClinics>();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("doctor_pkey");

            entity.ToTable("doctor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.UserAccount).WithOne(d => d.Doctor)
                .HasConstraintName("doctor_user_account_fkey");

            entity.HasMany(d => d.Clinics)
                .WithMany(c => c.Doctors)
                .UsingEntity<DoctorClinics>();

            entity.HasMany(d => d.Specializations)
                .WithMany(s => s.Doctors)
                .UsingEntity<DoctorClinics>();
        });

        modelBuilder.Entity<DoctorClinics>(entity =>
        {
            entity.HasKey(e => new { e.DoctorId, e.ClinicId, e.SpecializationId }).HasName("doctor_clinics_pkey");

            entity.ToTable("doctor_clinics");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .HasColumnName("note");
        });

        modelBuilder.Entity<MedicalEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medical_employee_pkey");

            entity.ToTable("medical_employee");

            entity.HasIndex(e => e.UserAccountId, "medical_employee_user_account_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

            entity.HasOne(d => d.Clinic).WithMany(p => p.MedicalEmployees)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medical_employee_clinic_fkey");

            entity.HasOne(d => d.UserAccount).WithOne(p => p.MedicalEmployee)
                .HasForeignKey<MedicalEmployee>(d => d.UserAccountId)
                .HasConstraintName("medical_employee_user_account_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patient_pkey");

            entity.ToTable("patient");

            entity.HasIndex(e => e.UserAccountId, "patient_user_account_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Jmbg)
                .HasColumnType("character varying")
                .HasColumnName("jmbg");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

            entity.HasOne(d => d.UserAccount).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserAccountId)
                .HasConstraintName("patient_user_account_fkey");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialization_pkey");

            entity.ToTable("specialization");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SpecializationName)
                .HasColumnName("specialization_name")
                .HasColumnType("character varying");
        });

        //modelBuilder.Entity<UserAccount>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("user_account_pkey");

        //    entity.ToTable("user_account");

        //    entity.Property(e => e.CreatedDate).HasColumnName("created_date");
        //    entity.Property(e => e.FirstName)
        //        .HasMaxLength(50)
        //        .HasColumnName("first_name");
        //    entity.Property(e => e.LastName)
        //        .HasMaxLength(50)
        //        .HasColumnName("last_name");
        //    entity.Property(e => e.UserType).HasColumnName("user_type");
        //});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
