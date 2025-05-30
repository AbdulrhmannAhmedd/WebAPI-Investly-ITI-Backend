using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ContactU__3214EC07B7F9C6CA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Governme__3214EC0745F293D2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GovId = table.Column<int>(type: "int", nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cities__3214EC07E554E366", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Cities__GovId__4CA06362",
                        column: x => x.GovId,
                        principalTable: "Governments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    HashedPassword = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    NationalId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FrontIdPicPath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    BackIdPicPath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ProfilePicPath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    GovernmentId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC07DBCD22E3", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Users__CityId__59FA5E80",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Users__CreatedBy__5AEE82B9",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Users__Governmen__59063A47",
                        column: x => x.GovernmentId,
                        principalTable: "Governments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Users__UpdatedBy__5BE2A6F2",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3214EC074C9193E2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Category__Create__6FE99F9F",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Category__Update__70DDC3D8",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserTypeFrom = table.Column<int>(type: "int", nullable: false),
                    UserTypeTo = table.Column<int>(type: "int", nullable: false),
                    UserIdTo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3214EC0715E0C82B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Feedback__Create__6C190EBB",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Feedback__Update__6D0D32F4",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Feedback__UserId__6B24EA82",
                        column: x => x.UserIdTo,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Founders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Founders__3214EC070FEAA322", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Founders__UserId__5FB337D6",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Investors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InvestingType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Investor__3214EC0780F28580", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Investors__UserI__6383C8BA",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserTypeFrom = table.Column<int>(type: "int", nullable: false),
                    UserTypeTo = table.Column<int>(type: "int", nullable: false),
                    UserIdTo = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__3214EC074D68EC79", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Notificat__Creat__6754599E",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Notificat__Updat__68487DD7",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Notificat__UserI__66603565",
                        column: x => x.UserIdTo,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Form_Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Standard__3214EC07FD4E8146", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Standards__Creat__73BA3083",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Standards__Updat__74AE54BC",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FounderId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AIRate = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    Stage = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Capital = table.Column<decimal>(type: "decimal(10,5)", nullable: true),
                    IsDrafted = table.Column<bool>(type: "bit", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC075E0D0000", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Business__Catego__7F2BE32F",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Business__Create__01142BA1",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Business__Founde__00200768",
                        column: x => x.FounderId,
                        principalTable: "Founders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Business__Update__02084FDA",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryStandard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Starus = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3214EC07756124F4", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CategoryS__Categ__797309D9",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CategoryS__Creat__7A672E12",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CategoryS__Stand__787EE5A0",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CategoryS__Updat__7B5B524B",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessStandardAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC074A4C9C5D", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessS__Busin__10566F31",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BusinessS__Creat__114A936A",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BusinessS__Stand__0F624AF8",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BusinessS__Updat__123EB7A3",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvestorContactRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorId = table.Column<int>(type: "int", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeclineReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Investor__3214EC076FDDB974", x => x.Id);
                    table.ForeignKey(
                        name: "FK__InvestorC__Busin__09A971A2",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__InvestorC__Creat__0B91BA14",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__InvestorC__Inves__0A9D95DB",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__InvestorC__Updat__0C85DE4D",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryStandardsKeyWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryStandardId = table.Column<int>(type: "int", nullable: false),
                    KeyWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3214EC077C678085", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CategoryS__Categ__04E4BC85",
                        column: x => x.CategoryStandardId,
                        principalTable: "CategoryStandard",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CategoryS__Creat__05D8E0BE",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CategoryS__Updat__06CD04F7",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_CategoryId",
                table: "Business",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_CreatedBy",
                table: "Business",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Business_FounderId",
                table: "Business",
                column: "FounderId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_UpdatedBy",
                table: "Business",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStandardAnswers_BusinessId",
                table: "BusinessStandardAnswers",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStandardAnswers_CreatedBy",
                table: "BusinessStandardAnswers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStandardAnswers_StandardId",
                table: "BusinessStandardAnswers",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStandardAnswers_UpdatedBy",
                table: "BusinessStandardAnswers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedBy",
                table: "Category",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UpdatedBy",
                table: "Category",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandard_CategoryId",
                table: "CategoryStandard",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandard_CreatedBy",
                table: "CategoryStandard",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandard_StandardId",
                table: "CategoryStandard",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandard_UpdatedBy",
                table: "CategoryStandard",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandardsKeyWords_CategoryStandardId",
                table: "CategoryStandardsKeyWords",
                column: "CategoryStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandardsKeyWords_CreatedBy",
                table: "CategoryStandardsKeyWords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStandardsKeyWords_UpdatedBy",
                table: "CategoryStandardsKeyWords",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovId",
                table: "Cities",
                column: "GovId");

            migrationBuilder.CreateIndex(
                name: "UQ_Cities_Name_Ar",
                table: "Cities",
                column: "Name_Ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Cities_Name_En",
                table: "Cities",
                column: "Name_En",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_CreatedBy",
                table: "Feedback",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UpdatedBy",
                table: "Feedback",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserIdTo",
                table: "Feedback",
                column: "UserIdTo");

            migrationBuilder.CreateIndex(
                name: "UQ__Founders__1788CC4D257E4087",
                table: "Founders",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Governments_Name_Ar",
                table: "Governments",
                column: "Name_Ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Governments_Name_En",
                table: "Governments",
                column: "Name_En",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestorContactRequests_BusinessId",
                table: "InvestorContactRequests",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorContactRequests_CreatedBy",
                table: "InvestorContactRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorContactRequests_InvestorId",
                table: "InvestorContactRequests",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorContactRequests_UpdatedBy",
                table: "InvestorContactRequests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Investor__1788CC4D151790F5",
                table: "Investors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedBy",
                table: "Notifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UpdatedBy",
                table: "Notifications",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserIdTo",
                table: "Notifications",
                column: "UserIdTo");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_CreatedBy",
                table: "Standards",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_UpdatedBy",
                table: "Standards",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GovernmentId",
                table: "Users",
                column: "GovernmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105346C2B56BA",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__E9AA32FA413F176B",
                table: "Users",
                column: "NationalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessStandardAnswers");

            migrationBuilder.DropTable(
                name: "CategoryStandardsKeyWords");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "InvestorContactRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "CategoryStandard");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "Investors");

            migrationBuilder.DropTable(
                name: "Standards");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Founders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Governments");
        }
    }
}
