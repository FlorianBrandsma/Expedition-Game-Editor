public class EditorPath
{
    public Path path    = new Path();

    public EditorPath(Route route, SelectionManager.Property property)
    {
        switch (route.data.table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(route);

                if (property == SelectionManager.Property.Open)
                    path = chapter.Open();

                if (property == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(route);

                if (property == SelectionManager.Property.Open)
                    path = phase.Open();

                if (property == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(route);

                if (property == SelectionManager.Property.Open)
                    path = quest.Open();

                if (property == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case "Step":

                PathManager.Structure step = new PathManager.Structure(route);

                if (property == SelectionManager.Property.Open)
                    path = step.Open();

                if (property == SelectionManager.Property.Edit)
                    path = step.Edit();

                break;

            case "Task":

                PathManager.Structure task = new PathManager.Structure(route);

                if (property == SelectionManager.Property.Open)
                    path = task.Open();

                if (property == SelectionManager.Property.Edit)
                    path = task.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(route);

                if (property == SelectionManager.Property.Open)
                    path = region.Open();

                if (property == SelectionManager.Property.Edit)
                    path = region.Edit();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(route);

                if (property == SelectionManager.Property.Open)
                    path = terrain.Open();

                if (property == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(route);

                if (property == SelectionManager.Property.Open)
                    path = item.Open();

                if (property == SelectionManager.Property.Edit)
                    path = item.Edit();

                if (property == SelectionManager.Property.Get)
                    path = item.Get();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(route);

                if (property == SelectionManager.Property.Open)
                    path = element.Open();

                if (property == SelectionManager.Property.Edit)
                    path = element.Edit();

                if (property == SelectionManager.Property.Get)
                    path = element.Get();

                break;

            case "Option":

                PathManager.Option option = new PathManager.Option(route);

                if (property == SelectionManager.Property.Open)
                    path = option.Open();

            break;

            default: break;
        }
    }
}
