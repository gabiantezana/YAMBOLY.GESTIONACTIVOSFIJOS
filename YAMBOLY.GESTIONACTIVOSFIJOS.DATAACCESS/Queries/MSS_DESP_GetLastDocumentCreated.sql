--SQL
/*
SELECT  top 1 * FROM [@MSS_DESP] x

JOIN [OUSR] y
ON x.UserSign = y.USERID

WHERE y.USER_CODE = 'param1'

ORDER BY x.DocEntry desc
*/


--HANA
SELECT top 1 * FROM "@MSS_DESP" x

JOIN "OUSR" y
ON x."UserSign" = y."USERID"

WHERE y."USER_CODE" = 'param1'

ORDER BY x."DocEntry" desc





