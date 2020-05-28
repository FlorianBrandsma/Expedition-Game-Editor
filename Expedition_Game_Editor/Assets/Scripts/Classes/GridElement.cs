using UnityEngine;

public class GridElement
{ 
    public Vector2 startPosition;
    public Rect rect;

    public GridElement(Vector2 startPosition, Rect rect)
    {
        this.startPosition = startPosition;
        this.rect = rect;
    }
}
