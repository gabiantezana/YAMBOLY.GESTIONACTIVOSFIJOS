SELECT "InventryNo", "ItemCode", "ItemName" 
FROM "OITM"
WHERE "ItemType" = 'F'
AND "InventryNo" IS NOT NULL
