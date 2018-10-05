using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;

namespace SAPADDON.USERMODEL._Menu
{
    /// <summary>
    /// Esta capa se usa para crear automáticamente los menús. Una clase dentro de otra indica un subnivel de menú.
    /// </summary>
    [MenuList]
    public static class _Menu
    {
        [MenuItem(menuTitle = "AddOn Gestión de activos fijos")]
        public static class MENU_PRINCIPAL
        {
            [MenuItem(menuTitle = "Configuración y permisos")]
            public static class MENU_CONFIGURACION
            {
                [MenuItem(menuTitle = "Configuración de series")]
                public const string MENU_CONFIGURACIONSERIES = null;

                [MenuItem(menuTitle = "Configuración de permisos")]
                public const string MENU_CONFIGURACIONPERMISOS = null;
                
                [MenuItem(menuTitle = "Maestros")]
                public static class MENU_MAESTROS
                {
                    [MenuItem(menuTitle = "Maestro prueba 1")]
                    public const string MENU_MAESTROPRUEBA1 = null;
                }
            }

            [MenuItem(menuTitle = "Menu prueba maestro principal")]
            public const string MENU_MAESTROPRUEBAMENU = null;
        }
    }
}
