﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private bool open;
    private bool initialized;
    private bool locked;
    private bool closedManually;

    private PathController pathController;

    //Don't reset form when opening
    public bool constant;
    public bool autoOpen;
    public bool autoClose;
    public bool openOnce;

    public EditorForm editorForm;

    public Texture2D openIcon;
    public Texture2D closeIcon;

    private Button button;

    EditorButton Button { get { return button.GetComponent<EditorButton>(); } }

    public void InitializeComponent(Path path) {  }

    public void SetComponent(Path path) { InitializeButton(); }

    public void InitializeForm(PathController pathController)
    {
        this.pathController = pathController;
    }

    public void SetForm()
    {
        editorForm.formComponent = this;

        //If the component was manually closed, don't open it here
        if (constant && closedManually) return;

        if (autoOpen)
        {
            if (locked) return;

            if (!initialized)
            {
                EditorManager.editorManager.InitializePath(new PathManager.Form(editorForm).Initialize());

                initialized = true;
            } else {

                //Set to true so the list will reset when selection is closed
                EditorManager.editorManager.InitializePath(editorForm.activePath, true);
            }

            if (openOnce)
                locked = true;
        }

        closedManually = false;

        SetIcon(true);
    }

    private void InitializeButton()
    {
        button = ComponentManager.componentManager.AddFormButton(component);

        button.onClick.AddListener(delegate { Interact(); });
    }

    public void Interact()
    {
        if (editorForm.gameObject.activeInHierarchy)
        {
            CloseForm();
            closedManually = true;
        } else {
            OpenForm();
            closedManually = false;
        }      
    }

    public void OpenForm()
    {
        Path path = (constant ? editorForm.activePath : new PathManager.Form(editorForm).Initialize());

        EditorManager.editorManager.InitializePath(path); 
    }

    private void CloseForm()
    {
        SelectionManager.CancelGetSelection();

        editorForm.CloseForm();
    }

    public void SetIcon(bool active)
    {
        button.GetComponent<EditorButton>().icon.texture = active ? openIcon : closeIcon;
    }

    public void CloseComponent()
    {
        if(autoClose)
            CloseForm();
    }
}
