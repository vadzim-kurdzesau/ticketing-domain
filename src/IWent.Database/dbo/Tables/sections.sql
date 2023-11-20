CREATE TABLE [dbo].[sections]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [name] NVARCHAR(50) NOT NULL, 
    [venue_id] INT NOT NULL, 
    [seat_type] INT NOT NULL
)
