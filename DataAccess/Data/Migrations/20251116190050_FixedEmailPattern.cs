using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedEmailPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidCheck1",
                table: "Trainers",
                sql: "Email like '_%@_%._%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidCheck",
                table: "Members",
                sql: "Email like '_%@_%._%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidCheck1",
                table: "Trainers",
                sql: "Email like '_%@_%._&'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidCheck",
                table: "Members",
                sql: "Email like '_%@_%._&'");
        }
    }
}
