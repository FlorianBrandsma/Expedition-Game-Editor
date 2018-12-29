using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour
{
    public enum Action
    {
        Nothing,
        Open,
        OpenOnce,
        Close,
    }

    private bool active;

    public EditorComponent component;

    public Action on_start;
    public Action on_close;

    public EditorForm editorForm;
    public ComponentManager componentManager;

    public Texture2D open_icon, close_icon;

    public Button button;

    public void SetComponent()
    {
        SetButton();

        if(on_start == Action.Open || on_start == Action.OpenOnce)
        {
            if (active) return;

            EditorManager.editorManager.InitializePath(new PathManager.Form(editorForm).Initialize());

            button.GetComponent<EditorButton>().icon.texture = open_icon;

            if (on_start == Action.OpenOnce)
                active = true;
        }     
    }

    private void SetButton()
    {
        button = componentManager.AddFormButton();

        //new_element.data = element.data;
        //new_element.selectionType = element.selectionType;
        //new_element.listType = element.listType;

        button.onClick.AddListener(delegate { Interact(); });
    }

    public void Interact()
    {
        if (editorForm.gameObject.activeInHierarchy)
            CloseForm();
        else
            OpenForm();    
    }

    public void OpenForm()
    {
        EditorManager.editorManager.OpenPath(new PathManager.Form(editorForm).Initialize());

        button.GetComponent<EditorButton>().icon.texture = open_icon;
    }

    private void CloseForm()
    {
        editorForm.CloseForm();
        editorForm.GetComponent<LayoutManager>().ResetLayout();

        editorForm.ResetSibling();

        button.GetComponent<EditorButton>().icon.texture = close_icon;
    }

    public void CloseComponent()
    {
        if(on_close == Action.Close)
            CloseForm();
    }
}
