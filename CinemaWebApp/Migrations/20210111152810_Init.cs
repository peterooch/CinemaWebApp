using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaWebApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    MinAge = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    HallID = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    MovieName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenings", x => new { x.HallID, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Screenings_Movies_MovieName",
                        column: x => x.MovieName,
                        principalTable: "Movies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketOrders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerUserID = table.Column<int>(nullable: true),
                    Paid = table.Column<bool>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketOrders_Users_BuyerUserID",
                        column: x => x.BuyerUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Seat = table.Column<int>(nullable: false),
                    ScreeningHallID = table.Column<string>(nullable: true),
                    ScreeningStartTime = table.Column<DateTime>(nullable: true),
                    TicketOrderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketOrders_TicketOrderID",
                        column: x => x.TicketOrderID,
                        principalTable: "TicketOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Screenings_ScreeningHallID_ScreeningStartTime",
                        columns: x => new { x.ScreeningHallID, x.ScreeningStartTime },
                        principalTable: "Screenings",
                        principalColumns: new[] { "HallID", "StartTime" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_MovieName",
                table: "Screenings",
                column: "MovieName");

            migrationBuilder.CreateIndex(
                name: "IX_TicketOrders_BuyerUserID",
                table: "TicketOrders",
                column: "BuyerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketOrderID",
                table: "Tickets",
                column: "TicketOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ScreeningHallID_ScreeningStartTime",
                table: "Tickets",
                columns: new[] { "ScreeningHallID", "ScreeningStartTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketOrders");

            migrationBuilder.DropTable(
                name: "Screenings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
