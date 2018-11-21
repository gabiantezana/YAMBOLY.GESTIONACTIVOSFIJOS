using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE
{
    [DBStructure]
    [SAPUDO(Name = "AF- ConfiguracionSeries",
           HeaderTableType = typeof(MSS_CFSE),
           CanCreateDefaultForm = BoYesNoEnum.tNO,      
           CanFind = BoYesNoEnum.tYES,
           EnableEnhancedForm = BoYesNoEnum.tYES,
           RebuildEnhancedForm = BoYesNoEnum.tYES,
           ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document
   )]
    public class MSS_CFSE_UDO
    {
    }
}
