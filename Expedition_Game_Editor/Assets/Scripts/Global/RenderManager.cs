﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

static public class RenderManager
{
    static public LayoutManager layoutManager;
    static public RectTransform UI { get { return layoutManager.UI; } }

    static public HistoryManager historyManager = new HistoryManager();

    static public Enums.LoadType loadType;
    
    //private void Update()
    //{
    //    //Escape button shares a built in function of the dropdown that closes it
    //    if (GameObject.Find("Dropdown List") != null) return;

    //    if (Input.GetKeyUp(KeyCode.Escape))
    //        PreviousPath();
    //}

    static public void Render(Path path)
    {
        SelectionManager.CancelGetSelection();

        //Set up data along the path
        path.form.InitializePath(path);

        //Get target routes for selecting elements
        SelectionManager.GetRouteList();

        //Deload inactive forms
        DeloadForms();

        //Open visible elements along the path
        OpenView(path);

        //Opening view only needs to initialize editors, so lists can be set
        ResetView();

        //Performed at the end so it doesn't interfere with the current (de)activation process
        InitializeSecondaryPaths(path);
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

    static public void ResetView()
    {
        //Activate layers, set anchors based on initialized values
        layoutManager.layers.ForEach(x => x.SetLayout());

        layoutManager.forms.ForEach(x => x.CloseEditor());
        layoutManager.forms.ForEach(x => x.OpenEditor());
    }

    static public void CloseForms()
    {
        layoutManager.forms.ForEach(x => x.CloseForm());
    }

    static public void PreviousPath()
    {
        historyManager.PreviousPath();

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

        layoutManager.forms.Where(form => form != path.form && form.activeInPath).ToList().ForEach(form => Render(form.activePath));

        loadType = Enums.LoadType.Normal;
    }

    static public List<IDataElement> GetData(IDataController dataController, SearchProperties searchProperties)
    {
        var dataList = dataController.DataManager.GetDataElements(searchProperties);

        var pathController = dataController.SegmentController.editorController.PathController;

        var mainForm = pathController.layoutSection.editorForm;
        mainForm.activePath.ReplaceDataLists(pathController.step, dataController.DataType, dataList);

        return dataList;
    }

    static public string PathString(Path path)
    {
        string str = "route: ";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].controller + "/";

        str += "\n";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].GeneralData.DataType + "-" + path.route[i].GeneralData.Id + "/";

        return str;
    }
}


