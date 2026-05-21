using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UbysV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_Dersler_DersId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_Ogrenciler_OgrenciId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Kullanicilar_KullaniciId",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_Bolum",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_DersId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_OgrenciId_DersId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_Dersler_DersAdi",
                table: "Dersler");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "Bolum",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Sinif",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Donem",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "OgretmenAdi",
                table: "Dersler");

            migrationBuilder.RenameColumn(
                name: "Not",
                table: "OgrenciDersler",
                newName: "VizeNotu");

            migrationBuilder.RenameColumn(
                name: "DersId",
                table: "OgrenciDersler",
                newName: "DevamsizlikSayisi");

            migrationBuilder.RenameColumn(
                name: "SaatlikDersSayisi",
                table: "Dersler",
                newName: "UygulamaSaat");

            migrationBuilder.AddColumn<int>(
                name: "AktifDonemId",
                table: "Ogrenciler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BolumId",
                table: "Ogrenciler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "Ogrenciler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Gano",
                table: "Ogrenciler",
                type: "decimal(4,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SinifSeviyesi",
                table: "Ogrenciler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TamamlananAkts",
                table: "Ogrenciler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ButunlemeNotu",
                table: "OgrenciDersler",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DersAtamaId",
                table: "OgrenciDersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DevamSayisi",
                table: "OgrenciDersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DonemId",
                table: "OgrenciDersler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalNotu",
                table: "OgrenciDersler",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GenelNot",
                table: "OgrenciDersler",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HarfNotu",
                table: "OgrenciDersler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotKatsayisi",
                table: "OgrenciDersler",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "OgrenciDersler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnaylayanKullaniciId",
                table: "OgrenciDersler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedNedeni",
                table: "OgrenciDersler",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BolumId",
                table: "Kullanicilar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unvan",
                table: "Kullanicilar",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BolumId",
                table: "Dersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxKontenjan",
                table: "Dersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeoriSaat",
                table: "Dersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BelgeTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    BelgeTur = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SonucNotu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IslemYapanId = table.Column<int>(type: "int", nullable: true),
                    TeslimTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BelgeTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BelgeTalepleri_Kullanicilar_IslemYapanId",
                        column: x => x.IslemYapanId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BelgeTalepleri_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BolumKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BolumAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fakulte = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinMezuniyetAkts = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donemler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonemKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DonemAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Yil = table.Column<int>(type: "int", nullable: false),
                    DonemTur = table.Column<int>(type: "int", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DersKayitBaslangic = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DersKayitBitis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donemler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DersAtamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    DonemId = table.Column<int>(type: "int", nullable: false),
                    OgretmenId = table.Column<int>(type: "int", nullable: false),
                    Gun = table.Column<int>(type: "int", nullable: true),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: true),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: true),
                    Derslik = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KayitliOgrenciSayisi = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersAtamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DersAtamalar_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DersAtamalar_Donemler_DonemId",
                        column: x => x.DonemId,
                        principalTable: "Donemler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DersAtamalar_Kullanicilar_OgretmenId",
                        column: x => x.OgretmenId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Duyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hedef = table.Column<int>(type: "int", nullable: false),
                    HedefBolumId = table.Column<int>(type: "int", nullable: true),
                    HedefDersAtamaId = table.Column<int>(type: "int", nullable: true),
                    YayinlayanId = table.Column<int>(type: "int", nullable: false),
                    YayinTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Onemli = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duyurular_Bolumler_HedefBolumId",
                        column: x => x.HedefBolumId,
                        principalTable: "Bolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Duyurular_DersAtamalar_HedefDersAtamaId",
                        column: x => x.HedefDersAtamaId,
                        principalTable: "DersAtamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Duyurular_Kullanicilar_YayinlayanId",
                        column: x => x.YayinlayanId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sinavlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersAtamaId = table.Column<int>(type: "int", nullable: false),
                    SinavTur = table.Column<int>(type: "int", nullable: false),
                    SinavTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    Derslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinavlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sinavlar_DersAtamalar_DersAtamaId",
                        column: x => x.DersAtamaId,
                        principalTable: "DersAtamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Yoklamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersAtamaId = table.Column<int>(type: "int", nullable: false),
                    YoklamaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OgretmenId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yoklamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yoklamalar_DersAtamalar_DersAtamaId",
                        column: x => x.DersAtamaId,
                        principalTable: "DersAtamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yoklamalar_Kullanicilar_OgretmenId",
                        column: x => x.OgretmenId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OgrenciYoklamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YoklamaId = table.Column<int>(type: "int", nullable: false),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    Geldi = table.Column<bool>(type: "bit", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciYoklamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OgrenciYoklamalar_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OgrenciYoklamalar_Yoklamalar_YoklamaId",
                        column: x => x.YoklamaId,
                        principalTable: "Yoklamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_AktifDonemId",
                table: "Ogrenciler",
                column: "AktifDonemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_BolumId",
                table: "Ogrenciler",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_DersAtamaId",
                table: "OgrenciDersler",
                column: "DersAtamaId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_DonemId",
                table: "OgrenciDersler",
                column: "DonemId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_OgrenciId_DersAtamaId",
                table: "OgrenciDersler",
                columns: new[] { "OgrenciId", "DersAtamaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_OnaylayanKullaniciId",
                table: "OgrenciDersler",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_BolumId",
                table: "Kullanicilar",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Dersler_BolumId",
                table: "Dersler",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_BelgeTalepleri_Durum",
                table: "BelgeTalepleri",
                column: "Durum");

            migrationBuilder.CreateIndex(
                name: "IX_BelgeTalepleri_IsActive",
                table: "BelgeTalepleri",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BelgeTalepleri_IslemYapanId",
                table: "BelgeTalepleri",
                column: "IslemYapanId");

            migrationBuilder.CreateIndex(
                name: "IX_BelgeTalepleri_OgrenciId",
                table: "BelgeTalepleri",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Bolumler_BolumKodu",
                table: "Bolumler",
                column: "BolumKodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bolumler_IsActive",
                table: "Bolumler",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DersAtamalar_DersId_DonemId",
                table: "DersAtamalar",
                columns: new[] { "DersId", "DonemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DersAtamalar_DonemId",
                table: "DersAtamalar",
                column: "DonemId");

            migrationBuilder.CreateIndex(
                name: "IX_DersAtamalar_IsActive",
                table: "DersAtamalar",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DersAtamalar_OgretmenId",
                table: "DersAtamalar",
                column: "OgretmenId");

            migrationBuilder.CreateIndex(
                name: "IX_Donemler_AktifMi",
                table: "Donemler",
                column: "AktifMi");

            migrationBuilder.CreateIndex(
                name: "IX_Donemler_DonemKodu",
                table: "Donemler",
                column: "DonemKodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donemler_IsActive",
                table: "Donemler",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_Hedef",
                table: "Duyurular",
                column: "Hedef");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_HedefBolumId",
                table: "Duyurular",
                column: "HedefBolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_HedefDersAtamaId",
                table: "Duyurular",
                column: "HedefDersAtamaId");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_IsActive",
                table: "Duyurular",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_YayinlayanId",
                table: "Duyurular",
                column: "YayinlayanId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciYoklamalar_OgrenciId",
                table: "OgrenciYoklamalar",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciYoklamalar_YoklamaId_OgrenciId",
                table: "OgrenciYoklamalar",
                columns: new[] { "YoklamaId", "OgrenciId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sinavlar_DersAtamaId_SinavTur",
                table: "Sinavlar",
                columns: new[] { "DersAtamaId", "SinavTur" });

            migrationBuilder.CreateIndex(
                name: "IX_Sinavlar_IsActive",
                table: "Sinavlar",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Sinavlar_SinavTarihi",
                table: "Sinavlar",
                column: "SinavTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Yoklamalar_DersAtamaId_YoklamaTarihi",
                table: "Yoklamalar",
                columns: new[] { "DersAtamaId", "YoklamaTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_Yoklamalar_IsActive",
                table: "Yoklamalar",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Yoklamalar_OgretmenId",
                table: "Yoklamalar",
                column: "OgretmenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dersler_Bolumler_BolumId",
                table: "Dersler",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Bolumler_BolumId",
                table: "Kullanicilar",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_DersAtamalar_DersAtamaId",
                table: "OgrenciDersler",
                column: "DersAtamaId",
                principalTable: "DersAtamalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_Donemler_DonemId",
                table: "OgrenciDersler",
                column: "DonemId",
                principalTable: "Donemler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_Kullanicilar_OnaylayanKullaniciId",
                table: "OgrenciDersler",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_Ogrenciler_OgrenciId",
                table: "OgrenciDersler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Bolumler_BolumId",
                table: "Ogrenciler",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Donemler_AktifDonemId",
                table: "Ogrenciler",
                column: "AktifDonemId",
                principalTable: "Donemler",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Kullanicilar_KullaniciId",
                table: "Ogrenciler",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dersler_Bolumler_BolumId",
                table: "Dersler");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Bolumler_BolumId",
                table: "Kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_DersAtamalar_DersAtamaId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_Donemler_DonemId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_Kullanicilar_OnaylayanKullaniciId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_OgrenciDersler_Ogrenciler_OgrenciId",
                table: "OgrenciDersler");

            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Bolumler_BolumId",
                table: "Ogrenciler");

            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Donemler_AktifDonemId",
                table: "Ogrenciler");

            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Kullanicilar_KullaniciId",
                table: "Ogrenciler");

            migrationBuilder.DropTable(
                name: "BelgeTalepleri");

            migrationBuilder.DropTable(
                name: "Duyurular");

            migrationBuilder.DropTable(
                name: "OgrenciYoklamalar");

            migrationBuilder.DropTable(
                name: "Sinavlar");

            migrationBuilder.DropTable(
                name: "Bolumler");

            migrationBuilder.DropTable(
                name: "Yoklamalar");

            migrationBuilder.DropTable(
                name: "DersAtamalar");

            migrationBuilder.DropTable(
                name: "Donemler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_AktifDonemId",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_BolumId",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_DersAtamaId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_DonemId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_OgrenciId_DersAtamaId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_OgrenciDersler_OnaylayanKullaniciId",
                table: "OgrenciDersler");

            migrationBuilder.DropIndex(
                name: "IX_Kullanicilar_BolumId",
                table: "Kullanicilar");

            migrationBuilder.DropIndex(
                name: "IX_Dersler_BolumId",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "AktifDonemId",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "BolumId",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Gano",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "SinifSeviyesi",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "TamamlananAkts",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "ButunlemeNotu",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "DersAtamaId",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "DevamSayisi",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "DonemId",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "FinalNotu",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "GenelNot",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "HarfNotu",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "NotKatsayisi",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "OnaylayanKullaniciId",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "RedNedeni",
                table: "OgrenciDersler");

            migrationBuilder.DropColumn(
                name: "BolumId",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Unvan",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "BolumId",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "MaxKontenjan",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "TeoriSaat",
                table: "Dersler");

            migrationBuilder.RenameColumn(
                name: "VizeNotu",
                table: "OgrenciDersler",
                newName: "Not");

            migrationBuilder.RenameColumn(
                name: "DevamsizlikSayisi",
                table: "OgrenciDersler",
                newName: "DersId");

            migrationBuilder.RenameColumn(
                name: "UygulamaSaat",
                table: "Dersler",
                newName: "SaatlikDersSayisi");

            migrationBuilder.AddColumn<string>(
                name: "Bolum",
                table: "Ogrenciler",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sinif",
                table: "Ogrenciler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Donem",
                table: "Dersler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgretmenAdi",
                table: "Dersler",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_Bolum",
                table: "Ogrenciler",
                column: "Bolum");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_DersId",
                table: "OgrenciDersler",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrenciDersler_OgrenciId_DersId",
                table: "OgrenciDersler",
                columns: new[] { "OgrenciId", "DersId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dersler_DersAdi",
                table: "Dersler",
                column: "DersAdi");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs",
                column: "EntityName");

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_Dersler_DersId",
                table: "OgrenciDersler",
                column: "DersId",
                principalTable: "Dersler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OgrenciDersler_Ogrenciler_OgrenciId",
                table: "OgrenciDersler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Kullanicilar_KullaniciId",
                table: "Ogrenciler",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
