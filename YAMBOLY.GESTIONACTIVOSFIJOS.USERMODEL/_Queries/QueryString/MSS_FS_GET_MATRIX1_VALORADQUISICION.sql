﻿SELECT "AttriAm48"
from "ITM13"
WHERE 
	"ItemCode" = $["@MSS_CONT_LINES".U_MSS_AFCI]
	AND $["@MSS_CONT_LINES".U_MSS_AFCI] != '';
