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

    public class Task7 : IExternalCommand
    {
        List<string> all_views = new List<string>();
        List<string> floor_plans = new List<string>();
        List<string> ceiling_plans = new List<string>();
        List<string> elevation_plans = new List<string>();
        List<string> threeD_plans = new List<string>();
        List<string> view_types = new List<string>();

        public dynamic PARENT_NAME;

        Button7 wpf = new Button7();
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                //PushButtonData buttonData = new PushButtonData("cmdFirst", "Revit First Tool", thisAssemblyPath, thisClassName);
                //PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                //pushButton.ToolTip = "my first plugin\nVersion : 1.1.0";

                PushButtonData buttonData = new PushButtonData("cmdSeventh", "Revit Seventh Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my seventh plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

        //public void Click_Show(UIDocument UiDoc, Document Doc)
        //{
        //    try
        //    {


        //        FilteredElementCollector viewCollector = new FilteredElementCollector(Doc);
        //        viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

        //        FilteredElementCollector wallCollector = new FilteredElementCollector(Doc);
        //        wallCollector.OfClass(typeof(Autodesk.Revit.DB.Wall));

        //        foreach (Element viewElement in viewCollector)
        //        {
        //            Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

        //            if (view.Title.Contains("Floor"))
        //            {
        //                //if (view.Name == wpf.CmbLevel_details.Text)
        //                //{
        //                //    //trans.Commit();

        //                //    UiDoc.ActiveView = view;
        //                //    List<ElementId> wallids = new List<ElementId>();
        //                //    foreach (Element wallElement in wallCollector)
        //                //    {

        //                //        //UiDoc.ActiveView.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(wallElement.Id.IntegerValue.ToString());
        //                //        wallids.Add(wallElement.Id);

        //                //    }
        //                //    UiDoc.Selection.SetElementIds(wallids);

        //                //    break;
        //                //}
        //            }
        //            //else if (wpf.CmbLevel_details.Text == "")
        //            //{
        //            //    System.Windows.MessageBox.Show("Please Select Appropriate Level from Dropdownlist");
        //            //}



        //            //}
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UiDoc = commandData.Application.ActiveUIDocument;
            Doc = UiDoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            collector.OfClass(typeof(Autodesk.Revit.DB.View));

            //ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Views);

            string docname = Doc.Title;

            TreeViewItem main = new TreeViewItem();


            main.Header = docname;
            wpf.All_views.Items.Add(main);


            foreach (Element viewElement in collector)
            {
                Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;
                // view_types.Add(view.ViewType.ToString());
                if (view.Title.Contains("Floor"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    floor_plans.Add(view.Name);
                }
                if (view.Title.StartsWith("Reflected Ceiling"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    ceiling_plans.Add(view.Name);
                }
                if (view.Title.StartsWith("Elevation"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    elevation_plans.Add(view.Name);
                }
                if (view.Title.StartsWith("3D"))
                {
                    view_types.Add(view.ViewType.ToString());
                    all_views.Add(view.Title);
                    threeD_plans.Add(view.Name);
                }
            }

            var viewtypes_set = new HashSet<string>(view_types);

            view_types = viewtypes_set.ToList();

            foreach (string v in view_types)
            {
                TreeViewItem view_type = new TreeViewItem();
                view_type.Header = v;
                main.Items.Add(view_type);
                if (v.Contains("Floor"))
                {
                    foreach (var item in floor_plans)
                    {
                        view_type.Items.Add(item);
                    }
                }
                if (v.Contains("Ceiling"))
                {
                    foreach (var item in ceiling_plans)
                    {
                        view_type.Items.Add(item);
                    }
                }
                if (v.Contains("Elevation"))
                {
                    foreach (var item in elevation_plans)
                    {
                        view_type.Items.Add(item);
                    }
                }
                if (v.Contains("Three"))
                {
                    foreach (var item in threeD_plans)
                    {
                        view_type.Items.Add(item);
                    }
                }
                 PARENT_NAME = view_type;
                view_type.MouseDoubleClick += View_type_MouseDoubleClick;


            }

            wpf.Show();

            return Result.Succeeded;
        }

        private void View_type_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Activate_Plan(UiDoc, Doc);
        }

        public void Activate_Plan(UIDocument UiDoc, Document Doc)
        {
 
            try
            {

                string selected= wpf.All_views.SelectedItem.ToString();

                FilteredElementCollector viewCollector = new FilteredElementCollector(Doc);
                viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

                foreach (Element viewElement in viewCollector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;


                    if (view.Name.Contains(selected))
                    {
                        //trans.Commit();

                        UiDoc.ActiveView = view;

                        break;
                    }

                    else if(selected=="")
                    {
                        System.Windows.MessageBox.Show("Please Select Appropriate View from Dropdownlist");
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
            }

        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
        //public void Show_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Click_Show(UiDoc, Doc);
        //}
    }
}
