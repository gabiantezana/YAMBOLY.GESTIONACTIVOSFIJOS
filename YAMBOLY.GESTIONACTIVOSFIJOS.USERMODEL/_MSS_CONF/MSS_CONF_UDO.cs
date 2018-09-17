using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONF
{
    [DBStructure]
    [SAPUDO(Name = "CONFIGURACION",
           HeaderTableType = typeof(MSS_CONF),
           CanCreateDefaultForm = BoYesNoEnum.tNO,
           CanFind = BoYesNoEnum.tNO,
           EnableEnhancedForm = BoYesNoEnum.tYES,
           RebuildEnhancedForm = BoYesNoEnum.tYES,
           ObjectType = SAPbobsCOM.BoUDOObjType.boud_MasterData
   )]
    public class MSS_CONF_UDO
    {

    }
}
