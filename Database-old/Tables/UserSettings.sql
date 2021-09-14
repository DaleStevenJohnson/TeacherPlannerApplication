CREATE TABLE [dbo].[UserSettings]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [user_id] int NOT NULL,
    CONSTRAINT [FK_UserSettings_ToUsers] FOREIGN KEY ([user_id]) REFERENCES [Users]([user_id]), 
)
