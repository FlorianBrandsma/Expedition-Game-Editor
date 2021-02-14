using UnityEngine;
using System.Collections.Generic;

static public class SelectionManager
{
    public enum Type
    {
        None,
        Select,
        Automatic,
    }

    public enum Property
    {
        None,
        Get,
        Set,
        Edit,
        Enter,
        Open,
        Toggle,
        OpenOutcomeEditor,
        LoadSave,
        OpenPhaseSaveRegion,
        OpenPhaseSaveRegionWorldInteractable,
        OpenOutcomeScenes,
        OpenSceneRegion
    }

    static public List<Route> routeList = new List<Route>();

    static public IElementData searchElementData;

    static public void GetRouteList()
    {
        routeList.Clear();

        foreach (EditorForm form in RenderManager.layoutManager.forms)
        {
            foreach (LayoutSection section in form.editorSections)
            {
                if (!section.Active) continue;

                //For each section, add the target controller's route to a list so
                //that it can be used for finding the elements that should be selected
                routeList.Add(section.TargetController.PathController.route);
            }
        }
    }

    static public void ResetSelection(List<IElementData> dataList)
    {
        CancelSelection(dataList);
        SelectData(dataList);
    }

    static public void SelectData(List<IElementData> dataList, IDisplayManager displayManager = null)
    {
        if (dataList.Count == 0) return;

        foreach (IElementData elementData in dataList)
        {
            foreach (Route route in routeList)
            {
                if (DataManager.Equals(elementData, route.ElementData))
                {
                    //Only select the element by which the route was opened if "unique selection" is true or when searching
                    //Note! Selection happens before element data is assigned a data element, limiting the use cases
                    if (route.uniqueSelection)
                    {
                        if (elementData.DataElement != route.ElementData.DataElement)
                            continue;
                    }

                    if (elementData.UniqueSelection != route.uniqueSelection)
                        continue;

                    if (elementData.SelectionStatus == Enums.SelectionStatus.None)
                    {
                        elementData.SelectionStatus = route.selectionStatus;

                    } else if (route.selectionStatus == Enums.SelectionStatus.Child) {

                        elementData.SelectionStatus = Enums.SelectionStatus.Both;
                    }
                    
                    if (elementData.DataElement != null)
                        ((EditorElement)elementData.DataElement.SelectionElement).SetOverlay();

                    if (displayManager != null)
                        displayManager.CorrectPosition(elementData, displayManager.Display.DataController.Data.dataList);
                }
            }
        }
    }

    static public void SelectSearch(IElementData selectedElementData)
    {
        searchElementData = selectedElementData;
    }

    static public void SelectSet(IElementData resultElementData)
    {
        var elementDataList = SelectionElementManager.FindElementData(searchElementData);

        var dataRequest = new DataRequest()
        {
            includeDependencies = true
        };

        //var validationElement = searchElementData.Clone();
        //validationElement.DataElement.Data.dataController.SetData(validationElement, resultElementData);

        //Validate changes once
        //ApplyChanges(dataRequest, validationElement, resultElementData);

        if (dataRequest.notificationList.Count > 0)
        {
            dataRequest.notificationList.ForEach(x => Debug.Log(x));

        } else {

            //Execute changes to all relevant elements
            elementDataList.ForEach(x => x.DataElement.SetSearchResult(dataRequest, resultElementData));
        }
        
        //Cancelling selection will re-render the editor and visualize the selection
        CancelGetSelection();
    }

    private static void ApplyChanges(DataRequest dataRequest, IElementData searchElementData, IElementData resultElementData)
    {
        if (resultElementData.Id > 0)
        {
            if (searchElementData.ExecuteType == Enums.ExecuteType.Add)
                searchElementData.Add(dataRequest);

            if (searchElementData.ExecuteType == Enums.ExecuteType.Update)
                searchElementData.Update(dataRequest);

        } else {

            searchElementData.Remove(dataRequest);
        }
    }

    static public void CancelGetSelection()
    {
        if (searchElementData == null) return;

        CancelSelection(new List<IElementData>() { searchElementData });

        searchElementData = null;

        //Return to previous path in form
        RenderManager.Render(RenderManager.layoutManager.forms[2].previousPath);  
    }

    static public void CancelSelection(List<IElementData> dataList)
    {
        dataList.ForEach(x => 
        {
            if(x.DataElement != null)
            {
                x.DataElement.SelectionElement.CancelSelection();

            } else {

                x.SelectionStatus = Enums.SelectionStatus.None;
            }               
        });
    }
}
