CREATE PROCEDURE [dbo].[sp_venues_get_by_id]
	@Id INT
AS
BEGIN
	SELECT * FROM venues AS v	
	LEFT JOIN sections AS c
	ON c.venue_id = v.id
	LEFT JOIN rows AS r
	ON r.section_id = c.id
	LEFT JOIN seats AS s
	ON s.row_id = r.id
	WHERE v.id = @Id
END
