using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCORE.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2a31296e-c29d-470f-a529-322e6218013c"), "HR" },
                    { new Guid("935852bc-3b30-4003-85fe-cfd5d4198b23"), "Accountant" },
                    { new Guid("9438216b-b2e7-4cbc-b684-a9ae5702259b"), "Software Development" },
                    { new Guid("d3f19cce-7846-43b9-8ba7-0bb3e1a3a818"), "Finance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("2a31296e-c29d-470f-a529-322e6218013c"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("935852bc-3b30-4003-85fe-cfd5d4198b23"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("9438216b-b2e7-4cbc-b684-a9ae5702259b"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("d3f19cce-7846-43b9-8ba7-0bb3e1a3a818"));
        }
    }
}
