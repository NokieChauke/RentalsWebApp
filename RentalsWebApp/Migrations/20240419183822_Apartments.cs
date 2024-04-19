﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalsWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Apartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Apartments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_AspNetUsers_UserId",
                table: "Apartments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_AspNetUsers_UserId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Apartments");
        }
    }
}
