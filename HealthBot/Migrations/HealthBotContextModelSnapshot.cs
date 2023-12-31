﻿// <auto-generated />
using System;
using HealthBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HealthBot.Migrations
{
    [DbContext(typeof(HealthBotContext))]
    partial class HealthBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "State", new[] { "Menu" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HealthBot.Biometry", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<Guid>("Author")
                        .HasColumnType("uuid")
                        .HasColumnName("author");

                    b.Property<DateTime?>("ChangedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("changed_at");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<int?>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer")
                        .HasColumnName("weight");

                    b.HasKey("Uuid")
                        .HasName("Biometry_pkey");

                    b.HasIndex("Author");

                    b.ToTable("biometry", (string)null);
                });

            modelBuilder.Entity("HealthBot.Diaryentry", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<Guid>("Author")
                        .HasColumnType("uuid")
                        .HasColumnName("author");

                    b.Property<string>("BloodPreassure")
                        .HasColumnType("text")
                        .HasColumnName("blood_preassure");

                    b.Property<int?>("BloodSaturation")
                        .HasColumnType("integer")
                        .HasColumnName("blood_saturation");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<int?>("HeartRate")
                        .HasColumnType("integer")
                        .HasColumnName("heart_rate");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Uuid")
                        .HasName("DiaryEntrys_pkey");

                    b.HasIndex("Author");

                    b.ToTable("diaryentrys", (string)null);
                });

            modelBuilder.Entity("HealthBot.Exportdatum", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<Guid>("Author")
                        .HasColumnType("uuid")
                        .HasColumnName("author");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExportedData")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exported_data");

                    b.HasKey("Uuid")
                        .HasName("ExportData_pkey");

                    b.HasIndex("Author");

                    b.ToTable("exportdata", (string)null);
                });

            modelBuilder.Entity("HealthBot.IntakeItem", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<int?>("CaloryAmount")
                        .HasColumnType("integer")
                        .HasColumnName("calory_amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("DiaryEntry")
                        .HasColumnType("uuid")
                        .HasColumnName("diary_entry");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("State")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("state")
                        .HasDefaultValueSql("'solid'::text");

                    b.Property<string>("Tags")
                        .HasColumnType("text")
                        .HasColumnName("tags");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer")
                        .HasColumnName("weight");

                    b.HasKey("Uuid")
                        .HasName("IntakeItems_pkey");

                    b.HasIndex("DiaryEntry");

                    b.ToTable("IntakeItems");
                });

            modelBuilder.Entity("HealthBot.User", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<int?>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Alias")
                        .HasColumnType("text")
                        .HasColumnName("alias");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("time with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("time with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("LastAction")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_action");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Sex")
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<DateTime?>("SubscriptionEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("subscription_end");

                    b.Property<DateTime?>("SubscriptionStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("subscription_start");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("time with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Uuid")
                        .HasName("Users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Obresver", b =>
                {
                    b.Property<Guid>("Observer")
                        .HasColumnType("uuid")
                        .HasColumnName("observer");

                    b.Property<Guid>("Observee")
                        .HasColumnType("uuid")
                        .HasColumnName("observee");

                    b.HasKey("Observer", "Observee")
                        .HasName("obresvers_pkey");

                    b.HasIndex("Observee");

                    b.ToTable("obresvers", (string)null);
                });

            modelBuilder.Entity("HealthBot.Biometry", b =>
                {
                    b.HasOne("HealthBot.User", "AuthorNavigation")
                        .WithMany("Biometries")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("author");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("HealthBot.Diaryentry", b =>
                {
                    b.HasOne("HealthBot.User", "AuthorNavigation")
                        .WithMany("Diaryentries")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Author");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("HealthBot.Exportdatum", b =>
                {
                    b.HasOne("HealthBot.User", "AuthorNavigation")
                        .WithMany("Exportdata")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("author");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("HealthBot.IntakeItem", b =>
                {
                    b.HasOne("HealthBot.Diaryentry", "DiaryEntryNavigation")
                        .WithMany("IntakeItems")
                        .HasForeignKey("DiaryEntry")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("DiaryEntry");

                    b.Navigation("DiaryEntryNavigation");
                });

            modelBuilder.Entity("Obresver", b =>
                {
                    b.HasOne("HealthBot.User", null)
                        .WithMany()
                        .HasForeignKey("Observee")
                        .IsRequired()
                        .HasConstraintName("fk_observee");

                    b.HasOne("HealthBot.User", null)
                        .WithMany()
                        .HasForeignKey("Observer")
                        .IsRequired()
                        .HasConstraintName("fk_observer");
                });

            modelBuilder.Entity("HealthBot.Diaryentry", b =>
                {
                    b.Navigation("IntakeItems");
                });

            modelBuilder.Entity("HealthBot.User", b =>
                {
                    b.Navigation("Biometries");

                    b.Navigation("Diaryentries");

                    b.Navigation("Exportdata");
                });
#pragma warning restore 612, 618
        }
    }
}
