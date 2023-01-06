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

    public class Task11 : IExternalCommand
    {

        List<string> wpf_output_data = new List<string>();

        Button7 wpf = new Button7();
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                PushButtonData buttonData = new PushButtonData("cmdEleventh", "Revit Eleventh Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my eleventh plugin\nVersion : 1.1.0";
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

            //wpf.WallsData.Text = string.Join(Environment.NewLine, wpf_output_data);

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



            foreach (Level lev in collection)
            {
                TreeViewItem levels = new TreeViewItem();
                levels.Header = lev.Name;
                wpf.All_views.Items.Add(levels);
                wpf_output_data.Add(lev.Name);
                //if(view.l)
                Dictionary<string, int> dict = new Dictionary<string, int>();
                foreach (Element el in elementsInView)
                {
                    if (null != el.Category && el.Category.HasMaterialQuantities)
                    {
                        if (lev.Id == el.LevelId)
                        {
                            wpf_output_data.Add(el.Category.Name);
                            if (dict.ContainsKey(el.Category.Name))
                            {
                                dict = dict.ToDictionary(kvp => kvp.Key, kv => kv.Value + 1);
                            }
                            else
                            {
                                dict.Add(el.Category.Name, 1);
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, int> entry in dict)
                {
                    TreeViewItem elementss = new TreeViewItem();
                    elementss.Header = string.Format("{0}" + "(" + "{1}" + ")", entry.Key, entry.Value);
                    levels.Items.Add(elementss);

                    elementss.MouseDoubleClick += Elementss_MouseDoubleClick;
                }
            }

        }

        public void Highlight_Element(UIDocument UiDoc, Document Doc)
        {
            try
            {

                string selected = wpf.All_views.SelectedItem.ToString();

                List<Level> levelCollection = new List<Level>();
                FilteredElementCollector collector = new FilteredElementCollector(Doc);
                ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();

                FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
                IList elementsInView = (IList)allElementsInView.ToElements();


                List<ElementId> wallids = new List<ElementId>();
                try
                {
                    foreach (Element elem in elementsInView)
                    {
                        try
                        {
                            if (selected.Contains(elem.Category.Name))
                            {
                                wallids.Add(elem.Id);
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                catch (Exception e)
                {

                }

                Element elemt = Doc.GetElement(wallids.First());

                bool found = false;
                foreach (Level lev in collection)
                {

                    if (lev.Id == elemt.LevelId)
                    {
                        FilteredElementCollector viewCollector = new FilteredElementCollector(Doc);
                        viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

                        foreach (Autodesk.Revit.DB.View v in viewCollector)
                        {
                            if (v.Title.Contains("Floor"))
                            {
                                if (v.Name == lev.Name)
                                {
                                    UiDoc.ActiveView = v;
                                    UiDoc.Selection.SetElementIds(wallids);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (selected == "")
                        {
                            System.Windows.MessageBox.Show("Please Select Appropriate View from Dropdownlist");
                            break;
                        }
                    }
                }
                if (!found)
                {
                    System.Windows.MessageBox.Show("Please Select Appropriate View from Tree");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Elementss_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Highlight_Element(UiDoc, Doc);
        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }

    }
}
