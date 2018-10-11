using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;

namespace SAPADDON.USERMODEL._Menu
{
  
    [MenuList]
    public static class _Menu
    {
        [MenuItem(MenuTitle = "AddOn Gestión de activos fijos")]
        public static class MENU_PRINCIPAL
        {
            [MenuItem(MenuTitle = "Configuración y permisos")]
            public static class MENU_CONFIGURACION
            {
                [MenuItem(MenuTitle = "Configuración de series")]
                public const string MENU_CONFIGURACIONSERIES = "MENU_CONFIGURACIONSERIES";

                [MenuItem(MenuTitle = "Configuración de permisos")]
                public const string MENU_CONFIGURACIONPERMISOS = "MENU_CONFIGURACIONPERMISOS";
            }

            [MenuItem(MenuTitle = "Contrato de concesión de activo fijo")]
            public const string MENU_CONTRATOCONCESIONACTIVOFIJO = "MENU_CONTRATOCONCESIONACTIVOFIJO";
        }
    }
}
