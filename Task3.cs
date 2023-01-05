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

    public class Task3 : IExternalCommand
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

                PushButtonData buttonData = new PushButtonData("cmdThird", "Revit Third Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my third plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        public void Click_Show(UIDocument UiDoc, Document Doc)
        {
            try
            {
   
                FilteredElementCollector viewCollector = new FilteredElementCollector(Doc);
                viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

                FilteredElementCollector wallCollector = new FilteredElementCollector(Doc);
                wallCollector.OfClass(typeof(Autodesk.Revit.DB.Wall));

                foreach (Element viewElement in viewCollector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

                    if (view.Title.Contains("Floor"))
                    {
                        if (view.Name == wpf.CmbLevel_details.Text)
                        {
                            //trans.Commit();

                            UiDoc.ActiveView = view;
                            List<ElementId> wallids = new List<ElementId>();
                            foreach (Element wallElement in wallCollector)
                            {

                                //UiDoc.ActiveView.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(wallElement.Id.IntegerValue.ToString());
                                wallids.Add(wallElement.Id);

                            }
                            UiDoc.Selection.SetElementIds(wallids);

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
            catch (Exception e)
            {

            }
        }






        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UiDoc = commandData.Application.ActiveUIDocument;
            Doc = UiDoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            collector.OfClass(typeof(Autodesk.Revit.DB.View));

            //ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Views);



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

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
        public void Show_Button_Click(object sender, RoutedEventArgs e)
        {
            Click_Show(UiDoc, Doc);
        }
    }
}
