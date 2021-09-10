CREATE TABLE [dbo].[TimetableWeeks]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [academic_year_id] INT NOT NULL
	CONSTRAINT [FK_TimetableWeeks_ToAcademicYears] FOREIGN KEY ([academic_year_id]) REFERENCES [AcademicYears]([id]), 
    [week_beginning] DATETIME NOT NULL, 
    [week] TINYINT NULL
)
