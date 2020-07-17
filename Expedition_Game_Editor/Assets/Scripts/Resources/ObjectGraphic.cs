using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class ObjectGraphic : MonoBehaviour, IPoolable
{
    public EditorElement EditorElement { get; set; }

    public GameObject model;
    public GameObject[] mesh;
    public ObjectProperties.Pivot pivot;

    public NavMeshObstacleShape obstacleShape;

    public Vector3 previewRotation;
    public Vector3 previewScale;

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.ObjectGraphic; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public Animator Animator { get { return GetComponent<Animator>(); } }

    public void Awake()
    {
        if(Animator != null)
            Animator.keepAnimatorControllerStateOnDisable = true;
    }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void SetStatus(Color value)
    {
        foreach (GameObject mesh in mesh)
        {
            var renderer = mesh.GetComponent<Renderer>();
            renderer.material.color = value;

            if (value.a == 0)
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            else
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}