using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    [HideInInspector]
    public bool activeInPath;

    private bool closedManually;

    public bool autoOpen;

    public EditorForm editorForm;

    public Texture2D openIcon;
    public Texture2D closeIcon;

    public void InitializeComponent(Path path)
    {
        editorForm.formComponent = this;

        activeInPath = true;
    }

    public void SetComponent(Path path)
    {
        InitializeButton();
    }

    public void SetForm()
    {
        if (closedManually) return;

        if (autoOpen)
        {
            if (!editorForm.activeInPath)
                EditorManager.editorManager.InitializePath(new PathManager.Form(editorForm).Initialize());
        }
        
        closedManually = false;
    }

    private void InitializeButton()
    {
        var button = ComponentManager.componentManager.AddFormButton(component);

        button.GetComponent<EditorButton>().icon.texture = editorForm.activeInPath ? openIcon : closeIcon;

        button.onClick.AddListener(delegate { Interact(); });
    }

    public void Interact()
    {
        if (editorForm.gameObject.activeInHierarchy)
        {
            closedManually = true;
            editorForm.CloseForm();
        } else {

            closedManually = false;
            OpenForm();
        }      
    }

    public void OpenForm()
    {
        Path path = (new PathManager.Form(editorForm).Initialize());

        EditorManager.editorManager.InitializePath(path);
    }

    public void CloseComponent()
    {
        activeInPath = false;
    }
}
