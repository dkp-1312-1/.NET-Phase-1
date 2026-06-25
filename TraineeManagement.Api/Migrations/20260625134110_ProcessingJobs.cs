using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ProcessingJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CorrelationId = table.Column<string>(type: "longtext", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    ErrorSummary = table.Column<string>(type: "longtext", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessingJobs_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "Role", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 25, 13, 41, 9, 736, DateTimeKind.Utc).AddTicks(2353), "$2a$12$DvHXE67ULQf6dSq1CHeQQOwdgrsEcTcQB9hHAel6AV40hEjq39u22", 1, new DateTime(2026, 6, 25, 13, 41, 9, 736, DateTimeKind.Utc).AddTicks(2610) });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingJobs_SubmissionId",
                table: "ProcessingJobs",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessingJobs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash", "Role", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 23, 6, 47, 11, 780, DateTimeKind.Utc).AddTicks(714), "$2a$12$W/8IO1ks48Ku7NVvOvEqq.2F6kVpkSL5ySP/5WjrVjIGX0yV..HSW", 0, new DateTime(2026, 6, 23, 6, 47, 11, 780, DateTimeKind.Utc).AddTicks(972) });
        }
    }
}
