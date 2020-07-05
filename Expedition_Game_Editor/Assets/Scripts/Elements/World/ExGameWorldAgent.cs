using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public class ExGameWorldAgent : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;

    private float scaleMultiplier;

    public NavMeshAgent NavMeshAgent        { get { return GetComponent<NavMeshAgent>(); } }
    public GameElement GameElement          { get { return GetComponent<GameElement>(); } }

    public bool allowMoving;

    private bool Moving                     { get { return NavMeshAgent.velocity.x > 0.1f || NavMeshAgent.velocity.z > 0.1f; } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldAgent; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.WorldInteractable: SetAgentDestination(); break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }

        NavMeshAgent.destination = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
    }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.WorldInteractable: SetWorldInteractableAgent(); break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }

        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
    }

    private void SetWorldInteractableAgent()
    {
        var elementData = (WorldInteractableElementData)GameElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        var interactionData = elementData.interactionDataList.Where(x => x.containsActiveTime).First();

        position = new Vector3(interactionData.PositionX, interactionData.PositionY, interactionData.PositionZ);
        rotation = new Vector3(interactionData.RotationX, interactionData.RotationY, interactionData.RotationZ);

        scaleMultiplier = interactionData.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetAgentDestination()
    {
        var elementData = (WorldInteractableElementData)GameElement.DataElement.data.elementData;

        var interactionData = elementData.interactionDataList.Where(x => x.containsActiveTime).First();

        position = new Vector3(interactionData.PositionX, interactionData.PositionY, interactionData.PositionZ);
    }

    private void SetObjectGraphic()
    {
        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!NavMeshAgent.isOnNavMesh) return;

        if (NavMeshAgent.remainingDistance > 0 && !Moving)
            allowMoving = true;

        //Settle the agent in place when it is close to the destination but stopped moving (due to potential agent clashing)
        if(allowMoving && NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance && !Moving)
            SettleAgent();
    }

    private void SettleAgent()
    {
        NavMeshAgent.destination = transform.localPosition;

        StopAllCoroutines();
        StartCoroutine(Rotate(rotation.y));

        allowMoving = false;
    }

    IEnumerator Rotate(float angle)
    {
        var rotateSpeed = 10;

        while(Mathf.Abs(transform.rotation.eulerAngles.y - angle) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void CloseElement()
    {
        StopAllCoroutines();

        allowMoving = false;
        
        GameElement.DataElement.data.elementData.DataElement = null;
        GameElement.DataElement.data.elementData = null;

        if (objectGraphic == null) return;

        PoolManager.ClosePoolObject(objectGraphic);
        objectGraphic = null;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
