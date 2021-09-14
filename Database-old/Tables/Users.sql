CREATE TABLE [dbo].[Users]
(
	[user_id] INT NOT NULL PRIMARY KEY, 
    [username] NVARCHAR(50) NOT NULL, 
    [password] NCHAR(64) NOT NULL
)
