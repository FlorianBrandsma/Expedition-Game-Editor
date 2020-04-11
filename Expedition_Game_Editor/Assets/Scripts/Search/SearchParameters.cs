using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchParameters
{
    public Enums.DataType dataType;
    public Enums.ElementType elementType;
    public Enums.IconType iconType;
    public bool autoUpdate;

    public List<int> id = new List<int>();
}
