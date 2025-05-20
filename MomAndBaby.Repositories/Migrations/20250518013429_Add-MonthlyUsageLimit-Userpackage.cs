using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomAndBaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyUsageLimitUserpackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthlyUsageLimit",
                table: "ServicePackages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyUsageLimit",
                table: "ServicePackages");
        }
    }
}
