UPDATE "ORDR" _ORDR
SET
_ORDR."U_MSSL_MVH"  =  _DESP."U_MSSL_MVH",
_ORDR."U_MSSL_PVH"  =  _DESP."U_MSSL_PVH",
_ORDR."U_MSS_CECI"  =  _DESP."U_MSS_CECI",
_ORDR."U_MSSL_RTR"  =  _DESP."U_MSSL_RTR",
_ORDR."U_MSSL_NTR"  =  _DESP."U_MSSL_NTR",
_ORDR."U_MSSL_DTR"  =  _DESP."U_MSSL_DTR",
_ORDR."U_MSSL_NCD"  =  _DESP."U_MSSL_NCD",
_ORDR."U_MSSL_LCD"  =  _DESP."U_MSSL_LCD",   
_ORDR."U_MSS_FETR"  =  _DESP."U_MSS_FETR",
_ORDR."U_MSS_ORIG"  =  _DESP."U_MSS_ORIG",  
_ORDR."U_MSS_DEST"  =  _DESP."U_MSS_DEST"   
FROM
(
SELECT DISTINCT
--DATOS DE VEHÍCULO
 VEHI."U_MSS_MARC"															AS "U_MSSL_MVH" --Marca
,VEHI."U_MSS_PLAC"															AS "U_MSSL_PVH" --Placa
,VEHI."U_MSS_CMTC"															AS "U_MSS_CECI" --Cert.
--DATOS DEL TRANSPORTISTA
,VEHI_OCRD."LicTradNum"														AS "U_MSSL_RTR" --RUC de SN vehículo
,VEHI_OCRD."CardName"														AS "U_MSSL_NTR" --Nombres de SN vehículo
,VEHI_OCRD_CRD1."Street"													AS "U_MSSL_DTR" --Dirección de SN vehículo
--DATOS DE CONDUCTOR
,COALESCE(COND."firstName",' ')|| COALESCE(COND."middleName",' ')||COALESCE( COND."lastName",'')		AS "U_MSSL_NCD" --Nombre conductor
,COND."U_MSS_LICE"															AS "U_MSSL_LCD" --Lic. conducir
--DATOS GENERALES
,DESP."U_MSS_FECH"															AS "U_MSS_FETR" --Fecha despacho
,ALMA."StreetNo"															AS "U_MSS_ORIG" --Origen despacho
, (SELECT top 1 "U_MSS_DIRE" FROM "@MSS_DESP_LINES" DESPLINES 
	WHERE
		DESPLINES."DocEntry" = 'param1'
		AND DESPLINES."U_MSS_LINE" IS NOT NULL
	ORDER BY
		(DESPLINES."LineId") DESC)											AS "U_MSS_DEST" --Destino despacho

FROM "@MSS_DESP" DESP

LEFT JOIN "@MSS_VEHI" VEHI
ON DESP."U_MSS_CODI" = VEHI."Code"

LEFT JOIN "OCRD" VEHI_OCRD
ON VEHI."U_MSS_CODS" = VEHI_OCRD."CardCode"

LEFT JOIN "CRD1" VEHI_OCRD_CRD1
ON VEHI_OCRD."CardCode" = VEHI_OCRD_CRD1."CardCode"

LEFT JOIN "OHEM" COND
ON DESP."U_MSS_COND" = COND."empID"

LEFT JOIN "OWHS" ALMA
ON DESP."U_MSS_ALMA" = ALMA."WhsCode"

WHERE DESP."DocEntry" = 'param1') _DESP

WHERE _ORDR."DocEntry" = 'param2'


