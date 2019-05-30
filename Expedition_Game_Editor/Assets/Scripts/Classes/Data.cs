using UnityEngine;
using System.Collections;

public class Data
{
    public IDataController DataController   { get; set; }
    public IDataElement DataElement         { get; set; }
    public IEnumerable SearchParameters     { get; set; }

    public Data()
    {
        DataElement = new GeneralDataElement();
    }

    public Data(IDataElement elementData)
    {
        DataElement = elementData;
    }

    public Data(IDataController controller)
    {
        DataController = controller;
        DataElement = new GeneralDataElement();
    }

    public Data(IDataController controller, IDataElement element)
    {
        DataController = controller;
        DataElement = element;
    }

    public Data(IDataController controller, IDataElement element, IEnumerable searchParameters)
    {
        DataController = controller;
        DataElement = element;
        SearchParameters = searchParameters;
    }

    public Data Copy()
    {
        return new Data(DataElement);
    }
}
