using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HallRentingService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HallServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hall_HallServices",
                columns: table => new
                {
                    LeftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RightId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall_HallServices", x => new { x.LeftId, x.RightId });
                    table.ForeignKey(
                        name: "FK_Hall_HallServices_HallServices_RightId",
                        column: x => x.RightId,
                        principalTable: "HallServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hall_HallServices_Halls_LeftId",
                        column: x => x.LeftId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HallId",
                table: "Bookings",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Hall_HallServices_RightId",
                table: "Hall_HallServices",
                column: "RightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Hall_HallServices");

            migrationBuilder.DropTable(
                name: "HallServices");

            migrationBuilder.DropTable(
                name: "Halls");
        }
    }
}
