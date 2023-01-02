using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Windows.Media.Imaging;

namespace Revit_ass_1
{
    public class Class1 : IExternalApplication
    {


        Result IExternalApplication.OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        Result IExternalApplication.OnStartup(UIControlledApplication application)
        {

            RibbonPanel panel1 = null;
            RibbonPanel panel2 = null;
            RibbonPanel panel3 = null;
            RibbonPanel panel4 = null;
            RibbonPanel panel5 = null;
            RibbonPanel panel6 = null;
            RibbonPanel panel7 = null;


            string pName = " Revit First Panel";
            string pName2 = " Revit Second Panel";
            string pName3 = " Revit Third Panel";
            string pName4 = " Revit Fourth Panel";
            string pName5 = " Revit Fifth Panel";
            string pName6 = " Revit Sixth Panel";
            string pName7 = " Revit Seventh Panel";
            
            string ncircleTab = " Revit First Tab";
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_UKAssemblyResolveFirst);
            //string pathTools = Assembly.GetExecutingAssembly().Location;


            try
            {
                application.CreateRibbonTab(ncircleTab);
            }
            catch (Exception e) { }
            panel1 = application.CreateRibbonPanel(ncircleTab, pName);
            panel2 = application.CreateRibbonPanel(ncircleTab, pName2);
            panel3 = application.CreateRibbonPanel(ncircleTab, pName3);
            panel4 = application.CreateRibbonPanel(ncircleTab, pName4);
            panel5 = application.CreateRibbonPanel(ncircleTab, pName5);
            panel6 = application.CreateRibbonPanel(ncircleTab, pName6);
            panel7 = application.CreateRibbonPanel(ncircleTab, pName7);

            List<RibbonPanel> panels = application.GetRibbonPanels(ncircleTab);
            foreach (RibbonPanel rP in panels)
            {
                if (rP.Name == pName)
                {
                    panel1 = rP;
                    break;
                }
            }

            if (panel1 == null)
            {
                panel1 = application.CreateRibbonPanel(ncircleTab, pName);
                panel2 = application.CreateRibbonPanel(ncircleTab, pName2);
                panel3 = application.CreateRibbonPanel(ncircleTab, pName3);
                panel4 = application.CreateRibbonPanel(ncircleTab, pName4);
                panel5 = application.CreateRibbonPanel(ncircleTab, pName5);
                panel6 = application.CreateRibbonPanel(ncircleTab, pName6);
                panel7 = application.CreateRibbonPanel(ncircleTab, pName7);

            }

            Command.addButton(panel1);
            Testing.addButton(panel2);
            Task3.addButton(panel3);
            Task4.addButton(panel4);
            Task5.addButton(panel5);
            Task6.addButton(panel6);
            Task7.addButton(panel7);
            
          

            return Result.Succeeded;
        }
    }
}
