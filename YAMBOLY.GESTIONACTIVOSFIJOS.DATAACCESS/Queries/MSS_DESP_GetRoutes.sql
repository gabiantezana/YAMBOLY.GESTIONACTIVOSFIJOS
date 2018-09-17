--SQL
/*
DECLARE @Names VARCHAR(8000) 
SELECT @Names = COALESCE(@Names + ' | ', '') + U_MSS_IDDI
FROM [@MSS_DESP_LINES] WHERE DocEntry = 'param1'
SELECT @Names
*/

--HANA
SELECT STRING_AGG(U_MSS_IDDI,',') AS Direcciones
FROM "@MSS_DESP_LINES" 
WHERE "DocEntry" = 'param1'