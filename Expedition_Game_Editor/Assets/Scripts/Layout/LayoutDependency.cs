using UnityEngine;
using System.Collections;

public class LayoutDependency : MonoBehaviour
{
    public LayoutDependency layoutDependency;

    public void InitializeLayout()
    {
        if(!gameObject.activeInHierarchy)
        {
            if (GetComponent<EditorLayout>() != null)
                GetComponent<EditorLayout>().InitializeLayout();

            gameObject.SetActive(true);

            if (layoutDependency != null)
                layoutDependency.InitializeLayout();
        }
    }

    public void SetLayout()
    {
        if (GetComponent<LayoutManager>() != null)
            GetComponent<LayoutManager>().SetLayout();

        if (layoutDependency != null)
            layoutDependency.SetLayout();
    }

    public void CloseLayout()
    {
        gameObject.SetActive(false);

        if (GetComponent<LayoutManager>() != null)
            GetComponent<LayoutManager>().CloseLayout();

        if (layoutDependency != null)
            layoutDependency.CloseLayout();
    }
}
