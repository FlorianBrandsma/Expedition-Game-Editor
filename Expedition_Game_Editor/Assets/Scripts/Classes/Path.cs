using System.Collections.Generic;
using System.Linq;

public class Path
{
    public WindowManager window { get; set; }
    public List<int> structure  { get; set; }
    public List<ElementData> data { get; set; }

    public Path(WindowManager new_window, List<int> new_structure, List<ElementData> new_data)
    {
        window = new_window;
        structure = new_structure;
        data = new_data; 
    }

    public void Clear()
    {
        structure.Clear();
        data.Clear();
    }

    public bool Equals(Path path)
    {
        if (!structure.SequenceEqual(path.structure))
            return false;
        /*
        if (!id.SequenceEqual(path.id))
            return false;
        */
        return true;
    }

    //Create a new path based on an editor list. ID list is generated
    public Path CreateEdit(List<int> base_editor)
    {
        List<int> base_id = new List<int>();

        for (int i = 0; i < base_editor.Count; i++)
            base_id.Add(0);

        return CreateEdit(base_editor, base_id);
    }

    //Create a new path based on editor and id lists
    public Path CreateEdit(List<int> base_editor, List<int> base_id)
    {
        Path path = new Path(null, new List<int>(), new List<ElementData>());

        for (int i = 0; i < base_editor.Count; i++)
        {
            path.structure.Add(base_editor[i]);
            path.data.Add(new ElementData()); //New controller
        }

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
        Path new_path = new Path(window, new List<int>(), new List<ElementData>());

        for (int i = 0; i < index; i++)
        {
            new_path.structure.Add(structure[i]);
            new_path.data.Add(data[i]);
        }

        return new_path;
    }
}
