using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyBackendApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "varchar(50)", nullable: false),
                    FirstUsed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUsed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Credentials_Username = table.Column<string>(type: "text", nullable: false),
                    Credentials_Password = table.Column<string>(type: "text", nullable: false),
                    Profile_FirstName = table.Column<string>(type: "text", nullable: false),
                    Profile_LastName = table.Column<string>(type: "text", nullable: false),
                    Profile_Email = table.Column<string>(type: "text", nullable: false),
                    Profile_Phone = table.Column<string>(type: "text", nullable: false),
                    Joined = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "followers_following",
                columns: table => new
                {
                    FollowerId = table.Column<long>(type: "bigint", nullable: false),
                    FollowingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followers_following", x => new { x.FollowerId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_followers_following_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_followers_following_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowersFollowing",
                columns: table => new
                {
                    FollowerId = table.Column<long>(type: "bigint", nullable: false),
                    FollowingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowersFollowing", x => new { x.FollowerId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_FollowersFollowing_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowersFollowing_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tweets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    InReplyToId = table.Column<long>(type: "bigint", nullable: true),
                    RepostOfId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tweets_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tweets_tweets_InReplyToId",
                        column: x => x.InReplyToId,
                        principalTable: "tweets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tweets_tweets_RepostOfId",
                        column: x => x.RepostOfId,
                        principalTable: "tweets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HashtagTweet",
                columns: table => new
                {
                    HashtagsId = table.Column<long>(type: "bigint", nullable: false),
                    TweetsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashtagTweet", x => new { x.HashtagsId, x.TweetsId });
                    table.ForeignKey(
                        name: "FK_HashtagTweet_Hashtags_HashtagsId",
                        column: x => x.HashtagsId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HashtagTweet_tweets_TweetsId",
                        column: x => x.TweetsId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TweetHashtags",
                columns: table => new
                {
                    TweetId = table.Column<long>(type: "bigint", nullable: false),
                    HashtagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetHashtags", x => new { x.TweetId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_TweetHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TweetHashtags_tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_likes",
                columns: table => new
                {
                    LikedByUsersId = table.Column<long>(type: "bigint", nullable: false),
                    LikedTweetsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_likes", x => new { x.LikedByUsersId, x.LikedTweetsId });
                    table.ForeignKey(
                        name: "FK_user_likes_Users_LikedByUsersId",
                        column: x => x.LikedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_likes_tweets_LikedTweetsId",
                        column: x => x.LikedTweetsId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_mentions",
                columns: table => new
                {
                    MentionedTweetsId = table.Column<long>(type: "bigint", nullable: false),
                    MentionedUsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_mentions", x => new { x.MentionedTweetsId, x.MentionedUsersId });
                    table.ForeignKey(
                        name: "FK_user_mentions_Users_MentionedUsersId",
                        column: x => x.MentionedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_mentions_tweets_MentionedTweetsId",
                        column: x => x.MentionedTweetsId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TweetId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => new { x.UserId, x.TweetId });
                    table.ForeignKey(
                        name: "FK_UserLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikes_tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMentions",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TweetId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMentions", x => new { x.UserId, x.TweetId });
                    table.ForeignKey(
                        name: "FK_UserMentions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMentions_tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_followers_following_FollowingId",
                table: "followers_following",
                column: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowersFollowing_FollowingId",
                table: "FollowersFollowing",
                column: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_HashtagTweet_TweetsId",
                table: "HashtagTweet",
                column: "TweetsId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetHashtags_HashtagId",
                table: "TweetHashtags",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_tweets_AuthorId",
                table: "tweets",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_tweets_InReplyToId",
                table: "tweets",
                column: "InReplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_tweets_RepostOfId",
                table: "tweets",
                column: "RepostOfId");

            migrationBuilder.CreateIndex(
                name: "IX_user_likes_LikedTweetsId",
                table: "user_likes",
                column: "LikedTweetsId");

            migrationBuilder.CreateIndex(
                name: "IX_user_mentions_MentionedUsersId",
                table: "user_mentions",
                column: "MentionedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_TweetId",
                table: "UserLikes",
                column: "TweetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMentions_TweetId",
                table: "UserMentions",
                column: "TweetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followers_following");

            migrationBuilder.DropTable(
                name: "FollowersFollowing");

            migrationBuilder.DropTable(
                name: "HashtagTweet");

            migrationBuilder.DropTable(
                name: "TweetHashtags");

            migrationBuilder.DropTable(
                name: "user_likes");

            migrationBuilder.DropTable(
                name: "user_mentions");

            migrationBuilder.DropTable(
                name: "UserLikes");

            migrationBuilder.DropTable(
                name: "UserMentions");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "tweets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
