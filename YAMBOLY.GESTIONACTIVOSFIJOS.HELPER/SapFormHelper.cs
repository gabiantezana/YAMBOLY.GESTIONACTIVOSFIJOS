using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class SapFormHelper
    {
        public static Form CreateForm(SAPbouiCOM.Application application, string stringXml, string formType)
        {
            FormCreationParams fCreationParams = application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
            fCreationParams.XmlData = stringXml;
            fCreationParams.FormType = formType;
            //fCreationParams.UniqueID = formName + DateTime.Now.ToString("hhmmss");
            var uid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            fCreationParams.UniqueID = uid;

            var mForm = application.Forms.AddEx(fCreationParams);
            mForm.Settings.Enabled = true;
            mForm.Visible = true;
            mForm.Mode = BoFormMode.fm_ADD_MODE;
            return mForm;
        }

        public static void ShowMessage(Application application, BoStatusBarMessageType messageType, BoMessageTime messageTime, String message)
        {
            application.StatusBar.SetText(message, messageTime, messageType);
        }

        private Int32? _ItemId { get; set; }
        public String GetNewItemId()
        {
            var id = (_ItemId ?? 1000) + 1;
            _ItemId = id;
            return _ItemId.ToString();
        }

        public static String CreateUID(String formName)
        {
            String txt = (formName ?? "Item") + "_";
            return txt + DateTime.Now.ToString("hhmmss");
        }

        /*
        Int32 _leftLbl = 7;
        Int32 _leftTxt = _leftLbl + 150;
        Int32 _widthLbl = 148;
        Int32 _widthTxt = _widthLbl + 15;

        Int32 _top = 8; //variación: +16
        Int32 _height = 14;
        */

        public SAPbouiCOM.Item item;
        public SAPbouiCOM.Button oButton;
        public SAPbouiCOM.StaticText oStaticText;
        public SAPbouiCOM.EditText oEditText;
        public SAPbouiCOM.ComboBox oComboBox;


        private Int32 _leftLbl { get; set; }
        public Int32 Left_Label { get { return _leftLbl; } }

        private Int32 _leftTxt { get; set; }
        public Int32 Left_Text { get { return _leftTxt; } }

        private Int32 _widthLbl { get; set; }
        public Int32 Width_Label { get { return _widthLbl; } }

        private Int32 _widthTxt { get; set; }
        public Int32 Width_Text { get { return _widthTxt; } }

        private Int32 _top { get; set; }
        public Int32 Top { get { return _top; } }

        private Int32 _height { get; set; }
        public Int32 Height { get { return _height; } }


        public void StartToDraw()
        {
            _leftLbl = 7;
            _leftTxt = _leftLbl + 150;
            _widthLbl = 148;
            _widthTxt = _widthLbl + 15;

            _top = 8;
            _height = 14;
        }

        public void AddRow() { _top += 16; }

        public void AddSecondColumn()
        {
            _leftLbl = 350;
            _leftTxt = _leftLbl + 150;
            _widthLbl = 148;
            _widthTxt = _widthLbl + 15;

            _top = 8; //variación: +16
            _height = 14;
        }

        public void AddItem(Form form, SAPbouiCOM.BoFormItemTypes type)
        {
            item = form.Items.Add(GetNewItemId(), type);
            item.Left = Left_Label;
            item.Width = Width_Label;
            item.Top = Top;
            item.Height = Height;
        }

        public void AddEditText(Form form, BoFormItemTypes type, String textToShow, string tableNamme, string fieldNames, String[][] validValues = null)
        {
            item = form.Items.Add(GetNewItemId(), BoFormItemTypes.it_STATIC);
            item.Left = Left_Label;
            item.Width = Width_Label;
            item.Top = Top;
            item.Height = Height;
            oStaticText = item.Specific;
            oStaticText.Caption = textToShow;
            ///*-----------------------------------------------------------------------*/
            item = form.Items.Add(GetNewItemId(), type);
            item.Left = Left_Text;
            item.Width = Width_Text;
            item.Top = Top;
            item.Height = Height;
            switch (type)
            {
                case BoFormItemTypes.it_EDIT:
                    oEditText = item.Specific;
                    oEditText.DataBind.SetBound(true, "", "EditSource");
                    break;
                case BoFormItemTypes.it_COMBO_BOX:
                    oComboBox = item.Specific;
                    oComboBox.ValidValues.Add("1", "Combo Value 1");
                    oComboBox.ValidValues.Add("2", "Combo Value 2");
                    oComboBox.ValidValues.Add("3", "Combo Value 3");
                    oComboBox.DataBind.SetBound(true, "", "CombSource");
                    break;
                case BoFormItemTypes.it_BUTTON:
                case BoFormItemTypes.it_STATIC:
                case BoFormItemTypes.it_FOLDER:
                case BoFormItemTypes.it_RECTANGLE:
                case BoFormItemTypes.it_LINKED_BUTTON:
                case BoFormItemTypes.it_PICTURE:
                case BoFormItemTypes.it_EXTEDIT:
                case BoFormItemTypes.it_CHECK_BOX:
                case BoFormItemTypes.it_OPTION_BUTTON:
                case BoFormItemTypes.it_MATRIX:
                case BoFormItemTypes.it_GRID:
                case BoFormItemTypes.it_PANE_COMBO_BOX:
                case BoFormItemTypes.it_ACTIVE_X:
                case BoFormItemTypes.it_BUTTON_COMBO:
                case BoFormItemTypes.it_WEB_BROWSER:
                default:
                    break;
            }

            AddRow();
        }

        public void AddDefaultButtons(Form form)
        {
            item = form.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            item.Left = Left_Label;
            item.Width = Width_Label;
            item.Top = Top;
            item.Height = Height;
            oButton = (SAPbouiCOM.Button)item.Specific;
            oButton.Caption = "Ok";
            /*-----------------------------------------------------------------------*/
            item = form.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            item.Left = Left_Text;
            item.Width = Width_Text;
            item.Top = Top;
            item.Height = Height;
            oButton = (SAPbouiCOM.Button)item.Specific;
            oButton.Caption = "Cancel";

            AddRow();
        }

    }
}
