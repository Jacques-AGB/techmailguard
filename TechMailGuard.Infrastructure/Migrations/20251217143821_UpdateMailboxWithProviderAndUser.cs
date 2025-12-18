using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMailGuard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMailboxWithProviderAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Mailboxes_MailboxId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Mailboxes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Mailboxes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Mailboxes_MailboxId",
                table: "Subscriptions",
                column: "MailboxId",
                principalTable: "Mailboxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Mailboxes_MailboxId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Mailboxes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Mailboxes");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Mailboxes_MailboxId",
                table: "Subscriptions",
                column: "MailboxId",
                principalTable: "Mailboxes",
                principalColumn: "Id");
        }
    }
}
