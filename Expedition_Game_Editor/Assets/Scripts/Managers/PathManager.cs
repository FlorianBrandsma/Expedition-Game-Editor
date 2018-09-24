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
        SectionManager section = EditorManager.editorManager.sections[0];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(source, section);
        }
    }

    #endregion

    #region Secondary

    public class Secondary
    {
        SectionManager section = EditorManager.editorManager.sections[1];

        List<int> source = new List<int>() { 0 };

        public Path Initialize()
        {
            return CreatePath(source, section);
        }
    }

    #endregion

    #region Tertiary

    public class Tertiary
    {
        SectionManager section = EditorManager.editorManager.sections[2];

        List<int> open = new List<int>() { };

        public Path Initialize()
        {
            return CreatePath(open, section);
        }
    }

    #endregion

    #endregion

    #region Editor

    #region Structure
    public class Structure
    {
        List<int> open = new List<int>() { 0 };
        List<int> edit = new List<int>() { 1 };

        Path path = new Path();
        List<ElementData> data_list = new List<ElementData>();

        SectionManager section  = EditorManager.editorManager.sections[0];
        Selection origin;

        public Structure(ElementData new_data, Path new_path, Selection new_origin)
        {
            path = new_path;

            data_list = CombineData(new_path.data, new List<ElementData>() { new_data });

            origin = new_origin.Copy();
        }

        public Path Open()
        {
            return new Path(CombinePath(path.route, open), data_list, section, origin);
        }

        public Path Edit()
        {
            return new Path(CombinePath(path.route, edit), data_list, section, origin);
        }
    }
    #endregion

    #region Item

    public class Item
    { 
        List<int> open;
        List<int> edit;

        ElementData data;

        Selection origin;

        public Item(ElementData new_data, Selection new_origin)
        {
            data = new_data;

            open    = new List<int>() { 0, 0, data.type };
            edit    = new List<int>() { 0, 3, 0, data.type };

            origin = new_origin;
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(open, data, section, origin);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];

            return CreatePath(edit, data, section, origin);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        List<int> open;
        List<int> edit;

        ElementData data;

        Selection origin;

        public Element(ElementData new_data, Selection new_origin)
        {
            data = new_data;

            open    = new List<int>() { 0, 1, data.type };
            edit    = new List<int>() { 0, 3, 1, data.type };

            origin = new_origin;
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(open, data, section, origin);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(edit, data, section, origin);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        List<int> open  = new List<int>() { 0, 2 };
        List<int> edit  = new List<int>() { 0, 1, 0 };

        ElementData data;

        Selection origin;

        public Region(ElementData new_data, Selection new_origin)
        {
            data = new_data;

            origin = new_origin;
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(open, data, section, origin);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(edit, data, section, origin);
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {  
        List<int> edit = new List<int>() { 0 };

        Path path = new Path();
        List<ElementData> data_list = new List<ElementData>();

        SectionManager section = EditorManager.editorManager.sections[0];
        Selection origin;

        public Terrain(ElementData new_data, Path new_path, Selection new_origin)
        {
            path = new_path;

            ElementData data = new_data;
            data_list = CombineData(path.data, new List<ElementData>() { data });

            origin = new_origin;
        }

        public Path Edit()
        {
            return new Path(CombinePath(path.route, edit), data_list, section, origin);
        }
    }

    #endregion

    #endregion

    #region Assets

    #region Object

    public class Object
    {
        List<int> source = new List<int>() { 1, 0 };

        ElementData data;

        SectionManager section = EditorManager.editorManager.sections[1];
        Selection origin;

        public Object(ElementData new_data, Selection new_origin)
        {
            data = new_data;

            origin = new_origin;
        }

        public Path Source()
        {
            return CreatePath(source, data, section, origin);
        }
    }

    #endregion

    #region Tile

    public class Tile
    {
        List<int> source = new List<int>() { 1, 1 };

        ElementData data;

        SectionManager section = EditorManager.editorManager.sections[1];
        Selection origin;

        public Tile(ElementData new_data, Selection new_origin)
        {
            data = new_data;

            origin = new_origin;
        }

        public Path Source()
        {
            return CreatePath(source, data, section, origin);
        }
    }

    #endregion

    #endregion

    #endregion
    static public Path CreatePath(List<int> new_editor, SectionManager new_section)
    {
        return CreatePath(new_editor, new ElementData(), new_section, new Selection());
    }

    static public Path CreatePath(List<int> new_editor, ElementData new_data, SectionManager new_section, Selection new_origin)
    {
        Path new_path = new Path(new List<int>(), new List<ElementData>(), new_section, new_origin);

        for (int i = 0; i < new_editor.Count; i++)
        {
            new_path.Add(new_editor[i]);
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
        Path new_path = new Path(new List<int>(), new List<ElementData>(), path.section, path.origin.Copy());

        for(int i = 0; i < path.route.Count; i++)
        {
            new_path.route.Add(path.route[i]);
            new_path.data.Add(path.data[i]);
        }

        if (new_path.data.Count > 0)
            new_path.data[new_path.data.Count - 1] = data;

        return new_path;
    }
}