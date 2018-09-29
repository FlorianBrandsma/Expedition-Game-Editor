public class EditorPath
{
    public Path open    = new Path();
    public Path edit    = new Path();
    public Path get     = new Path();

    public EditorPath(Route route)
    {
        switch (route.data.table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(route);

                open    = chapter.Open();
                edit    = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(route);

                open    = phase.Open();
                edit    = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(route);

                open    = quest.Open();
                edit    = quest.Edit();

                break;

            case "Objective":

                PathManager.Structure objective = new PathManager.Structure(route);

                open    = objective.Open();
                edit    = objective.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(route);

                open    = region.Open();
                edit    = region.Edit();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(route);

                edit    = terrain.Edit();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(route);

                open    = item.Open();
                edit    = item.Edit();
                get     = item.Get();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(route);

                open    = element.Open();
                edit    = element.Edit();
                get     = element.Get();

                break;

            case "Option":

                PathManager.Option option = new PathManager.Option(route);

                open    = option.Open();

            break;

            case "Asset":

                PathManager.Asset asset = new PathManager.Asset(route);

                open    = asset.Open();

            break;

            default: break;
        }
    }
}
