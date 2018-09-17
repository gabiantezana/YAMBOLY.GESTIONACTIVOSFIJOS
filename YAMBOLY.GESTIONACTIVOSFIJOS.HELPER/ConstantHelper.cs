using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class ConstantHelper
    {
        public const Int32 ApplicationId = -1;
        public static Int32 DefaulSuccessSAPNumber = 0;
        public const Int32 DefaultFieldSize = 10;
        public const String ADDON_NAME = "MSS ADDON DESPACHOS";
        public const String DEFAULT_ERROR_MESSAGE = "Some error occurred in application.Please contact your administrator.";
        public const String DEFAULT_SUCCESS_MESSAGE = "Successfuly operation";
        public static String DATEFORMAT = "yyyyMMdd";
        public static String DEFAULTDATENULL = "30/12/1899 00:00:00";
        public static string PARAM1 = "param1";
        public static string PARAM2 = "param2";
        public const  string PARENTPERMISSIONKEY = "MSS_PERM_PLANIF";
        public const  string PARENTPERMISSIONNAME = "AddOn Planificación de Despachos";


        public static class SAP_YES_NO
        {
            public static String YES = "Y";
            public static String NO = "N";
        }

        public static class OrigenVehiculo
        {
            public static string Propio = "01";
            public static string Tercero = "02";
        }

        public static class EstadoVehiculo
        {
            public static string Disponible = "DI";
            public static string Ruta = "RU";
            public static string EnMantenimiento = "MA";
            public static string Baja = "BA";
        }

        public static class EstadoPlanificacionDespacho
        {
            public const string Pendiente = "PENDIENTE";
            public const string Aprobado = "APROBADO";
            public const string Rechazado = "RECHAZADO";
        }

    }
}
