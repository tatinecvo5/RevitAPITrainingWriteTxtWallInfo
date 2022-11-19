using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingWriteTxtWallInfo
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string wallInfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
            .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            foreach (Wall wall in walls)
            {
                double wallVolume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                wallInfo += $"{wall.Name}\t{wallVolume}{Environment.NewLine}";
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string txtPath = Path.Combine(desktopPath, "Список стен.txt");

            File.WriteAllText(txtPath, wallInfo);

            TaskDialog.Show("Сообщение", "Файл создан на рабочем столе");
            return Result.Succeeded;
        }
    }
}
