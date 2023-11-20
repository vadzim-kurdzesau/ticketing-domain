CREATE PROCEDURE [dbo].[sp_venues_insert]
	@Id			AS INT OUTPUT,
	@Name		AS NVARCHAR(50),
	@Country	AS NVARCHAR(50),
	@Region		AS NVARCHAR(50) NULL,
	@City		AS NVARCHAR(50),
	@Street		AS NVARCHAR(100)
AS
BEGIN
	INSERT INTO venues (name, country, region, city, street)
	VALUES (@Name, @Country, @Region, @City, @Street);

	SET @Id = SCOPE_IDENTITY();
END
