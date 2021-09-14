CREATE TABLE [dbo].[KeyDates]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [academic_year_id] INT NOT NULL,
	[description] NVARCHAR(100) NOT NULL, 
    [type] NVARCHAR(50) NOT NULL, 
    [datetime] DATETIME NOT NULL, 
    CONSTRAINT [FK_KeyDates_ToAcademicYears] FOREIGN KEY ([academic_year_id]) REFERENCES [AcademicYears]([id]), 
)
