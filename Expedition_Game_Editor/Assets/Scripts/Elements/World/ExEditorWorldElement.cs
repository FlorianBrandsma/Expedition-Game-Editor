﻿using UnityEngine;
using System.Linq;

public class ExEditorWorldElement : MonoBehaviour, IElement, IPoolable
{
    private int modelId;
    private string modelPath;
    private Model model;
    
    private Vector3 position;
    private Vector3 rotation;
    
    private float scale;

    public EditorElement EditorElement      { get { return GetComponent<EditorElement>(); } }

    public Color ElementColor
    {
        set
        {
            if (model == null) return;

            model.SetStatus(value);
        }
    }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.WorldElement; } }
    public int PoolId                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        CloseModel();

        switch (EditorElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.WorldInteractable:      SetWorldInteractableElement();      break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationElement(); break;
            case Enums.DataType.WorldObject:            SetWorldObjectElement();            break;
            case Enums.DataType.Phase:                  SetPhaseElement();                  break;
            case Enums.DataType.SceneActor:             SetSceneActorElement();             break;
            case Enums.DataType.SceneProp:              SetScenePropElement();              break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.ElementData.DataType); break;
        }
        
        //Removed elements do not have a path and should not be set
        if (EditorElement.elementStatus != Enums.ElementStatus.Hidden && modelPath != "")
        {
            var prefab = Resources.Load<Model>(modelPath);

            model = (Model)PoolManager.SpawnObject(prefab, modelId);

            SetModel();
        }

        transform.localPosition     = new Vector3(position.x, position.y, position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(scale, scale, scale);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;

        position = new Vector3(elementData.PositionX, elementData.PositionY, -elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;
    }

    private void SetInteractionDestinationElement()
    {
        var elementData = (InteractionDestinationElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;

        position = new Vector3(elementData.PositionX, elementData.PositionY, -elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;
    }

    private void SetWorldObjectElement()
    {
        var elementData = (WorldObjectElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;

        position = new Vector3(elementData.PositionX, elementData.PositionY, -elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;
    }

    private void SetPhaseElement()
    {
        var elementData = (PhaseElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;

        position = new Vector3(elementData.DefaultPositionX, elementData.DefaultPositionY, -elementData.DefaultPositionZ);
        rotation = new Vector3(elementData.DefaultRotationX, elementData.DefaultRotationY, elementData.DefaultRotationZ);

        scale = elementData.Scale;
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;

        position = new Vector3(elementData.PositionX, elementData.PositionY, -elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        if (elementData.FaceTarget)
        {
            var targetActor = (SceneActorElementData)EditorElement.DataElement.Data.dataList.Where(x => x.Id == elementData.TargetSceneActorId).FirstOrDefault();

            if(targetActor != null)
            {
                var targetPosition = new Vector3(targetActor.PositionX, targetActor.PositionY, -targetActor.PositionZ);

                var faceTargetRotation = Quaternion.LookRotation((targetPosition - position).normalized).eulerAngles;
                rotation = new Vector3(0, faceTargetRotation.y, 0);
            }
        }
        
        scale = elementData.Scale;
    }

    private void SetScenePropElement()
    {
        var elementData = (ScenePropElementData)EditorElement.DataElement.ElementData;

        modelId = elementData.ModelId;
        modelPath = elementData.ModelPath;
        
        position = new Vector3(elementData.PositionX, elementData.PositionY, -elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;
    }

    private void SetModel()
    {
        model.EditorElement = EditorElement;

        model.transform.SetParent(transform, false);

        model.gameObject.SetActive(true);
    }

    public void CloseElement()
    {
        CloseModel();

        CloseTrackingElements();
    }

    private void CloseModel()
    {
        if (model == null) return;

        PoolManager.ClosePoolObject(model);
        model = null;
    }

    public void CloseTrackingElements()
    {
        if (EditorElement.glow != null)
        {
            EditorElement.glow.SetActive(false);
            EditorElement.glow = null;
        }

        if (EditorElement.lockIcon != null)
        {
            EditorElement.lockIcon.SetActive(false);
            EditorElement.lockIcon = null;
        }
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
