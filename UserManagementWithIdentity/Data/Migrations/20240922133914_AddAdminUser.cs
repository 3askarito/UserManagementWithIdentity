using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementWithIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfileImage]) VALUES (N'4f02da86-2dfd-448c-8365-ae3ccdac6c97', N'admin', N'ADMIN', N'admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAIAAYagAAAAEMz/4mm5hwvh+Gxn35LymmMISc42tc4x8F1N246AdiUo45InDsqeemPmQWWia6JZDQ==', N'BFWIF5FQUFPKPPCEVKBRUZWC2D4MJQVY', N'25c590e1-ab8e-4c9a-a7c6-da5fe15025d2', NULL, 0, 0, NULL, 1, 0, N'Abdulrahman', N'Elaskary', Null)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '4f02da86-2dfd-448c-8365-ae3ccdac6c97'");
        }
    }
}
