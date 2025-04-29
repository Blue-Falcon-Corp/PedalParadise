using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedalParadise.Migrations
{
    public partial class _2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Employees_AssignedEmployeeID",
                table: "RepairRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Manager",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Users",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeID",
                table: "Users",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Manager",
                table: "Users",
                column: "Manager");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Users_AssignedEmployeeID",
                table: "RepairRequests",
                column: "AssignedEmployeeID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_EmployeeID",
                table: "Users",
                column: "EmployeeID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Manager",
                table: "Users",
                column: "Manager",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Users_AssignedEmployeeID",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_EmployeeID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Manager",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Manager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ManagerNavigationEmployeeID = table.Column<int>(type: "int", nullable: true),
                    Manager = table.Column<int>(type: "int", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerNavigationEmployeeID",
                        column: x => x.ManagerNavigationEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Employees_Users_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerNavigationEmployeeID",
                table: "Employees",
                column: "ManagerNavigationEmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Employees_AssignedEmployeeID",
                table: "RepairRequests",
                column: "AssignedEmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
