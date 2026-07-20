using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class All : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 30, 12, 23, 0, 854, DateTimeKind.Utc).AddTicks(2213), "$2a$12$dHPfUDYaz1pqxYu94qRfVu3mefvCHhjJXoCr7bKt4vTmSfOuvmRbW", new DateTime(2026, 6, 30, 12, 23, 0, 854, DateTimeKind.Utc).AddTicks(2464) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 26, 13, 1, 45, 127, DateTimeKind.Utc).AddTicks(6075), "$2a$12$aCFIDNG/6T/nZe6LLq3.bObA8zjaAOXbo0JgR3UcA3tipg3WDP8ie", new DateTime(2026, 6, 26, 13, 1, 45, 127, DateTimeKind.Utc).AddTicks(7256) });
        }
    }
}
