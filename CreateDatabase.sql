--DATABASE--
CREATE DATABASE PhotoChannel
GO
USE [PhotoChannel]
GO
--SCHEMA--
CREATE SCHEMA PhotoChannel
Go
--TABLES--
CREATE TABLE [PhotoChannel].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[ChannelCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChannelId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_ChannelCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Channels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ChannelPhotoUrl] [nvarchar](max) NULL,
	[PublicId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Channels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PhotoId] [int] NOT NULL,
	[ShareDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Likes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PhotoId] [int] NOT NULL,
 CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[OperationClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimName] [nvarchar](max) NULL,
 CONSTRAINT [PK_OperationClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Photos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ChannelId] [int] NOT NULL,
	[ShareDate] [datetime2](7) NOT NULL,
	[PhotoUrl] [nvarchar](max) NULL,
	[PublicId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Subscribers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ChannelId] [int] NOT NULL,
 CONSTRAINT [PK_Subscribers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[UserOperationClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OperationClaimId] [int] NOT NULL,
 CONSTRAINT [PK_UserOperationClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]
GO

CREATE TABLE [PhotoChannel].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[PasswordHash] [varbinary](max) NULL,
	[IsActive] [bit] NOT NULL,
	[RefreshToken] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--REFERENCES--
ALTER TABLE [PhotoChannel].[ChannelCategories]  WITH CHECK ADD  CONSTRAINT [FK_ChannelCategories_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [PhotoChannel].[Categories] ([Id])
GO

ALTER TABLE [PhotoChannel].[ChannelCategories] CHECK CONSTRAINT [FK_ChannelCategories_Categories_CategoryId]
GO

ALTER TABLE [PhotoChannel].[ChannelCategories]  WITH CHECK ADD  CONSTRAINT [FK_ChannelCategories_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [PhotoChannel].[Channels] ([Id])
GO

ALTER TABLE [PhotoChannel].[ChannelCategories] CHECK CONSTRAINT [FK_ChannelCategories_Channels_ChannelId]
GO

ALTER TABLE [PhotoChannel].[Channels]  WITH CHECK ADD  CONSTRAINT [FK_Channels_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[Channels] CHECK CONSTRAINT [FK_Channels_Users_UserId]
GO

ALTER TABLE [PhotoChannel].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Photos_PhotoId] FOREIGN KEY([PhotoId])
REFERENCES [PhotoChannel].[Photos] ([Id])
GO

ALTER TABLE [PhotoChannel].[Comments] CHECK CONSTRAINT [FK_Comments_Photos_PhotoId]
GO

ALTER TABLE [PhotoChannel].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[Comments] CHECK CONSTRAINT [FK_Comments_Users_UserId]
GO

ALTER TABLE [PhotoChannel].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Photos_PhotoId] FOREIGN KEY([PhotoId])
REFERENCES [PhotoChannel].[Photos] ([Id])
GO

ALTER TABLE [PhotoChannel].[Likes] CHECK CONSTRAINT [FK_Likes_Photos_PhotoId]
GO

ALTER TABLE [PhotoChannel].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[Likes] CHECK CONSTRAINT [FK_Likes_Users_UserId]
GO

ALTER TABLE [PhotoChannel].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [PhotoChannel].[Channels] ([Id])
GO

ALTER TABLE [PhotoChannel].[Photos] CHECK CONSTRAINT [FK_Photos_Channels_ChannelId]
GO

ALTER TABLE [PhotoChannel].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[Photos] CHECK CONSTRAINT [FK_Photos_Users_UserId]
GO

ALTER TABLE [PhotoChannel].[Subscribers]  WITH CHECK ADD  CONSTRAINT [FK_Subscribers_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [PhotoChannel].[Channels] ([Id])
GO

ALTER TABLE [PhotoChannel].[Subscribers] CHECK CONSTRAINT [FK_Subscribers_Channels_ChannelId]
GO

ALTER TABLE [PhotoChannel].[Subscribers]  WITH CHECK ADD  CONSTRAINT [FK_Subscribers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[Subscribers] CHECK CONSTRAINT [FK_Subscribers_Users_UserId]
GO

ALTER TABLE [PhotoChannel].[UserOperationClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserOperationClaims_OperationClaims_OperationClaimId] FOREIGN KEY([OperationClaimId])
REFERENCES [PhotoChannel].[OperationClaims] ([Id])
GO

ALTER TABLE [PhotoChannel].[UserOperationClaims] CHECK CONSTRAINT [FK_UserOperationClaims_OperationClaims_OperationClaimId]
GO

ALTER TABLE [PhotoChannel].[UserOperationClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserOperationClaims_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [PhotoChannel].[Users] ([Id])
GO

ALTER TABLE [PhotoChannel].[UserOperationClaims] CHECK CONSTRAINT [FK_UserOperationClaims_Users_UserId]
GO
--TRIGGERS
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
Go
INSERT INTO PhotoChannel.Categories (Name) values('Kitap')
INSERT INTO PhotoChannel.Categories (Name) values('Sinema')
INSERT INTO PhotoChannel.Categories (Name) values('Bilim')
INSERT INTO PhotoChannel.Categories (Name) values('Kültür')
INSERT INTO PhotoChannel.Categories (Name) values('Edebiyat')
INSERT INTO PhotoChannel.OperationClaims (ClaimName) values('Admin')
INSERT INTO PhotoChannel.OperationClaims (ClaimName) values('Users')