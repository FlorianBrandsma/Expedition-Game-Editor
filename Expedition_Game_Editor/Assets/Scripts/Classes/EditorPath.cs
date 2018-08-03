using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPath
{
    public Path select  = new Path(new WindowManager(), new List<int>(), new List<int>());
    public Path edit    = new Path(new WindowManager(), new List<int>(), new List<int>());
    public bool adaptive = false;

    public EditorPath(ElementData data)
    {
        switch (data.table)
        {
            case "Chapter":

                PathManager.Chapter chapter = new PathManager.Chapter(data);

                select  = chapter.Select();
                edit    = chapter.Edit();

                break;

            case "Phase":

                PathManager.Phase phase = new PathManager.Phase(data);

                select  = phase.Select();
                edit    = phase.Edit();

                break;

            case "Quest":

                PathManager.Quest quest = new PathManager.Quest(data);

                select  = quest.Select();
                edit    = quest.Edit();

                break;

            case "Objective":

                PathManager.Objective objective = new PathManager.Objective(data);

                select  = objective.Select();
                edit    = objective.Edit();

                break;

            case "Region":



                //edit = new Path(new List<int>() { 0, 1 }, new List<int>());
                break;
            case "Terrain":
                //select = new Path(new List<int>() { 0, 1, 0 }, new List<int>());
                break;


            case "Item":

                PathManager.Item item = new PathManager.Item(data);

                select  = item.Select();
                edit    = item.Select();

                break;

            case "Element":
                //select = new Path(new List<int>() { 0, 1, data.type }, new List<int>());
                //edit = new Path(new List<int>() { 0, 2, 1 }, new List<int>());
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
