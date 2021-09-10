CREATE TABLE [dbo].[TodoLists]
(
	[id] INT NOT NULL PRIMARY KEY,
	[academic_year_id] INT NOT NULL,
	CONSTRAINT [FK_TodoLists_ToAcademicYears] FOREIGN KEY ([academic_year_id]) REFERENCES [AcademicYears]([id]), 
	[name] NVARCHAR(100) NOT NULL
)
