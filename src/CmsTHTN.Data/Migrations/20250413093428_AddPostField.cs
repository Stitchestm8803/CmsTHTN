using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsTHTN.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RoyaltyAmountPerPost",
                table: "AppUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoyaltyAmountPerPost",
                table: "AppUsers");
        }
    }
}
