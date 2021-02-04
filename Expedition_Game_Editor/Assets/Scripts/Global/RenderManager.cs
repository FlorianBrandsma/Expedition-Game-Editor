using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

static public class RenderManager
{
    static public LayoutManager layoutManager;
    static public RectTransform UI { get { return layoutManager.UI; } }

    static public Enums.LoadType loadType;
    
    static public void Render(Path path)
    {
        var mainPath = path.Clone();

        Debug.Log(PathString(mainPath));
        
        SelectionManager.CancelGetSelection();

        //Set up data along the path
        mainPath.form.InitializePath(mainPath);

        //Get target routes for selecting elements
        SelectionManager.GetRouteList();

        //Deload inactive forms
        DeloadForms();

        //Open visible elements along the path
        OpenView(mainPath);

        //Opening view only needs to initialize editors, so lists can be set
        ResetLayer(mainPath);
        
        //Performed at the end so it doesn't interfere with the current (de)activation process
        InitializeSecondaryPaths(mainPath);
        
        HistoryManager.AddHistory(mainPath);
    }

    static private void InitializeSecondaryPaths(Path path)
    {
        //Follows path, activates form actions and adds last route to history
        path.form.FinalizePath();

        //Set forms based on their action's state
        SetForms();
    }

    static private void DeloadForms()
    {
        layoutManager.forms.ForEach(x => x.loaded = x.activeInPath);
    }

    static private void SetForms()
    {
        layoutManager.forms.Where(x => x.formAction != null).Select(x => x.formAction).ToList().ForEach(formComponent =>
        {
            if (formComponent.activeInPath)
                formComponent.SetForm();
            else
                formComponent.editorForm.CloseForm();
        });
    }

    static private void OpenView(Path path)
    {
        //Initialize the layout of this form
        path.form.OpenView();
    }

    static public void ResetLayer(Path path)
    {
        //Get layer which the rendered path's form belongs to, including sub layers
        var layers = new List<EditorLayer>() { path.form.editorLayer };
        layers.AddRange(path.form.editorLayer.subLayers);

        //Get forms which belong to the layer
        var forms = layoutManager.forms.Where(x => layers.Contains(x.editorLayer)).ToList();

        //Activate layers, set anchors based on initialized values
        layoutManager.layers.ForEach(x => x.SetLayout());

        //Close segments and actions
        forms.ForEach(x => x.CloseSegments());

        //Open section editors and actions
        forms.ForEach(x => x.OpenSegments());

        //Reset active editors to apply edited values to data which might have been changed when the same data controller is re-opened
        layoutManager.forms.ForEach(x => x.ResetEditor());
    }

    static public void CloseForms()
    {
        layoutManager.forms.ForEach(x => x.CloseForm());
    }

    static public void PreviousPath()
    {
        HistoryManager.PreviousPath();

        loadType = Enums.LoadType.Normal;
    }

    static public void ResetPath(bool reload)
    {
        if (reload)
            loadType = Enums.LoadType.Reload;

        foreach (EditorForm form in layoutManager.forms)
            form.ResetPath();

        loadType = Enums.LoadType.Normal;
    }

    static public void ResetPath(Path path)
    {
        Render(path);

        //layoutManager.forms.Where(form => form != path.form && form.activeInPath).ToList().ForEach(form => Render(form.activePath));

        loadType = Enums.LoadType.Normal;
    }
    
    static public string PathString(Path path)
    {
        string str = "route: ";

        for (int i = 0; i < path.routeList.Count; i++)
            str += path.routeList[i].controllerIndex + "/";

        str += "\n";

        for (int i = 0; i < path.routeList.Count; i++)
        {
            if (path.routeList[i].data != null)
                str += path.routeList[i].data.dataController.DataType + ":" + path.routeList[i].id + "/";
            else
                str += "Null:0/";
        }

        return str;
    }
}



