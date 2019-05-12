using UnityEngine;
using System.Collections;

public class Data
{
    public IDataController DataController   { get; set; }
    public IEnumerable ElementData          { get; set; }
    public IEnumerable SearchParameters     { get; set; }

    public Data()
    {
        ElementData = new[] { new GeneralData() };
    }

    public Data(IEnumerable elementData)
    {
        ElementData = elementData;
    }

    public Data(IDataController controller)
    {
        DataController = controller;
        ElementData = new[] { new GeneralData() };
    }

    public Data(IDataController controller, IEnumerable element)
    {
        DataController = controller;
        ElementData = element;
    }

    public Data(IDataController controller, IEnumerable element, IEnumerable searchParameters)
    {
        DataController = controller;
        ElementData = element;
        SearchParameters = searchParameters;
    }

    public Data Copy()
    {
        return new Data(ElementData);
    }
}
