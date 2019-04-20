using UnityEngine;
using System.Collections;

public class Data
{
    public IDataController controller { get; set; }
    public IEnumerable element { get; set; }

    public Data()
    {
        controller = null;
        element = new[] { new GeneralData() };
    }

    public Data(IEnumerable element)
    {
        controller = null;
        this.element = element;
    }

    public Data(IDataController controller, IEnumerable element)
    {
        this.controller = controller;
        this.element = element;
    }

    public Data Copy()
    {
        return new Data(null, element);
    }
}
