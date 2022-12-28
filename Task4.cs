using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;



namespace Revit_ass_1
{
    [Transaction(TransactionMode.Manual)]

    public class Task4 : IExternalCommand
    {

        Button4 wpf = new Button4();

        public Reference pickobj { get; set; }
        public ElementType etype { get; set; }
        public Element ele { get; set; }
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                //PushButtonData buttonData = new PushButtonData("cmdFirst", "Revit First Tool", thisAssemblyPath, thisClassName);
                //PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                //pushButton.ToolTip = "my first plugin\nVersion : 1.1.0";

                PushButtonData buttonData = new PushButtonData("cmdFourth", "Revit Fourth Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my fourth plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        public void Click_Show(UIDocument UiDoc, Document Doc)
        {
            List<string> all_details = new List<string>();

            string a = etype.Name;
            string b = ((Autodesk.Revit.DB.WallType)etype).Width.ToString();
            string c = ele.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString();
            all_details.Add(a);
            all_details.Add("Width :" + b);
            all_details.Add("Length :" + c);

            //foreach (PropertyInfo prop in etype.GetType().GetProperties())
            //{
            //    try
            //    {
            //        string pName = prop.Name;
            //        dynamic pValue;
            //        if (pName == "Name")
            //        {
            //            pValue = etype.Name;
            //        }
            //        else
            //        {
            //            pValue = prop.GetValue(etype);

            //        }
            //        all_details.Add(pName + " = " + pValue);
            //    }
            //    catch (Exception e)
            //    {

            //    }

            //}

            var message1 = string.Join(Environment.NewLine, all_details);

            System.Windows.MessageBox.Show(message1);

        }


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {

                UiDoc = commandData.Application.ActiveUIDocument;
                Doc = UiDoc.Document;


                //pick element from user
                pickobj = UiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);


                //Retrieve element
                ElementId eleid = pickobj.ElementId;
                ele = Doc.GetElement(eleid);


                //Get element type
                ElementId etypeid = ele.GetTypeId();
                etype = Doc.GetElement(etypeid) as ElementType;



                if (etype.FamilyName.Contains("Wall"))
                {
                    if (pickobj != null)
                    {
                        wpf.Show();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Select only walls");
                }

                wpf.Show_Button.Click += Show_Button_Click;

            }
            catch (Exception e)
            {

            }

            return Result.Succeeded;
        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
        public void Show_Button_Click(object sender, RoutedEventArgs e)
        {
            Click_Show(UiDoc, Doc);
        }
    }
}
