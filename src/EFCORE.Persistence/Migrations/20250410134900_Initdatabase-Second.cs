using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCORE.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitdatabaseSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salary_Employee_Id",
                table: "Salary");

            migrationBuilder.AddForeignKey(
                name: "FK_Salary_Employee_EmployeeId",
                table: "Salary",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salary_Employee_EmployeeId",
                table: "Salary");

            migrationBuilder.AddForeignKey(
                name: "FK_Salary_Employee_Id",
                table: "Salary",
                column: "Id",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
