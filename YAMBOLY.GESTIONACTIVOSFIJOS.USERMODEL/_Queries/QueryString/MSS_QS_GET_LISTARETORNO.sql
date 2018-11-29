SELECT
'' 				as "Seleccionar"
,'' 			as "Estado"
,"U_MSS_AFCO" 	as "Código AF"
,"U_MSS_AFDE" 	as "Descripción AF"
FROM "@MSS_CONT_LINES" 
WHERE "DocEntry" = 'PARAM1'
AND "U_MSS_ESTD" != 'PARAM2' 
