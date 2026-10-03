using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    thread_id = table.Column<int>(type: "int", nullable: false),
                    posted_by = table.Column<int>(type: "int", nullable: false),
                    post_title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    like = table.Column<int>(type: "int", nullable: false),
                    post_content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    post_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_edited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_created_by_admin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LikeOfPosts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeOfPosts", x => x.id);
                    table.ForeignKey(
                        name: "FK_LikeOfPosts_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    reply_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    replied_by = table.Column<int>(type: "int", nullable: false),
                    reply_content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reply_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    upvote_amount = table.Column<int>(type: "int", nullable: false),
                    downvote_amount = table.Column<int>(type: "int", nullable: false),
                    is_edited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    reply_to_reply = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.reply_id);
                    table.ForeignKey(
                        name: "FK_Replies_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replies_Replies_reply_to_reply",
                        column: x => x.reply_to_reply,
                        principalTable: "Replies",
                        principalColumn: "reply_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LikeOfReplies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vote = table.Column<int>(type: "int", nullable: false),
                    reply_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeOfReplies", x => x.id);
                    table.ForeignKey(
                        name: "FK_LikeOfReplies_Replies_reply_id",
                        column: x => x.reply_id,
                        principalTable: "Replies",
                        principalColumn: "reply_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LikeOfPosts_post_id",
                table: "LikeOfPosts",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_LikeOfReplies_reply_id",
                table: "LikeOfReplies",
                column: "reply_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_post_id",
                table: "Replies",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_reply_to_reply",
                table: "Replies",
                column: "reply_to_reply");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeOfPosts");

            migrationBuilder.DropTable(
                name: "LikeOfReplies");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
