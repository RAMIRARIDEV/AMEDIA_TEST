
/****** Object:  Table [dbo].[Users]    Script Date: 23/10/2022 04:08:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[password] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[email] [varchar](200) NULL,
	[profile] [int] NULL,
	[createDate] [date] NULL,
	[updateDate] [date] NULL,
	[isActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[UsersProfiles](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE PROCEDURE [dbo].[CreateUser] 
	@email VARCHAR(50),
	@password VARCHAR(50),
	@name VARCHAR(50),
	@profile INT,
	@codReturn INT OUTPUT
AS
BEGIN

--const
        DECLARE @SUCCESSFUL INT = 0;
        DECLARE  @ERROR_EXISTING_USER INT = 1;
        DECLARE  @ERROR_NON_EXISTENT_USER INT = 2;
        DECLARE  @ERROR_INCOMPLETE_DATA INT= 3;
        DECLARE  @ERROR_EXECUTION INT= 4;

	SET NOCOUNT ON;
	SET @codReturn = @ERROR_EXECUTION

	IF EXISTS (SELECT TOP 1 id FROM Users WHERE UPPER(@email) = UPPER(Email))
	BEGIN
		SET @codReturn = @ERROR_EXISTING_USER
	END
	ELSE
	BEGIN
		INSERT INTO Users (password, name,email,profile,createDate,isActive) 
		VALUES(@password,@name,@email,@profile,GETDATE(),1) 
		
		SET @codReturn = @SUCCESSFUL
	END
	
	RETURN @codReturn

END
GO


CREATE PROCEDURE [dbo].[UpdateUser] 
	@email VARCHAR(50),
	@password VARCHAR(50),
	@name VARCHAR(50),
	@profile INT,
	@id BIGINT,

	@codReturn INT OUTPUT
AS
BEGIN

--const
        DECLARE @SUCCESSFUL INT = 0;
        DECLARE  @ERROR_EXISTING_USER INT = -1;
        DECLARE  @ERROR_NON_EXISTENT_USER INT = 2;
        DECLARE  @ERROR_INCOMPLETE_DATA INT= 3;
        DECLARE  @ERROR_EXECUTION INT= 4;

	SET NOCOUNT ON;
	SET @codReturn = @ERROR_EXECUTION

	IF EXISTS (SELECT TOP 1 id FROM Users WHERE UPPER(@email) = UPPER(Email))
	BEGIN
		SET @codReturn = @ERROR_EXISTING_USER
	END
	ELSE
	BEGIN
		UPDATE Users SET password = @password, name = @name,email = @email,profile = @profile,UpdateDate = GETDATE() where id = @id
		SET @codReturn = @SUCCESSFUL
	END
	
	RETURN @codReturn

END
GO

CREATE PROCEDURE [dbo].[ChangeState] 
	@id BIGINT,

	@codReturn INT OUTPUT
AS
BEGIN

--const
        DECLARE @SUCCESSFUL INT = 0;
        DECLARE  @ERROR_EXISTING_USER INT = -1;
        DECLARE  @ERROR_NON_EXISTENT_USER INT = 2;
        DECLARE  @ERROR_INCOMPLETE_DATA INT= 3;
        DECLARE  @ERROR_EXECUTION INT= 4;

	SET NOCOUNT ON;
	SET @codReturn = @ERROR_EXECUTION

	IF EXISTS (SELECT id FROM Users WHERE id =@id AND isActive = 1)
	BEGIN
		UPDATE  Users SET isActive = 0 WHERE id =@id
		SET @codReturn = @SUCCESSFUL
	END
	ELSE
	BEGIN		
		UPDATE  Users SET isActive = 1 WHERE id =@id
		SET @codReturn = @SUCCESSFUL
	END
	
	RETURN @codReturn

END
GO