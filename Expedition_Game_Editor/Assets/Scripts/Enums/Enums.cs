using UnityEngine;
using System.Collections;

public class Enums
{
    public enum DisplayMode
    {
        List,
        Diagram,
    }

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
    };

    public enum SelectionProperty
    {
        None,
        Get,
        Set,
    }
}
