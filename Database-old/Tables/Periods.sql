CREATE TABLE [dbo].[Periods]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [day_id] INT NOT NULL, 
    CONSTRAINT [FK_Periods_ToDays] FOREIGN KEY ([day_id]) REFERENCES [Days]([id]), 
    [timetable_classcode] int NULL,
    CONSTRAINT [FK_Periods_ToTimetable] FOREIGN KEY ([timetable_classcode]) REFERENCES [Timetable]([period_id]),
    [user_entered_classcode] NVARCHAR(15) NULL, 
    [period_number] TINYINT NOT NULL, 
    [margin_text] NVARCHAR(100) NULL,
    [main_text] NVARCHAR(1000) NULL,
    [side_text] NVARCHAR(200) NULL,
)
