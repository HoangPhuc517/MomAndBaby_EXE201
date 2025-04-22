using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomAndBaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class DeleteExpertIdInUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExpertId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
