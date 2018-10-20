using System;
using System.Collections.Generic;

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
        public static string PARAM1 = "PARAM1";
        public static string PARAM2 = "PARAM2";
        public static string PARAM3 = "PARAM3";
        public const string PARENTPERMISSIONKEY = "MSS_PERM_PLANIF";
        public const string PARENTPERMISSIONNAME = "AddOn Planificación de Despachos";
        public const string DEFAULT_SAP_SUCCESSBUTTON = "1";

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

        public static string MENU_TITLE = "AddOn Activos Fijos";
        public static string MENU_SAP_ID = "43520";

        public class MenuItem
        {
            public static string UID { get; set; }
            public static string Title { get; set; }
        }

        public static class MenuUID
        {
            public const string _150Form = "3073";
            public const string MenuBuscar = "1281";
            public const string MenuCrear = "1282";
            public const string RegistroDatosSiguiente = "1288";
            public const string RegistroDatosAnterior = "1289";
            public const string RegistroDatosPrimero = "1290";
            public const string RegistroDatosUltimo = "1291";
            public const string AddLine = "1292";
            public const string RemoveLine = "1293";
            public const string RegistroActualizar = "1304";
            public const string InventoryMenu = "3072";

            public const string AddonGestionDespachosMenu = "-2000";
            public const string ConfiguracionAddonSubMenu = "-2002";
            public const string MaestroVehiculosSubMenu = "-2003";
            public const string DespachoVehiculosSubMenu = "-2004";
            public const string AprobacionDespachoVehiculosSubMenu = "-2005";
            public const string AutorizacionusuarioAlmacenSubMenu = "-2006";
            public const string DefinicionUsuarioAprobadorSubMenu = "-2007";


            public const string ConfiguracionSeries = "-2001";
        }


    }
}
