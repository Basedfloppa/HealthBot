using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBot.Migrations
{
    public partial class AdminTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    user = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Admin_pkey", x => x.uuid);
                    table.ForeignKey(
                        name: "User",
                        column: x => x.user,
                        principalTable: "user",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");
        }
    }
}
