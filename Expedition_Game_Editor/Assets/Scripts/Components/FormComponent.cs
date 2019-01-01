using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private bool locked;

    public bool auto_open;
    public bool auto_close;

    public bool open_once;

    public EditorForm editorForm;

    public Texture2D open_icon, close_icon;

    private Button button;

    public void SetComponent()
    {
        SetButton();

        if(auto_open)
        {
            if (locked) return;

            EditorManager.editorManager.InitializePath(new PathManager.Form(editorForm).Initialize());

            button.GetComponent<EditorButton>().icon.texture = open_icon;

            if (open_once)
                locked = true;
        }     
    }

    private void SetButton()
    {
        button = ComponentManager.componentManager.AddFormButton(component);

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
        editorForm.CloseForm(false);
        editorForm.GetComponent<LayoutManager>().ResetLayout();
        ComponentManager.componentManager.SortComponents();

        editorForm.ResetSibling();

        button.GetComponent<EditorButton>().icon.texture = close_icon;
    }

    public void CloseComponent()
    {
        if(auto_close)
            CloseForm();
    }
}
