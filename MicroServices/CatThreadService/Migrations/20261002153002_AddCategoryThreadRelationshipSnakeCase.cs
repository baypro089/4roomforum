using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatThreadService.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryThreadRelationshipSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "Threads",
                newName: "view_count");

            migrationBuilder.RenameColumn(
                name: "ThreadTitle",
                table: "Threads",
                newName: "thread_title");

            migrationBuilder.RenameColumn(
                name: "ThreadContent",
                table: "Threads",
                newName: "thread_content");

            migrationBuilder.RenameColumn(
                name: "IsPinned",
                table: "Threads",
                newName: "is_pinned");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Threads",
                newName: "is_closed");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Threads",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Threads",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Threads",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "Threads",
                newName: "thread_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Categories",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Categories",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Categories",
                newName: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_category_id",
                table: "Threads",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Categories_category_id",
                table: "Threads",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Categories_category_id",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_category_id",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "Threads",
                newName: "ViewCount");

            migrationBuilder.RenameColumn(
                name: "thread_title",
                table: "Threads",
                newName: "ThreadTitle");

            migrationBuilder.RenameColumn(
                name: "thread_content",
                table: "Threads",
                newName: "ThreadContent");

            migrationBuilder.RenameColumn(
                name: "is_pinned",
                table: "Threads",
                newName: "IsPinned");

            migrationBuilder.RenameColumn(
                name: "is_closed",
                table: "Threads",
                newName: "IsClosed");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Threads",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Threads",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Threads",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "thread_id",
                table: "Threads",
                newName: "ThreadId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Categories",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Categories",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Categories",
                newName: "CategoryId");
        }
    }
}
