using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedalParadise.Migrations
{
    public partial class _3rd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Table_Clients_ClientID",
                table: "Order_Table");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Clients_ClientID",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Clients_ClientID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Clients_ClientID",
                table: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "ShoppingCarts",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_ClientID",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_UserID");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Reviews",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ClientID",
                table: "Reviews",
                newName: "IX_Reviews_UserID");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "RepairRequests",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequests_ClientID",
                table: "RepairRequests",
                newName: "IX_RepairRequests_UserID");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Order_Table",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Table_ClientID",
                table: "Order_Table",
                newName: "IX_Order_Table_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ClientUserID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Membership",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientUserID",
                table: "Users",
                column: "ClientUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Table_Users_UserID",
                table: "Order_Table",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Users_UserID",
                table: "RepairRequests",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Users_UserID",
                table: "ShoppingCarts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ClientUserID",
                table: "Users",
                column: "ClientUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Table_Users_UserID",
                table: "Order_Table");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Users_UserID",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Users_UserID",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ClientUserID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClientUserID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClientUserID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Membership",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ShoppingCarts",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_UserID",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_ClientID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Reviews",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                newName: "IX_Reviews_ClientID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "RepairRequests",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequests_UserID",
                table: "RepairRequests",
                newName: "IX_RepairRequests_ClientID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Order_Table",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Table_UserID",
                table: "Order_Table",
                newName: "IX_Order_Table_ClientID");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Membership = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                    table.ForeignKey(
                        name: "FK_Clients_Users_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Table_Clients_ClientID",
                table: "Order_Table",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Clients_ClientID",
                table: "RepairRequests",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Clients_ClientID",
                table: "Reviews",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Clients_ClientID",
                table: "ShoppingCarts",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
