CREATE TABLE [dbo].[Notes]
(
	[id] INT NOT NULL PRIMARY KEY,
	[day_id] INT NOT NULL, 
	CONSTRAINT [FK_Notes_ToDays] FOREIGN KEY ([day_id]) REFERENCES [Days]([id]), 
    [text] NVARCHAR(1000) NULL
)
