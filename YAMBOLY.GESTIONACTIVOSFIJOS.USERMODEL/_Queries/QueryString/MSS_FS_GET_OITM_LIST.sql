SELECT "InventryNo", "ItemCode", "ItemName" 
FROM "OITM"
WHERE "ItemType" = 'F'
AND "InventryNo" IS NOT NULL
AND (	"U_MSS_EAAF" = '01'
		OR "U_MSS_EAAF" IS NULL
		OR "U_MSS_EAAF" = ''
	);