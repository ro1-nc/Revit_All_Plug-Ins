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

    public class Task6 : IExternalCommand
    {

        Button4 wpf = new Button4();

       

        List<string> all_details = new List<string>();
        //public selection pickobj { get; set; }
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

                PushButtonData buttonData = new PushButtonData("cmdSixth", "Revit Sixth Tool", thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = "my sixth plugin\nVersion : 1.1.0";
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

                //pick element from user
                //UiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.Selection.)=pickobj ;


                //Selcted room by user
                Room room = Doc.GetElement(selectedIds.ToArray()[0]) as Room;

                FilteredElementCollector rooms = new FilteredElementCollector(Doc)
          .WhereElementIsNotElementType()
           .OfCategory(BuiltInCategory.OST_Rooms);

                Dictionary<ElementId, List<string>> map_wall_to_rooms = new Dictionary<ElementId,List<string>>();

                SpatialElementBoundaryOptions opts  = new SpatialElementBoundaryOptions();

                FilteredElementCollector collector = new FilteredElementCollector(Doc, selectedIds);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);

                IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();


                
                IList<Subelement> walls = new List<Subelement>(); 
                foreach (Room r in rooms)
                {
                    if (r.Name == room.Name)
                    {
                        IList<IList<BoundarySegment>> boundary= r.GetBoundarySegments(opts);

                        foreach (BoundarySegment bs in boundary[0])
                        {
                            Element eFromString = Doc.GetElement(bs.ElementId);
                            all_details.Add(eFromString.Name);
                        }

                    }
                        
                }

                //Above code is sufficient
                //Comment below code
                //foreach (Wall w in filter)
                //{

                //}

                //Retrieve element
                //ElementId eleid = pickobj.ElementId;
                //ele = Doc.GetElement(eleid);


                ////Get element type
                //ElementId etypeid = ele.GetTypeId();
                //etype = Doc.GetElement(etypeid) as ElementType;



                //if (etype.FamilyName.Contains("Wall"))
                //{
                //    if (pickobj != null)
                //    {
                //        wpf.Show();
                //    }
                //}
                //else
                //{
                //    System.Windows.MessageBox.Show("Select only walls");
                //}

                //wpf.Show_Button.Click += Show_Button_Click;
                System.Windows.MessageBox.Show(string.Join(Environment.NewLine, all_details),"List of walls");
            }
            catch (Exception e)
            {

            }

            return Result.Succeeded;
        }

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }
       
    }
}
