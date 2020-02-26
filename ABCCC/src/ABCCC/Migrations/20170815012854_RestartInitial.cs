using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ABCCC.Migrations
{
    public partial class RestartInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllMovies",
                columns: table => new
                {
                    MovieID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: false),
                    ShortDescription = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    ReleaseDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllMovies", x => x.MovieID);
                });

            migrationBuilder.CreateTable(
                name: "Cineplex",
                columns: table => new
                {
                    CineplexID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: false),
                    LongDescription = table.Column<string>(nullable: false),
                    ShortDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cineplex", x => x.CineplexID);
                });

            migrationBuilder.CreateTable(
                name: "Enquiry",
                columns: table => new
                {
                    EnquiryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiry", x => x.EnquiryID);
                });

            migrationBuilder.CreateTable(
                name: "CineplexMovie",
                columns: table => new
                {
                    CineplexID = table.Column<int>(nullable: false),
                    MovieID = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    SessionTime = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cineplex__CB419E6DBC409681", x => new { x.CineplexID, x.MovieID, x.Day, x.SessionTime });
                    table.ForeignKey(
                        name: "FK__CineplexM__Cinep__35BCFE0A",
                        column: x => x.CineplexID,
                        principalTable: "Cineplex",
                        principalColumn: "CineplexID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CineplexMovie_AllMovies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "AllMovies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CineplexMovie_MovieID",
                table: "CineplexMovie",
                column: "MovieID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CineplexMovie");

            migrationBuilder.DropTable(
                name: "Enquiry");

            migrationBuilder.DropTable(
                name: "Cineplex");

            migrationBuilder.DropTable(
                name: "AllMovies");
        }
    }
}
