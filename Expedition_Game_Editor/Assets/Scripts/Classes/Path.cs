using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Path
{
    public List<int> route          { get; set; }
    public List<ElementData> data   { get; set; }
    public SectionManager section   { get; set; }
    public Selection origin         { get; set; }

    public Path()
    {
        route   = new List<int>();
        data    = new List<ElementData>();
        section = null;
        origin  = null;
    }

    public Path(List<int> new_route, List<ElementData> new_data, SectionManager new_section, Selection new_origin)
    {
        route   = new_route;
        data    = new_data;
        section = new_section;
        origin  = new_origin;
    }

    public void Clear()
    {
        route.Clear();
        data.Clear();
    }

    #region Equals

    public bool Equals(Path path)
    {
        if (!route.SequenceEqual(path.route))
            return false;

        return true;
    }

    public bool Equals(Path path, int step)
    {
        if (route[step] != path.route[step])
            return false;

        return data[step].Equals(path.data[step]);
    }

    #endregion

    #region Add

    public void Add()
    {
        Add(0, new ElementData());
    }

    public void Add(int index)
    {
        Add(index, new ElementData());
    }

    public void Add(int index, ElementData new_data)
    {
        route.Add(index);
        data.Add(new_data);
    }

    #endregion

    public Path Copy()
    {
        Path copy = new Path();

        for(int i = 0; i < route.Count; i++)
        {
            copy.route.Add(route[i]);
            copy.data.Add(data[i]);
        }

        return copy;
    }

    //Create a new path based on an editor list. ID list is generated
    public Path CreateEdit(List<int> base_route)
    {
        List<int> base_id = new List<int>();

        for (int i = 0; i < base_route.Count; i++)
            base_id.Add(0);

        return CreateEdit(base_route, base_id);
    }

    //Create a new path based on editor and id lists
    public Path CreateEdit(List<int> base_route, List<int> base_id)
    {
        Path path = new Path();

        path.section = section;

        for (int i = 0; i < base_route.Count; i++)
            path.Add(base_route[i]);
        
        return path;
    }

    //Add new indexes to an editor or id list
    public List<int> Add(List<int> path, List<int> new_index)
    {
        List<int> new_path = new List<int>();

        for (int i = 0; i < path.Count; i++)
            new_path.Add(path[i]);

        for (int i = 0; i < new_index.Count; i++)
            new_path.Add(new_index[i]);

        return new_path;
    }

    public Path Trim(int index)
    {
        Path new_path = new Path(new List<int>(), new List<ElementData>(), section, origin);

        for (int i = 0; i < index; i++)
            new_path.Add(route[i], data[i]);

        return new_path;
    }
}
