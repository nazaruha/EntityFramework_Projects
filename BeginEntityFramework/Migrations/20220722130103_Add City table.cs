using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeginEntityFramework.Migrations
{
    public partial class AddCitytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "tblCountries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblCountries",
                table: "tblCountries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tblCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCities_tblCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "tblCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCities_CountryId",
                table: "tblCities",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblCountries",
                table: "tblCountries");

            migrationBuilder.RenameTable(
                name: "tblCountries",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");
        }
    }
}
