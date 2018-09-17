--SQL
/*
UPDATE [@MSS_DESP] 
SET U_MSS_APRO = 'Y' 
WHERE DocEntry = 'param1'
*/

--HANA
UPDATE "@MSS_DESP" 
SET "U_MSS_ESTA" = 'param2' 
WHERE "DocEntry" = 'param1'



