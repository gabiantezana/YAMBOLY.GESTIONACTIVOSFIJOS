﻿UPDATE "@MSS_CONT_LINES" 
SET "U_MSS_ESTD" = 'PARAM2' 
WHERE 
	"U_MSS_AFCO" = 'PARAM1'
	AND "DocEntry" = 'PARAM3'