using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SearchElement : MonoBehaviour
{
    private IDataController dataController { get { return GetComponent<IDataController>(); } }

    public Route route = new Route();

    public SegmentController segmentController;
    public IEditor dataEditor { get; set; }

    public GameObject glow;

    public GameObject displayParent;

    public EditorTile Tile { get { return GetComponent<EditorTile>(); } }

    bool selected;

    public void InitializeElement(IEditor dataEditor)
    {
        this.dataEditor = dataEditor;

        route.data = new Data(dataController);
    }

    public void SetResult(IEnumerable dataElement)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        SetElement(dataElement);

        segmentController.GetComponent<ISegment>().SetSearchResult(this);
    }

    public void SetElement(IEnumerable dataElement)
    {
        route.data.element = dataElement;

        switch (dataController.DataType)
        {
            case Enums.DataType.ObjectGraphic: SetObjectGraphic(); break;
            default: break;
        }

        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().SetDisplay();
    }
    
    private void SetObjectGraphic()
    {
        route.data.controller.DataList = route.data.element.Cast<ObjectGraphicDataElement>().ToList();

        var objectGraphicDataElement = route.data.element.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        Tile.icon.texture = Resources.Load<Texture2D>(objectGraphicDataElement.icon);   
    }

    public void SelectElement()
    {
        if(selected)
        {
            SelectionManager.CancelGetSelection();
            return;
        }

        List<int> controllers = new List<int>() { 1 };

        SearchData searchData = new SearchData();
        //Overwriting IElement with searchData might cause problems?
        route.data = new Data(dataController, new[] { searchData });

        Path path = PathManager.CreatePath(PathManager.CreateRoutes(controllers, route), EditorManager.editorManager.forms[2], null);

        EditorManager.editorManager.InitializePath(path);
        EditorManager.editorManager.forms[2].formComponent.OpenFormExternally();

        SelectionManager.SelectSearch(this);

        selected = true;
        glow.SetActive(true);    
    }

    public void CancelSelection()
    {
        selected = false;
        glow.SetActive(false);
    }
}
