﻿SELECT
"U_MSS_COUS", 
"U_MSS_COAL",
"U_MSS_PEPE",
* 
FROM "@MSS_CFPE"
WHERE "U_MSS_COUS" = 'PARAM1'
AND "U_MSS_COAL" ='PARAM2'
AND "PARAM3" = 'Y'