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

    #region Editor

    #region Structure
    public class Structure
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> source    = new List<int>() { 0 };
        List<int> edit      = new List<int>() { 1 };

        List<ElementData> data_list = new List<ElementData>();
        ElementData data;

        public Structure(ElementData new_data)
        {
            data = new_data;
            data_list = CombineData(data.path.data, new List<ElementData>() { data });
        }

        public Path Source()
        {
            return new Path(window, CombinePath(data.path.structure, source), data_list);
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), data_list);
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
            edit      = new List<int>() { 0, 2, 0, data.type };
        }

        public Path Source()
        {
            WindowManager window = EditorManager.editorManager.windows[1];
            return CreatePath(window, source, data);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data);
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
            edit    = new List<int>() { 0, 2, 1, data.type };
        }

        public Path Source()
        {
            WindowManager window = EditorManager.editorManager.windows[1];
            return CreatePath(window, source, data);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data);
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
            return CreatePath(window, source, data);
        }

        public Path Edit()
        {
            WindowManager window = EditorManager.editorManager.windows[0];
            return CreatePath(window, edit, data);
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        WindowManager window = EditorManager.editorManager.windows[0];

        List<int> edit = new List<int>() { 0 };

        List<ElementData> data_list = new List<ElementData>();
        ElementData data;

        public Terrain(ElementData new_data)
        {
            data = new_data;
            data_list = CombineData(data.path.data, new List<ElementData>() { data });
        }

        public Path Edit()
        {
            return new Path(window, CombinePath(data.path.structure, edit), data_list);
        }
    }

    #endregion

    #endregion

    #region Source

    #region Object

    public class Object
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        ElementData data;

        List<int> source = new List<int>() { 1, 0 };

        public Object(ElementData new_data)
        {
            data = new_data;
        }

        public Path Source()
        {
            return CreatePath(window, source, data);
        }
    }

    #endregion

    #region Tile

    public class Tile
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        ElementData data;

        List<int> source = new List<int>() { 1, 1 };

        public Tile(ElementData new_data)
        {
            data = new_data;
        }

        public Path Source()
        {
            return CreatePath(window, source, data);
        }
    }

    #endregion

    #region Sound

    public class Sound
    {
        WindowManager window = EditorManager.editorManager.windows[1];

        ElementData data;

        List<int> source;

        public Sound(ElementData new_data)
        {
            data = new_data;

            source = new List<int>() { 1, 2, data.type };
        }

        public Path Source()
        {
            return CreatePath(window, source, data);
        }
    }

    #endregion

    #endregion

    #endregion
    static public Path CreatePath(WindowManager window, List<int> new_editor)
    {
        return CreatePath(window, new_editor, new ElementData());
    }

    static public Path CreatePath(WindowManager window, List<int> new_editor, ElementData new_data)
    {
        Path new_path = new Path(window, new List<int>(), new List<ElementData>());

        for (int i = 0; i < new_editor.Count; i++)
        {
            new_path.structure.Add(new_editor[i]);
            new_path.data.Add(new ElementData());
        }

        if (new_path.data.Count > 0)
            new_path.data[new_path.data.Count - 1] = new_data;

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

    static public List<ElementData> CombineData(List<ElementData> data, List<ElementData> new_data)
    {
        List<ElementData> result = new List<ElementData>();

        for (int i = 0; i < data.Count; i++)
            result.Add(data[i]);

        for (int i = 0; i < new_data.Count; i++)
        {
            //Add
            //if (relative_index)
            result.Add(new_data[i]);
            /*
            else
                new_path[new_path.Count - (i + 1)] = new_index[i];
            */
            //Merge
        }

        return result;
    }

    static public Path ReloadPath(Path path, ElementData data)
    {
        Path new_path = new Path(path.window, new List<int>(), new List<ElementData>());

        for(int i = 0; i < path.structure.Count; i++)
        {
            new_path.structure.Add(path.structure[i]);
            new_path.data.Add(path.data[i]);
        }

        if (new_path.data.Count > 0)
            new_path.data[new_path.data.Count - 1] = data;

        return new_path;
    }
}