--SQL
/*
SELECT 
ShipToCode
FROM
[ORDR] x

WHERE 
x.DocEntry = '12'
*/

--HANA
SELECT "ShipToCode" as "Result", * FROM "ORDR" x
WHERE 
x."DocEntry" = 'param1'

