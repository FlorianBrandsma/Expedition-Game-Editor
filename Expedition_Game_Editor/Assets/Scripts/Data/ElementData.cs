using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ElementData 
{
    public string table;
    public int id;
    public int type;
    public Path path = new Path(new List<int>(), new List<int>());
}
