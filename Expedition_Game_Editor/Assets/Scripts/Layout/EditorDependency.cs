using UnityEngine;
using System.Collections;

public class EditorDependency : MonoBehaviour
{
    public EditorDependency editorDependency;

    public void Activate()
    {
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);

            if (editorDependency != null)
                editorDependency.Activate();
        }
    }

    public void SetDependency()
    {
        if (GetComponent<LayoutManager>() != null)
            GetComponent<LayoutManager>().SetLayout();

        if (editorDependency != null)
            editorDependency.SetDependency();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);

        if (editorDependency != null)
            editorDependency.Deactivate();
        
    }

    public void CloseDependency()
    {
        if (GetComponent<LayoutManager>() != null)
            GetComponent<LayoutManager>().CloseLayout();

        if (editorDependency != null)
            editorDependency.CloseDependency();
    }
}
