using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPath
{
    public Path source  = new Path();
    public Path edit    = new Path();

    public EditorPath(ElementData data, Path path)
    {
        switch (data.table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(data, path);

                source  = chapter.Source();
                edit    = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(data, path);

                source  = phase.Source();
                edit    = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(data, path);

                source  = quest.Source();
                edit    = quest.Edit();

                break;

            case "Objective":

                PathManager.Structure objective = new PathManager.Structure(data, path);

                source  = objective.Source();
                edit    = objective.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(data);

                source = region.Source();
                edit = region.Edit();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(data, path);

                edit = terrain.Edit();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(data);

                source  = item.Source();
                edit    = item.Edit();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(data);

                source = element.Source();
                edit = element.Edit();

                break;

            case "Object":

                PathManager.Object obj = new PathManager.Object(data);

                source = obj.Source();

                break;

            case "Tile":

                PathManager.Tile tile = new PathManager.Tile(data);

                source = tile.Source();

                break;

            case "Sound":

                PathManager.Sound sound = new PathManager.Sound(data);

                source = sound.Source();

                break;

            default: break;
        }
    }

    

    /*
    if (pathManager.adaptive)
    {
        //1. Get current Path
        //2. Get new index
        //3. Add new index
        //4. Add new id

        Path this_path = GetComponent<IController>().GetPath();
        //Must be: path it's currently on (using depth)

        List<int> new_id_path = CombinePath(this_path.id, new List<int> { selected_id });

        if(pathManager.structure.editor.Count > 0)
            select_path = new Path(CombinePath(this_path.editor, pathManager.structure.editor), new_id_path);

        if(pathManager.edit.editor.Count > 0)
            edit_path = new Path(CombinePath(this_path.editor, pathManager.edit.editor), new_id_path);
    } else {
        //1. Clear existing path
        //2. Create new select path using select_index
        //3. Create new edit path using edit_index
        //4. Create new ID (fill with 0s to the length of the new path. replace last index with selected id)
        if (pathManager.structure.editor.Count > 0)
            select_path = CreatePath(pathManager.structure.editor, selected_id);

        if (pathManager.edit.editor.Count > 0)
            edit_path = CreatePath(pathManager.edit.editor, selected_id);
    }
    */
}
