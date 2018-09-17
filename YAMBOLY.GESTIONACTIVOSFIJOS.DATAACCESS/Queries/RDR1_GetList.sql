--HANA
 SELECT DISTINCT
b."DocDueDate" 					as "Fecha Entrega"
,case when b."U_MSSM_CRM" = 'Y'
  THEN 
  	'SI' 
  ELSE 
  	'NO' 
  END  							AS "Origen movil"
,d2."U_MSS_RUTA" 				as "Ruta"
,d2."U_MSS_NOVE"					as "Vendedor"
,b."CardName" 					as "Cliente"
,b."DocNum" 					as "Codigo OV"
,a."LineNum" 					as "LineNum Ref."
,a."ItemCode" 					as "Codigo articulo"
,a."Dscription" 				as "Descripcion"
,a."unitMsr" 					as "Unidad medida"
,a."OpenQty" 				as "Cant. pendiente"
,cast(b."DocEntry" as varchar)
 || '-' || 
 cast( a."LineNum" as varchar)	as "InternalCode"
 ,''							as "[]"
FROM "RDR1" a

JOIN "ORDR" b
on a."DocEntry" = b."DocEntry"

JOIN "OCRD" c
on b."CardCode" = c."CardCode"

JOIN "OCRG" d
on c."GroupCode" = d."GroupCode"

JOIN "CRD1" d2
on b."CardCode" = d2."CardCode"

LEFT JOIN "PKL1" e
ON e."OrderEntry" = a."DocEntry" 
and e."OrderLine" = a."LineNum"  --LEFT JOIN SOLO A LOS PICKING QUE NO EST�N CERRADOS EN DETALLE LINESTATUS. DEBE SER =='C'

--LEFT JOIN "OSLP" f
--on b."SlpCode" = f."SlpCode"




WHERE 
a."ShipDate" <= TO_DATE('param1', 'yyyyMMdd') and 
a."LineStatus"  = 'O' and -- S�lo l�neas que tengan estado abierto.
b."CANCELED" = 'N' and-- s�lo �rdenes de venta que no est�n canceladas.
b."DocStatus" = 'O' and-- S�lo �rdenes de ventas que tengan estado abierto.
a."PickStatus" = 'N' and -- //????
a."WhsCode" =  'param2'


 