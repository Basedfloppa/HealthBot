using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: true),
                    sex = table.Column<string>(type: "text", nullable: true),
                    subscription_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    subscription_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "time with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "time with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "time with time zone", nullable: true),
                    last_action = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "biometry",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<Guid>(type: "uuid", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Biometry_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "diaryentrys",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    heart_rate = table.Column<int>(type: "integer", nullable: true),
                    blood_saturation = table.Column<int>(type: "integer", nullable: true),
                    blood_preassure = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DiaryEntrys_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "Author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exportdata",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<Guid>(type: "uuid", nullable: false),
                    exported_data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ExportData_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "obresvers",
                columns: table => new
                {
                    observer = table.Column<Guid>(type: "uuid", nullable: false),
                    observee = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("obresvers_pkey", x => new { x.observer, x.observee });
                    table.ForeignKey(
                        name: "fk_observee",
                        column: x => x.observee,
                        principalTable: "users",
                        principalColumn: "uuid");
                    table.ForeignKey(
                        name: "fk_observer",
                        column: x => x.observer,
                        principalTable: "users",
                        principalColumn: "uuid");
                });

            migrationBuilder.CreateTable(
                name: "IntakeItems",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    calory_amount = table.Column<int>(type: "integer", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: false, defaultValueSql: "'solid'::text"),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    diary_entry = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("IntakeItems_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "DiaryEntry",
                        column: x => x.diary_entry,
                        principalTable: "diaryentrys",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_biometry_author",
                table: "biometry",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_diaryentrys_author",
                table: "diaryentrys",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_exportdata_author",
                table: "exportdata",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_IntakeItems_diary_entry",
                table: "IntakeItems",
                column: "diary_entry");

            migrationBuilder.CreateIndex(
                name: "IX_obresvers_observee",
                table: "obresvers",
                column: "observee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "biometry");

            migrationBuilder.DropTable(
                name: "exportdata");

            migrationBuilder.DropTable(
                name: "IntakeItems");

            migrationBuilder.DropTable(
                name: "obresvers");

            migrationBuilder.DropTable(
                name: "diaryentrys");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
