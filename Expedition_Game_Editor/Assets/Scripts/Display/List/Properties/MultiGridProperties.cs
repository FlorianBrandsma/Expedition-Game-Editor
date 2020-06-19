using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class MultiGridProperties : MonoBehaviour, IProperties
{
    public Enums.ElementType elementType;

    public Enums.ElementType innerElementType;
    public SelectionManager.Type innerSelectionType;
    public SelectionManager.Property innerSelectionProperty;

    [HideInInspector]
    public Vector2 elementSize;

    public float margin;

    public IDataController[] DataControllers { get { return GetComponents<IDataController>(); } }

    public IDataController PrimaryDataController
    {
        get
        {
            if (DataControllers.Length > 0)
                return DataControllers[0];
            else
                return null;
        }
    }

    public IDataController SecondaryDataController
    {
        get
        {
            if (DataControllers.Length > 1)
                return DataControllers[1];
            else
                return null;
        }
    }

    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.MultiGrid;
    }
    #endregion
}
