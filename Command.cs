using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Revit_ass_1
{
    [Transaction(TransactionMode.Manual)]

    class Command : IExternalCommand
    {
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                PushButtonData buttonData = new PushButtonData("cmdFirst", "Revit First Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my first plugin\nVersion : 1.1.0";




                //PushButtonData buttonData2 = new PushButtonData("cmdSecond", "Revit Second Tool", thisAssemblyPath, thisClassName);
                //PushButton pushButton2 = panel.AddItem(buttonData2) as PushButton;
                //pushButton2.ToolTip = "my second plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {

                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Levels);

                ElementCategoryFilter filter2 = new ElementCategoryFilter(BuiltInCategory.OST_Views);


                IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

                string name = ((Autodesk.Revit.DB.Level)floors[1]).Name;
                double x = ((Autodesk.Revit.DB.Level)floors[1]).Elevation;
                double y = ((Autodesk.Revit.DB.Level)floors[1]).ProjectElevation;



                List<string> all_details = new List<string>();
                for (int i = 0; i < floors.Count; i++)
                {
                    string level_name = "Level Name : " + floors[i].Name;
                    string elevation = "        Elevation : " + ((Autodesk.Revit.DB.Level)floors[i]).Elevation;
                    string proj_elevation = "      Project Elevation : " + ((Autodesk.Revit.DB.Level)floors[i]).ProjectElevation;
                    all_details.Add(level_name);
                    all_details.Add(elevation);
                    all_details.Add(proj_elevation);
                }

                var message1 = string.Join(Environment.NewLine, all_details);

                var duplicate = doc.GetElement(floors[1].Id);

                UserControl1 wpf = new UserControl1();
                wpf.Output.Text = message1;
                wpf.Show();


                //MessageBox.Show(message1,"Level Information");
            }
            catch (Exception ex)
            {
            }
            return Result.Succeeded;
        }
    }
}
