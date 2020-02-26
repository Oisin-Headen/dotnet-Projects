using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ABCCC.Migrations
{
    public partial class HourUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie");

            migrationBuilder.DropColumn(
                name: "SessionTime",
                table: "CineplexMovie");

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "CineplexMovie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "CineplexMovie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie",
                columns: new[] { "CineplexID", "MovieID", "Day", "Hour" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "CineplexMovie");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "CineplexMovie");

            migrationBuilder.AddColumn<string>(
                name: "SessionTime",
                table: "CineplexMovie",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cineplex__CB419E6DBC409681",
                table: "CineplexMovie",
                columns: new[] { "CineplexID", "MovieID", "Day", "SessionTime" });
        }
    }
}
