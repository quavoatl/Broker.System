using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Broker.System.Migrations.BrokerDb
{
    public partial class Added_FK_To_Limit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BrokerId",
                table: "Limits",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Limits_BrokerId",
                table: "Limits",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Limits_AspNetUsers_BrokerId",
                table: "Limits",
                column: "BrokerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Limits_AspNetUsers_BrokerId",
                table: "Limits");

            migrationBuilder.DropIndex(
                name: "IX_Limits_BrokerId",
                table: "Limits");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrokerId",
                table: "Limits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
