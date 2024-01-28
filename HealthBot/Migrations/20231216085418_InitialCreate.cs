using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    last_action = table.Column<string>(type: "text", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: true),
                    sex = table.Column<string>(type: "text", nullable: true),
                    subscription_end = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    subscription_start = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.chat_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "biometry",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<long>(type: "bigint", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    changed_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("Biometry_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "diaryentrys",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    calory_amount = table.Column<int>(type: "integer", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    heart_rate = table.Column<int>(type: "integer", nullable: true),
                    blood_saturation = table.Column<int>(type: "integer", nullable: true),
                    blood_preassure = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("DiaryEntrys_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "Author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "exportdata",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<long>(type: "bigint", nullable: false),
                    exported_data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("ExportData_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "author",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "obresvers",
                columns: table => new
                {
                    observer = table.Column<long>(type: "bigint", nullable: false),
                    observee = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("obresvers_pkey", x => new { x.observer, x.observee });
                    table.ForeignKey(
                        name: "fk_observee",
                        column: x => x.observee,
                        principalTable: "users",
                        principalColumn: "chat_id"
                    );
                    table.ForeignKey(
                        name: "fk_observer",
                        column: x => x.observer,
                        principalTable: "users",
                        principalColumn: "chat_id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_biometry_author",
                table: "biometry",
                column: "author"
            );

            migrationBuilder.CreateIndex(
                name: "IX_diaryentrys_author",
                table: "diaryentrys",
                column: "author"
            );

            migrationBuilder.CreateIndex(
                name: "IX_exportdata_author",
                table: "exportdata",
                column: "author"
            );

            migrationBuilder.CreateIndex(
                name: "IX_obresvers_observee",
                table: "obresvers",
                column: "observee"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "biometry");

            migrationBuilder.DropTable(name: "exportdata");

            migrationBuilder.DropTable(name: "obresvers");

            migrationBuilder.DropTable(name: "diaryentrys");

            migrationBuilder.DropTable(name: "users");
        }
    }
}
