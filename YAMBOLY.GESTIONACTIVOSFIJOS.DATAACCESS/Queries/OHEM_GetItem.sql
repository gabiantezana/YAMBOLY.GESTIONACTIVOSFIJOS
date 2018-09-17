--SQL
/*
SELECT * FROM [@MSS_DESP] x
JOIN [OHEM] y
ON x.U_MSS_REPA = y.empID
WHERE DocEntry = 'param1'
*/
--HANA
SELECT * FROM "OHEM" 
WHERE "empID" = 'param1'

