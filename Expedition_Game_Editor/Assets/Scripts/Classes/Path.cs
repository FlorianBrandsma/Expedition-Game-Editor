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

        copy.section = section;
        copy.origin = origin;

        return copy;
    }

    //SOON OBSELETE : ONLY USED BY MINI BUTTONS!
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

        for (int i = 0; i < base_route.Count; i++)
            path.Add(base_route[i]);

        path.section = section;
        path.origin = origin;

        return path;
    }

    public Path Trim(int index)
    {
        Path new_path = new Path(new List<int>(), new List<ElementData>(), section, origin);

        for (int i = 0; i < index; i++)
            new_path.Add(route[i], data[i]);

        return new_path;
    }
}
