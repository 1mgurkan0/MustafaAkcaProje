using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDuyuruOkumalarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BelgeDosyaAdi",
                table: "BelgeTalepleri",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BelgeDosyaYolu",
                table: "BelgeTalepleri",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DuyuruOkumalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    DuyuruId = table.Column<int>(type: "int", nullable: false),
                    OkunmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuyuruOkumalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuyuruOkumalar_Duyurular_DuyuruId",
                        column: x => x.DuyuruId,
                        principalTable: "Duyurular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuyuruOkumalar_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuyuruOkumalar_DuyuruId",
                table: "DuyuruOkumalar",
                column: "DuyuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DuyuruOkumalar_OgrenciId",
                table: "DuyuruOkumalar",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_DuyuruOkumalar_OgrenciId_DuyuruId",
                table: "DuyuruOkumalar",
                columns: new[] { "OgrenciId", "DuyuruId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuyuruOkumalar");

            migrationBuilder.DropColumn(
                name: "BelgeDosyaAdi",
                table: "BelgeTalepleri");

            migrationBuilder.DropColumn(
                name: "BelgeDosyaYolu",
                table: "BelgeTalepleri");
        }
    }
}
