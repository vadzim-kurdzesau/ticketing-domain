CREATE PROCEDURE [dbo].[sp_events_insert]
    @Name NVARCHAR(255),
    @Date DATETIME,
    @venue_id INT,
    @inserted_id INT OUTPUT
AS
BEGIN
    INSERT INTO events (name, date, venue_id)
    VALUES (@name, @date, @venue_id);

    SET @inserted_id = SCOPE_IDENTITY();
END
