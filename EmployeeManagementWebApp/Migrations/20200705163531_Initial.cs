using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagementWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1024)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    AppLogTypeId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLogs_AppLogTypes_AppLogTypeId",
                        column: x => x.AppLogTypeId,
                        principalTable: "AppLogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepartmentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1024)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ExtensionPhone = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmploymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePositions",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePositions", x => new { x.EmployeeId, x.PositionId });
                    table.ForeignKey(
                        name: "FK_EmployeePositions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePositions_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobilePhones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(nullable: true),
                    Number = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobilePhones_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLogs_AppLogTypeId",
                table: "AppLogs",
                column: "AppLogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePositions_PositionId",
                table: "EmployeePositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GroupId",
                table: "Employees",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DepartmentId",
                table: "Groups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MobilePhones_EmployeeId",
                table: "MobilePhones",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.DropTable(
                name: "EmployeePositions");

            migrationBuilder.DropTable(
                name: "MobilePhones");

            migrationBuilder.DropTable(
                name: "AppLogTypes");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
