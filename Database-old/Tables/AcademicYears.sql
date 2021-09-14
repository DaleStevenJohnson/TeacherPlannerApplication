CREATE TABLE [dbo].[AcademicYears]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [user_id] INT NOT NULL,
    [year] CHAR(4) NOT NULL, 
    CONSTRAINT [FK_AcademicYears_ToUsers] FOREIGN KEY ([user_id]) REFERENCES [Users]([user_id]), 
)
