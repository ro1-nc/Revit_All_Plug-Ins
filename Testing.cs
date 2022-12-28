using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
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

    public class Testing : IExternalCommand
    {
        Button2 wpf = new Button2();

        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                //PushButtonData buttonData = new PushButtonData("cmdFirst", "Revit First Tool", thisAssemblyPath, thisClassName);
                //PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                //pushButton.ToolTip = "my first plugin\nVersion : 1.1.0";

                PushButtonData buttonData2 = new PushButtonData("cmdSecond", "Revit Second Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton2 = panel.AddItem(buttonData2) as PushButton;
                pushButton2.ToolTip = "my second plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        public void Click_Show(UIDocument UiDoc, Document Doc)
        {



            //using (Transaction trans = new Transaction(Doc, "Viewing Levels"))
            //{
            //    trans.Start();
            //Button2 wpf = new Button2();
            //wpf.CmbLevel_details.AllowDrop = true;


            FilteredElementCollector viewCollector = new FilteredElementCollector(Doc);
            viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

            foreach (Element viewElement in viewCollector)
            {
                Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

                if (view.Title.Contains("Floor"))
                {
                    if (view.Name == wpf.CmbLevel_details.Text)
                    {
                        //trans.Commit();

                        UiDoc.ActiveView = view;

                        break;
                    }
                }
                else if (wpf.CmbLevel_details.Text == "")
                {
                    System.Windows.MessageBox.Show("Please Select Appropriate Level from Dropdownlist");
                }



                //}
            }

        }






        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {




                UiDoc = commandData.Application.ActiveUIDocument;
                Doc = UiDoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(Doc);
                collector.OfClass(typeof(Autodesk.Revit.DB.View));

                //ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Levels);

                //IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();


                wpf.CmbLevel_details.AllowDrop = true;

                foreach (Element viewElement in collector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

                    if (view.Title.Contains("Floor"))
                    {
                        wpf.CmbLevel_details.Items.Add(view.Name);
                    }
                }
                wpf.Show();

                wpf.Show_Button.Click += Show_Button_Click;



                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
        public void Show_Button_Click(object sender, RoutedEventArgs e)
        {
            Click_Show(UiDoc, Doc);
        }
    }
}
