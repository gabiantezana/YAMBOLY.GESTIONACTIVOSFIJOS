SELECT x.*,  y."CardName" as "U_MSS_RAZO" FROM "@MSS_VEHI" x 

LEFT JOIN "OCRD" y 
ON y."CardCode" = x."U_MSS_CODS" 
WHERE "Code" = 'param1' 