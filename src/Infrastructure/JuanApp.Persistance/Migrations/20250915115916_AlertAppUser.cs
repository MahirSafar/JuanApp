using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuanApp.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AlertAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribed",
                table: "AspNetUsers");
        }
    }
}
