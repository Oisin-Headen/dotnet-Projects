using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ABCCC.Migrations
{
    public partial class CineplexMovieUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "CineplexMovie");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "CineplexMovie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie",
                columns: new[] { "CineplexID", "Day", "Hour", "Period" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "CineplexMovie");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "CineplexMovie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie",
                columns: new[] { "CineplexID", "MovieID", "Day", "Hour", "Period" });
        }
    }
}
