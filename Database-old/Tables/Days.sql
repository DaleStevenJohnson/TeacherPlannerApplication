CREATE TABLE [dbo].[Days]
(
	[id] INT NOT NULL PRIMARY KEY,
	[academic_year_id] int NOT NULL,
	CONSTRAINT [FK_Days_ToAcademicYears] FOREIGN KEY ([academic_year_id]) REFERENCES [AcademicYears]([id]), 
	[date] datetime NOT NULL
)
