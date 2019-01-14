using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private bool initialized;

    private bool locked;
    public bool manually_closed;

    public bool auto_open;
    public bool auto_close;

    public bool open_once;

    //Don't reset form when opening
    public bool constant;

    public EditorForm editorForm;

    public Texture2D open_icon, close_icon;

    private Button button;

    public void SetComponent(Path new_path)
    {
        SetButton();

        //If the component was manually closed, don't open it here
        if (constant && manually_closed) return;

        if(auto_open)
        {
            if (locked) return;
            
            if(!initialized)
            {
                EditorManager.editorManager.InitializePath(new PathManager.Form(editorForm).Initialize());

                initialized = true;
            } else {
                EditorManager.editorManager.InitializePath(editorForm.active_path);
            }

            button.GetComponent<EditorButton>().icon.texture = open_icon;

            if (open_once)
                locked = true;
        }

        manually_closed = false;
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
        {
            CloseForm();
            manually_closed = true;
        } else {
            OpenForm();
            manually_closed = false;
        }      
    }

    public void OpenForm()
    {
        Path path = (constant ? editorForm.active_path : new PathManager.Form(editorForm).Initialize());

        EditorManager.editorManager.OpenPath(path);

        button.GetComponent<EditorButton>().icon.texture = open_icon;
    }

    private void CloseForm()
    {
        editorForm.CloseForm(false);
        editorForm.GetComponent<LayoutManager>().ResetLayout();
        ComponentManager.componentManager.SortComponents();

        editorForm.ResetSibling();

        SelectionManager.SelectElements();

        button.GetComponent<EditorButton>().icon.texture = close_icon;
    }

    public void CloseComponent()
    {
        if(auto_close)
            CloseForm();
    }
}
