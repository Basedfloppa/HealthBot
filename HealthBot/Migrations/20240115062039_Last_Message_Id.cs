using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBot.Migrations
{
    public partial class Last_Message_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "message_id",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "message_id", table: "users");
        }
    }
}
