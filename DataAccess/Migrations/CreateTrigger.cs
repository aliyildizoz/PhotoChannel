using DataAccess.Dal.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(PhotoChannelContext))]
    [Migration("999999999999999999_CreateTrigger")]
    public partial class CreateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                CREATE TRIGGER CategoriesDeleteTrigger
                                    ON [PhotoChannel].[Categories]
                                    INSTEAD OF DELETE
                                AS
                                BEGIN
                                    SET NOCOUNT ON;
                                        DELETE FROM ChannelCategories WHERE  CategoryId = (select Id from deleted)
                                        DELETE FROM Categories WHERE  Id = (select Id from deleted)
                                END
                                Go

                                CREATE TRIGGER ChannelsDeleteTrigger
                                    ON [PhotoChannel].[Channels]
                                    INSTEAD OF DELETE
                                AS
                                BEGIN
                                    SET NOCOUNT ON;
                                        DELETE FROM ChannelCategories WHERE  ChannelId = (select Id from deleted)
                                        DELETE FROM Subscribers WHERE  ChannelId = (select Id from deleted)
                                        DELETE FROM Photos WHERE  ChannelId = (select Id from deleted)
                                        DELETE FROM Channels WHERE  Id = (select Id from deleted)
                                END
                                Go

                                CREATE TRIGGER PhotosDeleteTrigger
                                    ON [PhotoChannel].[Photos]
                                    INSTEAD OF DELETE
                                AS
                                BEGIN
                                    SET NOCOUNT ON;
                                        DELETE FROM Likes WHERE  PhotoId = (select Id from deleted)
                                        DELETE FROM Comments WHERE  PhotoId = (select Id from deleted)
                                        DELETE FROM Photos WHERE  Id = (select Id from deleted)
                                END
                                Go

                                CREATE TRIGGER UsersDeleteTrigger
                                    ON [PhotoChannel].[Users]
                                    INSTEAD OF DELETE
                                AS
                                BEGIN
                                    SET NOCOUNT ON;

                                        DELETE FROM Likes WHERE  UserId = (select Id from deleted)
                                        DELETE FROM Comments WHERE  UserId = (select Id from deleted)
                                        DELETE FROM Subscribers WHERE  UserId = (select Id from deleted)
                                        DELETE FROM Photos WHERE  UserId = (select Id from deleted)
                                        DELETE FROM Channels WHERE  UserId = (select Id from deleted)
                                        DELETE FROM UserOperationClaims WHERE  UserId = (select Id from deleted)
                                        DELETE FROM Users WHERE  Id = (select Id from deleted)
                                END
                                Go

                                CREATE TRIGGER OperationClaimsDeleteTrigger
                                    ON [PhotoChannel].[OperationClaims]
                                    INSTEAD OF DELETE
                                AS
                                BEGIN
                                    SET NOCOUNT ON;
                                        DELETE FROM UserOperationClaims WHERE  OperationClaimId = (select Id from deleted)
                                        DELETE FROM OperationClaims WHERE  Id = (select Id from deleted)
                                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                DROP TRIGGER [CategoriesDeleteTrigger];
                                GO
                                DROP TRIGGER [ChannelsDeleteTrigger];
                                GO
                                DROP TRIGGER [PhotosDeleteTrigger];
                                GO
                                DROP TRIGGER [UsersDeleteTrigger];
                                GO
                                DROP TRIGGER [OperationClaimsDeleteTrigger];
            ");
        }
    }
}
