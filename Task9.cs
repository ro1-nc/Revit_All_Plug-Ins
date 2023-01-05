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
using System.Windows.Controls;
using System.Windows.Forms;



namespace Revit_ass_1
{
    [Transaction(TransactionMode.Manual)]

    public class Task9 : IExternalCommand
    {
        List<string> all_views = new List<string>();
        List<string> floor_plans = new List<string>();
        List<string> ceiling_plans = new List<string>();
        List<string> elevation_plans = new List<string>();
        List<string> threeD_plans = new List<string>();
        List<string> view_types = new List<string>();

        List<string> wpf_output_data = new List<string>();

        Button9 wpf = new Button9();
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                PushButtonData buttonData = new PushButtonData("cmdNineth", "Revit Nineth Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my nineth plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UiDoc = commandData.Application.ActiveUIDocument;
            Doc = UiDoc.Document;

            wpf_output_data.Clear();

            WallData();
          
            wpf.WallsData.Text =string.Join(Environment.NewLine, wpf_output_data);
           
            wpf.Show();

            return Result.Succeeded;
        }

        public void WallData()
        {
            
            List<Level> levelCollection = new List<Level>();
            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();

            FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
            IList elementsInView = (IList)allElementsInView.ToElements();
            
            
            foreach(Level lev in collection)
            {
                wpf_output_data.Add(lev.Name);
                //if(view.l)
                foreach (Element el in elementsInView)
                {
                    if (lev.Id == el.LevelId)
                    {

                        if (el.GetType() == typeof(Wall))
                        {
                            //all_elements.Add(el);
                            wpf_output_data.Add("   Name : " + el.Name);
                            wpf_output_data.Add("       Width : " + ((Autodesk.Revit.DB.Wall)el).Width.ToString());
                            wpf_output_data.Add("       Length : " + el.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString());

                        }
                    }
                }
            }
            
        }
        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
       
    }
}
