using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Route
{
    public class Data
    {
        public IDataController dataController;
        public IDataElement dataElement;
        public IEnumerable searchParameters;

        public List<IDataElement> dataList;

        public Data()
        {
            dataElement = new GeneralDataElement();
        }
        
        public Data(Data data)
        {
            dataController = data.dataController;
            dataElement = data.dataElement;
            searchParameters = data.searchParameters;
            
            if(data.dataList != null)
            {
                //Shallow copy
                dataList = data.dataList;
            }
        }

        public Data(SelectionElement.Data selectionData)
        {
            dataController = selectionData.dataController;
            dataElement = selectionData.dataElement;
            searchParameters = selectionData.searchParameters;

            dataList = selectionData.dataController.DataList;
        }

        public Data(IDataController dataController)
        {
            this.dataController = dataController;
            dataElement = new GeneralDataElement();
        }

        public Data(IDataController dataController, IDataElement dataElement)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
        }

        public Data(Data data, IDataElement dataElement)
        {
            dataController = data.dataController;
            this.dataElement = dataElement;

            dataList = data.dataList;
        }

        public Data(IDataController dataController, IDataElement dataElement, IEnumerable searchParameters)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
            this.searchParameters = searchParameters;
        }
    }
    
    public int controller;
    public Data data;
    public Path path;

    public Enums.SelectionGroup selectionGroup;

    public Route() { }

    public Route(Path path)
    {
        controller = 0;
        data = new Data(null, new GeneralDataElement(), new[] { new SearchParameters() });
        this.path = path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = new Data(route.data);

        selectionGroup = route.selectionGroup;

        path = route.path;
    }

    public Route(SelectionElement selectionElement)
    {
        data = new Data(selectionElement.data);
        path = selectionElement.path;
        selectionGroup = selectionElement.selectionGroup;
    }

    public Route(int controller, Data data, Enums.SelectionGroup selectionGroup)
    {
        this.controller = controller;
        this.data = new Data(data);

        this.selectionGroup = selectionGroup;
    }

    public bool Equals(Route route)
    {
        if (controller != route.controller)
            return false;

        if (!GeneralData().Equals(route.GeneralData()))
            return false;

        return true;
    }

    public Route Copy()
    {
        return new Route(this);
    }

    public GeneralData GeneralData()
    {
        return (GeneralData)data.dataElement;
    }
}
