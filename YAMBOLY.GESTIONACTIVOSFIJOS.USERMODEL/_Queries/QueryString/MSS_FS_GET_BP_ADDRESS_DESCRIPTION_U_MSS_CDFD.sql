SELECT   
IFNULL( x."Street", '') ||', '||
IFNULL( x."City",'')	||', '||
IFNULL( x."County",'') 	||', '|| 
IFNULL( y."Name", '')
FROM "CRD1" x
LEFT JOIN "OCST" y
on x."State" = y."Code"
WHERE 
	x."AdresType" = 'B' 
	AND (y."Country" = 'PE' OR y."Country" is null)
	AND "CardCode" = $["@MSS_CONT".U_MSS_CCOD]
	AND x."Address" = $["@MSS_CONT".U_MSS_CDFI]