using UnityEngine;
using System.Collections;

public class Enums
{
    public enum SortType
    {
        Panel,
        List,
        Grid,
    }

    public enum SelectionType
    {
        None,
        Select,
        Automatic,
    }

    public enum SelectionProperty
    {
        None,
        Get,
        Set,
        Edit,
        Open,
    }

    public enum ItemType
    {
        Supply,
        Arm,
        Spoil,
    }
}
