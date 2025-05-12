using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaSolution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPathAndSeedChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistMediaEntities_MediaEntities_MediaId",
                table: "PlaylistMediaEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistMediaEntities_MediaEntities_MediaId",
                table: "PlaylistMediaEntities",
                column: "MediaId",
                principalTable: "MediaEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistMediaEntities_MediaEntities_MediaId",
                table: "PlaylistMediaEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistMediaEntities_MediaEntities_MediaId",
                table: "PlaylistMediaEntities",
                column: "MediaId",
                principalTable: "MediaEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
