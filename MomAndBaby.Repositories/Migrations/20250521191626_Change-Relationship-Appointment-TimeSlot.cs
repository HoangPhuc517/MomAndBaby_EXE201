using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomAndBaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipAppointmentTimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_Appointments_AppointmentId",
                table: "TimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlot_AppointmentId",
                table: "TimeSlot");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "TimeSlot");

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TimeSlotId",
                table: "Appointments",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_TimeSlot_TimeSlotId",
                table: "Appointments",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_TimeSlot_TimeSlotId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TimeSlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Appointments");

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "TimeSlot",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_AppointmentId",
                table: "TimeSlot",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_Appointments_AppointmentId",
                table: "TimeSlot",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
