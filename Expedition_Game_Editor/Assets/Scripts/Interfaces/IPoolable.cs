using UnityEngine;

public interface IPoolable
{
    Transform Transform             { get; }
    Enums.ElementType ElementType   { get; }
    int PoolId                      { get; set; }
    bool IsActive                   { get; }
    IPoolable Instantiate();
    void ClosePoolable();
}
