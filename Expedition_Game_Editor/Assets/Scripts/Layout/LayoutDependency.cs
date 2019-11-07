using UnityEngine;
using System.Collections;

public class LayoutDependency : MonoBehaviour
{
    public LayoutDependency layoutDependency;

    public void InitializeDependency()
    {
        if(!gameObject.activeInHierarchy)
        {
            if (GetComponent<EditorLayout>() != null)
                GetComponent<EditorLayout>().InitializeLayout();

            gameObject.SetActive(true);

            if (layoutDependency != null)
                layoutDependency.InitializeDependency();
        }
    }

    public void SetDependency()
    {
        if (GetComponent<LayoutContent>() != null)
            GetComponent<LayoutContent>().SetContent();

        if (layoutDependency != null)
            layoutDependency.SetDependency();
    }

    public void CloseDependency()
    {
        gameObject.SetActive(false);

        if (GetComponent<LayoutContent>() != null)
            GetComponent<LayoutContent>().CloseContent();

        if (GetComponent<LayoutSection>() != null)
            GetComponent<LayoutSection>().CloseSection();

        if (layoutDependency != null)
            layoutDependency.CloseDependency();
    }
}
