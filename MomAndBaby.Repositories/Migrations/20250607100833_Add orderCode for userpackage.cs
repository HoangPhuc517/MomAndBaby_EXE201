using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomAndBaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddorderCodeforuserpackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderCode",
                table: "UserPackages",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "UserPackages");
        }
    }
}
