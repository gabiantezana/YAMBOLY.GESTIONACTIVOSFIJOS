                                                                                                                                                                                                                                                                        {´´l                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             SELECT distinct
 ''					AS "LineId"
 ,b."DocEntry"		AS "U_MSS_DOCE"
 ,b."DocNum"		AS "U_MSS_ORDE"
 ,a."LineNum"		AS "U_MSS_LINE"
 ,b."CardCode"		AS "U_MSS_CODC"
 ,b."CardName"		AS "U_MSS_NOMB"
 ,b."ShipToCode"	AS "U_MSS_IDDI"
 ,b."Address"		AS "U_MSS_DIRE"
 ,a."ItemCode"		AS "U_MSS_CODA"
 ,a."Dscription"	AS "U_MSS_DESC"
 ,a."unitMsr"		AS "U_MSS_UNID"
 ,a."OpenCreQty"	AS "U_MSS_CANP"
 ,d."OnHand"		AS "U_MSS_CANA"
 ,a."OpenCreQty"	AS "U_MSS_CAND"
 ,e."SWeight1"		AS "U_MSS_PEST"
 ,e."SVolume"		AS "U_MSS_VOLT"
FROM "RDR1" a

LEFT JOIN "ORDR" b
ON a."DocEntry" = b."DocEntry"

LEFT JOIN "CRD1" c
ON b."ShipToCode" = c."Address"

LEFT JOIN "OITW" d
ON a."ItemCode" = d."ItemCode" 

LEFT JOIN "OITM" e
on a."ItemCode" = e."ItemCode"

WHERE b."DocEntry" = 'param1' 
and a."LineNum" = 'param2' 
and a."WhsCode" = d."WhsCode"
