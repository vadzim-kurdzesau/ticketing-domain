CREATE TABLE [dbo].[events]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [name] NCHAR(150) NOT NULL, 
    [date] DATE NOT NULL, 
    [venue_id] INT NOT NULL
)
