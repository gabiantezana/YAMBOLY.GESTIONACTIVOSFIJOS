SELECT x."U_MSS_COAL", y."WhsName" 
FROM "@MSS_CFPE" x 
JOIN "OWHS" y 
ON x."U_MSS_COAL" = y."WhsCode" 
JOIN "OUSR" z
ON x."U_MSS_COUS" = z."USER_CODE"
WHERE z."USERID" = $[user]