using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchParameters
{
    public Enums.DataType dataType;

    public int temp_id_count;

    public bool unique;
    public bool exact;
    
    public string value;

    public List<int> id     = new List<int>();
    public List<int> type   = new List<int>();
}
