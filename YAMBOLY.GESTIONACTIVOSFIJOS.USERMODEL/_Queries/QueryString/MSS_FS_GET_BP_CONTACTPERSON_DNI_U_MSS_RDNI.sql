SELECT "Address" 
FROM "OCPR" 
WHERE 
"CardCode" =$["@MSS_CONT".U_MSS_CCOD] 
AND "Name" = $["@MSS_CONT".U_MSS_RCOD] 