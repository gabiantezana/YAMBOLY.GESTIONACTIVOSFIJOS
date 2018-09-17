--HANA

SELECT '' as "Activar"
,x."DocEntry" AS "Despacho"
--,x."U_MSS_COME" AS "Comentario"
,x."U_MSS_MOTI" AS "Motivo"
,x."U_MSS_CODI" as "Vehiculo"
,y."firstName" || ' ' ||  ' ' ||  y."lastName"  AS "Repartidor"
,REPLACE (x."U_MSS_ARTI",'.', '') as "Cant. articulos"
,x."U_MSS_ALMA" as "Almacen"
,x."U_MSS_VOLU" AS "Volumen"
,x."U_MSS_PESO" AS "Peso"
,x."U_MSS_NUME" AS "Direcciones de entrega"
,x."Creator" as "Creador"
,x."U_MSS_FECH" as "Fecha"
,x."U_MSS_ERRO" as "Error de Aprobacion"

FROM "@MSS_DESP" x
JOIN "OHEM" y
ON x."U_MSS_COND" = y."empID"

JOIN "OUSR" a
ON a."LstPwdChB" = x."Creator"

JOIN "@MSS_ASUU" z
ON z."U_MSS_USUA" = a."USER_CODE"

where  x."U_MSS_ESTA" = 'PENDIENTE'
and z."U_MSS_APRO" = 'param1'





