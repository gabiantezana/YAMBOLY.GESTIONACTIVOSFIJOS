--SQL
/*SELECT Code, U_MSS_USUA, U_MSS_ALMA FROM [SBO_JHOMERON]..[@MSS_ASUA] where U_MSS_USUA = 'param1'*/

--HANA
SELECT "Code", "U_MSS_USUA", "U_MSS_ALMA" FROM "@MSS_ASUA" where "U_MSS_USUA" = 'param1'