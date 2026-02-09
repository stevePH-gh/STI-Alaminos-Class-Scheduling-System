using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddTicksToSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleWednesday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleWednesday");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleTuesday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleTuesday");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleThursday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleThursday");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleSaturday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleSaturday");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleMonday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleMonday");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ScheduleFriday");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ScheduleFriday");

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleWednesday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleWednesday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleTuesday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleTuesday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleThursday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleThursday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleSaturday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleSaturday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleMonday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleMonday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EndTicks",
                table: "ScheduleFriday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTicks",
                table: "ScheduleFriday",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleWednesday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleWednesday");

            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleTuesday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleTuesday");

            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleThursday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleThursday");

            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleSaturday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleSaturday");

            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleMonday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleMonday");

            migrationBuilder.DropColumn(
                name: "EndTicks",
                table: "ScheduleFriday");

            migrationBuilder.DropColumn(
                name: "StartTicks",
                table: "ScheduleFriday");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleWednesday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleWednesday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleTuesday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleTuesday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleThursday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleThursday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleSaturday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleSaturday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleMonday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleMonday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "ScheduleFriday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "ScheduleFriday",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
