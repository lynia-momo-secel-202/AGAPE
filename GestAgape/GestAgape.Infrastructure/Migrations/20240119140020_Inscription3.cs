using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inscription3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Modepaiement",
                schema: "Admission",
                table: "Paiements",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.DropColumn(
                name: "Modepaiement",
                schema: "Admission",
                table: "Paiements");

        }
    }
}
