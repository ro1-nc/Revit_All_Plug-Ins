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

            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            collector.OfClass(typeof(Autodesk.Revit.DB.View));

            string docname = Doc.Title;

            List<Element> all_elements = new List<Element>();

            wpf_output_data.Clear();

            foreach (Element viewElement in collector)
            {
                Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;
                // view_types.Add(view.ViewType.ToString());
                if (view.Title.Contains("Floor"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    floor_plans.Add(view.Name);
                 
                    WallData(view);    

                }
                if (view.Title.StartsWith("Reflected Ceiling"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    ceiling_plans.Add(view.Name);
                    
                    WallData(view);
                }
                if (view.Title.StartsWith("Elevation"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    elevation_plans.Add(view.Name);
                    
                    WallData(view);
                }
                if (view.Title.StartsWith("3D"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    threeD_plans.Add(view.Name);
                    
                    WallData(view);
                }              
            }

            wpf.WallsData.Text =string.Join(Environment.NewLine, wpf_output_data);
           
            wpf.Show();

            return Result.Succeeded;
        }

        public void WallData(Autodesk.Revit.DB.View view)
        {
            FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
            IList elementsInView = (IList)allElementsInView.ToElements();
            
            wpf_output_data.Add(view.Title);
            foreach (Element el in elementsInView)
            {
                if (view.Id.IntegerValue == el.LevelId.IntegerValue + 1)
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
        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
       
    }
}
