using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JuanApp.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataToSlider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sliders",
                columns: new[] { "Id", "Created", "Deleted", "Description", "ImageUrl", "IsActive", "IsDeleted", "LastModified", "Order", "RedirectUrl", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Check out our new summer collection of stylish accessories and bags.", "slider-1.jpg", true, false, null, 1, "/shop", "Creative and Smart" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Explore the latest trends in fashion and find your unique style.", "slider-2.jpg", true, false, null, 2, "/shop", "Amazing Fashion" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Discover a curated collection of modern and exclusive products.", "slider-3.jpg", true, false, null, 3, "/shop", "Unique and Modern" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
