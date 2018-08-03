using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager
{
    #region Paths
    #region Structure
    public class Structure
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> select = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Structure(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Select()
        {
            return new Path(window, CombinePath(data.path.structure, select), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Chapter
    public class Chapter
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> select = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Chapter(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Select()
        {
            return new Path(window, CombinePath(data.path.structure, select), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Phase
    public class Phase
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> select = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Phase(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Select()
        {
            return new Path(window, CombinePath(data.path.structure, select), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Quest

    public class Quest
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> select = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Quest(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Select()
        {
            return new Path(window, CombinePath(data.path.structure, select), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Objective
    public class Objective
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> select = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Objective(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Select()
        {
            return new Path(window, CombinePath(data.path.structure, select), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Region
    #endregion

    #region Terrain
    #endregion

    #region Item

    public class Item
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        ElementData data;

        List<int> select = new List<int>();
        List<int> edit = new List<int>();

        public Item(ElementData new_data)
        {
            data = new_data;

            List<int> select    = new List<int>() { 0, 0, data.type };
            List<int> edit      = new List<int>() { 0, 2, 0 };
        }

        public Path Select()
        {
            return CreatePath(window, select, data.id);
        }

        public Path Edit()
        {
            return CreatePath(window, edit, data.id);
        }
    }

    #endregion

    #region Element
    #endregion
    #endregion

    static public Path CreatePath(WindowManager window, List<int> new_editor, int new_id)
    {
        Path new_path = new Path(window, new List<int>(), new List<int>());

        for (int i = 0; i < new_editor.Count; i++)
        {
            new_path.structure.Add(new_editor[i]);

            if (i < new_editor.Count - 1)
                new_path.id.Add(0);
        }

        if (new_path.id.Count > 0)
            new_path.id[new_path.id.Count - 1] = new_id;

        return new_path;
    }

    static public List<int> CombinePath(List<int> path, List<int> index)
    {
        List<int> new_path = new List<int>();

        for (int i = 0; i < path.Count; i++)
            new_path.Add(path[i]);

        for (int i = 0; i < index.Count; i++)
        {
            //Add
            //if (relative_index)
            new_path.Add(index[i]);
            /*
            else
                new_path[new_path.Count - (i + 1)] = new_index[i];
            */
            //Merge
        }

        return new_path;
    }
}





