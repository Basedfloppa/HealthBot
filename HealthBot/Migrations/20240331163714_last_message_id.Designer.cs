using System;
using HealthBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HealthBot.Migrations
{
    [DbContext(typeof(HealthBotContext))]
    [Migration("20240331163714_last_message_id")]
    partial class last_message_id
    {
        protected void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HealthBot.Admin", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("User")
                        .HasColumnType("bigint")
                        .HasColumnName("user");

                    b.HasKey("Uuid")
                        .HasName("Admin_pley");

                    b.ToTable("admin", (string)null);
                });

            modelBuilder.Entity("HealthBot.Biometry", b =>
                {
                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<long>("Author")
                        .HasColumnType("bigint")
                        .HasColumnName("author");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<float?>("Height")
                        .HasColumnType("real")
                        .HasColumnName("height");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<float?>("Weight")
                        .HasColumnType("real")
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

                    b.Property<long>("Author")
                        .HasColumnType("bigint")
                        .HasColumnName("author");

                    b.Property<string>("BloodPreassure")
                        .HasColumnType("text")
                        .HasColumnName("blood_preassure");

                    b.Property<float?>("BloodSaturation")
                        .HasColumnType("real")
                        .HasColumnName("blood_saturation");

                    b.Property<float?>("CaloryAmount")
                        .HasColumnType("real")
                        .HasColumnName("calory_amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<float?>("HeartRate")
                        .HasColumnType("real")
                        .HasColumnName("heart_rate");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("State")
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<string>("Tags")
                        .HasColumnType("text")
                        .HasColumnName("tags");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<float?>("Weight")
                        .HasColumnType("real")
                        .HasColumnName("weight");

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

                    b.Property<long>("Author")
                        .HasColumnType("bigint")
                        .HasColumnName("author");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExportedData")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exported_data");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Uuid")
                        .HasName("ExportData_pkey");

                    b.HasIndex("Author");

                    b.ToTable("exportdata", (string)null);
                });

            modelBuilder.Entity("HealthBot.User", b =>
                {
                    b.Property<long>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ChatId"));

                    b.Property<int?>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Alias")
                        .HasColumnType("text")
                        .HasColumnName("alias");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("LastAction")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_action");

                    b.Property<int>("MessageId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Sex")
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<DateTime?>("SubscriptionEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("subscription_end");

                    b.Property<DateTime?>("SubscriptionStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("subscription_start");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("ChatId")
                        .HasName("Users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Obresver", b =>
                {
                    b.Property<long>("Observer")
                        .HasColumnType("bigint")
                        .HasColumnName("observer");

                    b.Property<long>("Observee")
                        .HasColumnType("bigint")
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
                        .WithMany("DiaryEntries")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Author");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("HealthBot.Exportdatum", b =>
                {
                    b.HasOne("HealthBot.User", "AuthorNavigation")
                        .WithMany("ExportData")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Author");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("Obresver", b =>
                {
                    b.HasOne("HealthBot.User", null)
                        .WithMany()
                        .HasForeignKey("Observee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_observee");

                    b.HasOne("HealthBot.User", null)
                        .WithMany()
                        .HasForeignKey("Observer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_observer");
                });

            modelBuilder.Entity("HealthBot.User", b =>
                {
                    b.Navigation("Biometries");

                    b.Navigation("DiaryEntries");

                    b.Navigation("ExportData");
                });
        }
    }
}
