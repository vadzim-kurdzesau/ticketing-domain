CREATE TABLE [dbo].[venues]
(
	[id]        INT             NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [name]      NVARCHAR(50)    NOT NULL, 
    [country]   NVARCHAR(50)    NOT NULL, 
    [region]    NVARCHAR(50)    NULL, 
    [city]      NVARCHAR(50)    NOT NULL, 
    [street]    NVARCHAR(100)   NOT NULL
)
