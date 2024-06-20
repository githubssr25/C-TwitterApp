using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBackendApp.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureUserMentions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_mentions_user_account_MentionedUsersId",
                table: "user_mentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_mentions",
                table: "user_mentions");

            migrationBuilder.DropIndex(
                name: "IX_user_mentions_MentionedUsersId",
                table: "user_mentions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_mentions",
                table: "user_mentions",
                columns: new[] { "MentionedUsersId", "MentionedTweetsId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_mentions_MentionedTweetsId",
                table: "user_mentions",
                column: "MentionedTweetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_mentions_user_account_MentionedUsersId",
                table: "user_mentions",
                column: "MentionedUsersId",
                principalTable: "user_account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_mentions_user_account_MentionedUsersId",
                table: "user_mentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_mentions",
                table: "user_mentions");

            migrationBuilder.DropIndex(
                name: "IX_user_mentions_MentionedTweetsId",
                table: "user_mentions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_mentions",
                table: "user_mentions",
                columns: new[] { "MentionedTweetsId", "MentionedUsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_mentions_MentionedUsersId",
                table: "user_mentions",
                column: "MentionedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_mentions_user_account_MentionedUsersId",
                table: "user_mentions",
                column: "MentionedUsersId",
                principalTable: "user_account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
