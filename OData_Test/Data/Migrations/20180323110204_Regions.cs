using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OData_Test.Data.Migrations
{
    public partial class Regions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mantle_Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryCode = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    HasStates = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Order = table.Column<short>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    RegionType = table.Column<byte>(nullable: false),
                    StateCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantle_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mantle_Regions_Mantle_Regions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Mantle_Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mantle_Regions_ParentId",
                table: "Mantle_Regions",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mantle_Regions");
        }
    }
}
