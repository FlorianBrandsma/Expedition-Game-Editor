using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager
{
    #region Paths

    #region Initializers

    #region Primary

    public class Primary
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(window, source);
        }
    }

    #endregion

    #region Secondary

    public class Secondary
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        List<int> source = new List<int>() { 0 };

        public Path Initialize()
        {
            return CreatePath(window, source);
        }
    }

    #endregion

    #region Tertiary

    public class Tertiary
    {
        WindowManager window = EditorManager.editorManager.windows[2];

        List<int> source = new List<int>() { };

        public Path Initialize()
        {
            return CreatePath(window, source);
        }
    }

    #endregion

    #endregion

    #region Structure
    public class Structure
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> source = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Structure(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Source()
        {
            return new Path(window, CombinePath(data.path.structure, source), id_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }
    #endregion

    #region Item

    public class Item
    {
        ElementData data;

        List<int> source;
        List<int> edit;

        public Item(ElementData new_data)
        {
            data = new_data;

            source    = new List<int>() { 0, 0, data.type };
            edit      = new List<int>() { 0, 2, 0 };
        }

        public Path Source()
        {
            WindowManager window = EditorManager.editorManager.windows[1];
            return CreatePath(window, source, data.id);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data.id);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        ElementData data;

        List<int> source;
        List<int> edit;

        public Element(ElementData new_data)
        {
            data = new_data;

            source  = new List<int>() { 0, 1, data.type };
            edit    = new List<int>() { 0, 2, 1 };
        }

        public Path Source()
        {
            WindowManager window = EditorManager.editorManager.windows[1];
            return CreatePath(window, source, data.id);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data.id);
        }
    }

    #endregion

    #region Sound

    public class Sound
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        ElementData data;

        List<int> source = new List<int>();

        public Sound(ElementData new_data)
        {
            data = new_data;

            List<int> source = new List<int>() { 0, 0, data.type };
        }

        public Path Source()
        {
            return CreatePath(window, source, data.id);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        ElementData data;

        List<int> source    = new List<int>() { 0, 2 };
        List<int> edit      = new List<int>() { 0, 1 };

        public Region(ElementData new_data)
        {
            data = new_data;
        }

        public Path Source()
        {
            WindowManager window = EditorManager.editorManager.windows[1];
            return CreatePath(window, source, data.id);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data.id);
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> edit = new List<int>() { 0 };

        List<int> id_list = new List<int>();
        ElementData data;

        public Terrain(ElementData new_data)
        {
            data = new_data;
            id_list = CombinePath(data.path.id, new List<int>() { data.id });
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), id_list);
        }
    }

    #endregion

    #endregion
    static public Path CreatePath(WindowManager window, List<int> new_editor)
    {
        return CreatePath(window, new_editor, 0);
    }

    static public Path CreatePath(WindowManager window, List<int> new_editor, int new_id)
    {
        Path new_path = new Path(window, new List<int>(), new List<int>());

        for (int i = 0; i < new_editor.Count; i++)
        {
            new_path.structure.Add(new_editor[i]);
            new_path.id.Add(0);
        }

        if (new_path.id.Count > 0)
            new_path.id[new_path.id.Count - 1] = new_id;

        EditorManager.PathString(new_path);

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

    static public Path ReloadPath(Path path, int id)
    {
        Path new_path = new Path(path.window, new List<int>(), new List<int>());

        for(int i = 0; i < path.structure.Count; i++)
        {
            new_path.structure.Add(path.structure[i]);
            new_path.id.Add(path.id[i]);
        }

        if (new_path.id.Count > 0)
            new_path.id[new_path.id.Count - 1] = id;

        return new_path;
    }
}