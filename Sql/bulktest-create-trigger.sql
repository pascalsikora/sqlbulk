CREATE TRIGGER [dbo].[upsert] 
   ON  [dbo].[bulktest] 
   INSTEAD OF INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;

    MERGE dbo.bulktest AS tgt
    USING inserted as src
        ON tgt.id = src.id
    WHEN MATCHED
        THEN
            UPDATE
            SET number = src.number
    WHEN NOT MATCHED
        THEN
            INSERT (id, number)
            VALUES (src.id, src.number);

END
GO
