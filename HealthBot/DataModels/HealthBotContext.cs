using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealthBot;

public partial class HealthBotContext : DbContext
{
    public HealthBotContext() { }

    public HealthBotContext(DbContextOptions<HealthBotContext> options)
        : base(options) { }

    public virtual DbSet<Biometry> Biometries { get; set; }

    public virtual DbSet<Diaryentry> DiaryEntrys { get; set; }

    public virtual DbSet<Exportdatum> ExportData { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Host=db;Database=healthbot;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("Users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Alias).HasColumnName("alias");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.LastAction).HasColumnName("last_action");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.SubscriptionEnd).HasColumnName("subscription_end");
            entity.Property(e => e.SubscriptionStart).HasColumnName("subscription_start");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity
                .HasMany(d => d.Observees)
                .WithMany(p => p.Observers)
                .UsingEntity<Dictionary<string, object>>(
                    "Obresver",
                    r =>
                        r.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("Observee")
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasConstraintName("fk_observee"),
                    l =>
                        l.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("Observer")
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasConstraintName("fk_observer"),
                    j =>
                    {
                        j.HasKey("Observer", "Observee").HasName("obresvers_pkey");
                        j.ToTable("obresvers");
                        j.IndexerProperty<long>("Observer").HasColumnName("observer");
                        j.IndexerProperty<long>("Observee").HasColumnName("observee");
                    }
                );

            entity
                .HasMany(d => d.Observers)
                .WithMany(p => p.Observees)
                .UsingEntity<Dictionary<string, object>>(
                    "Obresver",
                    r =>
                        r.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("Observer")
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasConstraintName("fk_observer"),
                    l =>
                        l.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("Observee")
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasConstraintName("fk_observee"),
                    j =>
                    {
                        j.HasKey("Observer", "Observee").HasName("obresvers_pkey");
                        j.ToTable("obresvers");
                        j.IndexerProperty<long>("Observer").HasColumnName("observer");
                        j.IndexerProperty<long>("Observee").HasColumnName("observee");
                    }
                );
        });

        modelBuilder.Entity<Biometry>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("Biometry_pkey");

            entity.ToTable("biometry");

            entity.Property(e => e.Uuid).ValueGeneratedNever().HasColumnName("uuid");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity
                .HasOne(d => d.AuthorNavigation)
                .WithMany(p => p.Biometries)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("author");
        });

        modelBuilder.Entity<Diaryentry>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("DiaryEntrys_pkey");

            entity.ToTable("diaryentrys");

            entity.Property(e => e.Uuid).ValueGeneratedNever().HasColumnName("uuid");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.CaloryAmount).HasColumnName("calory_amount");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.HeartRate).HasColumnName("heart_rate");
            entity.Property(e => e.BloodSaturation).HasColumnName("blood_saturation");
            entity.Property(e => e.BloodPreassure).HasColumnName("blood_preassure");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity
                .HasOne(d => d.AuthorNavigation)
                .WithMany(p => p.DiaryEntries)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("Author");
        });

        modelBuilder.Entity<Exportdatum>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("ExportData_pkey");

            entity.ToTable("exportdata");

            entity.Property(e => e.Uuid).ValueGeneratedNever().HasColumnName("uuid");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExportedData).HasColumnName("exported_data");

            entity
                .HasOne(d => d.AuthorNavigation)
                .WithMany(p => p.ExportData)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("Author");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("Admin_pley");

            entity.ToTable("admin");

            entity.Property(e => e.Uuid).ValueGeneratedNever().HasColumnName("uuid");
            entity.Property(e => e.User).HasColumnName("user");
            entity.Property(e => e.Level).HasColumnName("level");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
