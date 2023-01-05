using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
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

    public class Task10 : IExternalCommand
    {

        Button9 wpf = new Button9();

        List<string> openings_wall = new List<string>();
        public Reference pickobj { get; set; }
        //public selection pickobj { get; set; }
        public ElementType etype { get; set; }
        public Element ele { get; set; }
        public static void addButton(RibbonPanel panel)
        {
            try
            {
                string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                PushButtonData buttonData = new PushButtonData("cmdTenth", "Revit Tenth Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my tenth plugin\nVersion : 1.1.0";
            }
            catch (Exception ex)
            {
            }
        }

       
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {

                UiDoc = commandData.Application.ActiveUIDocument;
                Doc = UiDoc.Document;


                Autodesk.Revit.UI.Selection.Selection selection = UiDoc.Selection;
                ICollection<Autodesk.Revit.DB.ElementId> selectedIds = UiDoc.Selection.GetElementIds();

                //Selcted wall by user
                try
                {
                    Wall selected_wall = Doc.GetElement(selectedIds.ToArray()[0]) as Wall;

                    FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
                    IList elementsInView = (IList)allElementsInView.ToElements();

                    openings_wall.Add("Wall name : " + selected_wall.Name);

                    foreach (Element el in elementsInView)
                    {
                        try
                        {
                            FamilyInstance fi = el as FamilyInstance;

                            if (fi.Host.Id == selected_wall.Id)
                            {
                                openings_wall.Add("     Opening present- Category :" + el.Category.Name + "   " + "Name :" + el.Name);
                            }
                        }
                        catch (Exception e)
                        {

                        }

                    }

                    if (openings_wall.Count == 1)
                    {
                        openings_wall.Add("     No openings are present");
                    }


                    wpf.WallsData.Text = string.Join(Environment.NewLine, openings_wall);

                    wpf.Show();
                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show("Please Select only wall from current view");
                }



                
               
                
                

            }
            catch (Exception e)
            {

            }

            return Result.Succeeded;
        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set;}
        
    }
}
