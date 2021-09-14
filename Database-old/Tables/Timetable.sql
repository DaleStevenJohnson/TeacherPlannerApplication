CREATE TABLE [dbo].[Timetable]
(
	[period_id] INT NOT NULL PRIMARY KEY, 
    [academic_year_id] INT NOT NULL
	CONSTRAINT [FK_Timetable_ToAcademicYears] FOREIGN KEY ([academic_year_id]) REFERENCES [AcademicYears]([id]), 
    [week] TINYINT NOT NULL, 
    [day] TINYINT NOT NULL, 
    [period] TINYINT NOT NULL, 
    [classcode] NVARCHAR(15) NOT NULL, 
    [room] NVARCHAR(10) NOT NULL, 
)
