SELECT "FirstName" || ' ' || "MiddleName" || ' ' || "LastName"
FROM "OCPR" WHERE 
"CardCode" =$["@MSS_CONT".U_MSS_CCOD] 
AND "Name" = $["@MSS_CONT".U_MSS_DCOD] 