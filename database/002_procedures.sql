USE Yukimushi;
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetReadingsByDateRange
    @From   DATETIME2,
    @To     DATETIME2,
    @Device NVARCHAR(40) = NULL,
    @Group  NVARCHAR(40) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.id                           AS Id,
        d.mac                          AS Device,
        k.name                         AS Kind,
        r.recorded_at                  AS TimeStamp,
        CAST(fr.value AS REAL)         AS Value,
        br.value                       AS State,
        CAST(gr.longitude AS FLOAT)    AS Longitude,
        CAST(gr.latitude AS REAL)      AS Latitude
    FROM dbo.reading r
    INNER JOIN dbo.device d                ON d.id = r.device_id
    INNER JOIN dbo.device_group g          ON g.id = d.group_id
    INNER JOIN dbo.kind k                  ON k.id = r.kind_id
    LEFT JOIN dbo.float_reading fr         ON fr.reading_id = r.id
    LEFT JOIN dbo.boolean_reading br       ON br.reading_id = r.id
    LEFT JOIN dbo.geoespatial_reading gr   ON gr.reading_id = r.id
    WHERE r.recorded_at >= @From
      AND r.recorded_at <= @To
      AND (@Device IS NULL OR d.mac = @Device OR d.nombre = @Device)
      AND (@Group  IS NULL OR g.nombre = @Group)
    ORDER BY r.recorded_at;
END
GO
