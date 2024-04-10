using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBot.Migrations
{
    /// <inheritdoc />
    public partial class graph_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "graph",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    user = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Graph_pkey", x => x.uuid);
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
