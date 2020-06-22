using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Route
{
    public class Data
    {
        public IDataController dataController;
        public IElementData elementData;
        public SearchProperties searchProperties;

        public List<IElementData> dataList;

        public Data()
        {
            elementData = new GeneralElementData();
        }
        
        public Data(Data data)
        {
            dataController = data.dataController;
            elementData = data.elementData;
            searchProperties = data.searchProperties;
            
            if(data.dataList != null)
            {
                //Shallow copy
                dataList = data.dataList;
            }
        }

        public Data(DataElement.Data data)
        {
            dataController = data.dataController;
            elementData = data.elementData;
            searchProperties = data.searchProperties;

            dataList = data.dataController.DataList;
        }

        public Data(IDataController dataController)
        {
            this.dataController = dataController;
            elementData = new GeneralElementData();
        }

        public Data(IDataController dataController, IElementData elementData)
        {
            this.dataController = dataController;
            this.elementData = elementData;
        }

        public Data(Data data, IElementData elementData)
        {
            dataController = data.dataController;
            this.elementData = elementData;

            dataList = data.dataList;
        }

        public Data(IDataController dataController, IElementData elementData, SearchProperties searchProperties)
        {
            this.dataController = dataController;
            this.elementData = elementData;
            this.searchProperties = searchProperties;
        }
    }
    
    public int controller;
    public Data data;
    public Path path;

    public Enums.SelectionStatus selectionStatus;

    public GeneralData GeneralData { get { return (GeneralData)data.elementData; } }

    public Route() { }

    public Route(Path path)
    {
        controller = 0;
        data = new Data(null, new GeneralElementData(), new SearchProperties(Enums.DataType.None));
        this.path = path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = new Data(route.data);

        selectionStatus = route.selectionStatus;

        path = route.path;
    }
    
    public Route(EditorElement editorElement)
    {
        data = new Data(editorElement.DataElement.data);
        path = editorElement.DataElement.Path;
        selectionStatus = editorElement.selectionStatus;
    }

    public Route(int controller, Data data, Enums.SelectionStatus selectionStatus)
    {
        this.controller = controller;
        this.data = data;

        this.selectionStatus = selectionStatus;
    }

    public bool Equals(Route route)
    {
        if (controller != route.controller)
            return false;

        if (!GeneralData.Equals(route.GeneralData))
            return false;

        return true;
    }

    public Route Copy()
    {
        return new Route(this);
    }
}
