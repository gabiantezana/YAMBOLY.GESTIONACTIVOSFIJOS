﻿SELECT "ItemCode" 
FROM "OITM" 
WHERE 
	"InventryNo"= $["@MSS_CONT_LINES".U_MSS_AFCI]
	AND $["@MSS_CONT_LINES".U_MSS_AFCI] != '';