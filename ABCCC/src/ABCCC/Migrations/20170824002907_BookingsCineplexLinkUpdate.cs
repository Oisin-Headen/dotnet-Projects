using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ABCCC.Migrations
{
    public partial class BookingsCineplexLinkUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CineplexId",
                table: "Bookings",
                column: "CineplexId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_To_Cineplex",
                table: "Bookings",
                column: "CineplexId",
                principalTable: "Cineplex",
                principalColumn: "CineplexID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_To_Cineplex",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CineplexId",
                table: "Bookings");
        }
    }
}
