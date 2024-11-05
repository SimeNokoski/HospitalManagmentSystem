using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DataAccess.Migrations
{
    public partial class UpdateSuperAdminId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName" },
                values: new object[] { 1, "nokoski99@gmail.com", "j?b??????tN???", 2, "simeSuperAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName" },
                values: new object[] { 111, "nokoski99@gmail.com", "j?b??????tN???", 2, "simeSuperAdmin" });
        }
    }
}
