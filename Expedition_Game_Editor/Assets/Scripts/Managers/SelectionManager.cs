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

    static public IElementData getElementData;

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

    static public void SelectData(List<IElementData> dataList, IDisplayManager displayManager = null)
    {
        if (dataList.Count == 0) return;

        foreach (IElementData elementData in dataList)
        {
            foreach (Route route in routeList)
            {
                if (route.id == 0) continue;
                
                if (DataManager.Equals(elementData, route.ElementData))
                {
                    if (elementData.SelectionStatus == Enums.SelectionStatus.None)
                    {
                        elementData.SelectionStatus = route.selectionStatus;

                    } else {

                        elementData.SelectionStatus = Enums.SelectionStatus.Both;
                    }

                    if (displayManager != null)
                        displayManager.CorrectPosition(elementData, displayManager.Display.DataController.Data.dataList);
                }
            }
        }
    }

    static public void SelectSearch(IElementData selectedElementData)
    {
        getElementData = selectedElementData;
    }

    static public void SelectSet(IElementData setElementData)
    {
        var elementDataList = SelectionElementManager.FindElementData(getElementData);

        //First set the result to all relevant elements
        elementDataList.ForEach(x => x.DataElement.SetResult(setElementData));

        //Cancelling selection will re-render the editor and visualize the selection
        CancelGetSelection();
    }
    
    static public void CancelGetSelection()
    {
        if (getElementData == null) return;

        CancelSelection(new List<IElementData>() { getElementData });

        getElementData = null;

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
