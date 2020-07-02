﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

// Build and update a localized navmesh from the sources marked by NavMeshSourceTag
[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
    // The center of the build
    public Transform m_Tracked;
    public Vector3 center;

    // The size of the build bounds
    public Vector3 m_Size = new Vector3(80.0f, 80.0f, 20.0f);

    NavMeshData m_NavMesh;
    AsyncOperation m_Operation;
    NavMeshDataInstance m_Instance;
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

    //IEnumerator Start()
    //{
    //    while (true)
    //    {
    //        //Short delay to avoid excessive operations
    //        yield return new WaitForSeconds(2);

    //        UpdateNavMesh(true);
            
    //        yield return m_Operation;
    //    }
    //}

    void OnEnable()
    {
        // Construct and add navmesh
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);

        if (m_Tracked == null)
            m_Tracked = transform;

        UpdateNavMesh(false);
    }

    void OnDisable()
    {
        // Unload navmesh and clear handle
        m_Instance.Remove();
    }

    public void UpdateNavMesh(bool asyncUpdate = false)
    {
        //Don't do anything if the target area hasn't changed
        if (asyncUpdate && center == (m_Tracked ? m_Tracked.position : transform.position))
            return;

        NavMeshSourceTag.Collect(ref m_Sources);
        var defaultBuildSettings = NavMesh.GetSettingsByIndex(0);

        defaultBuildSettings.overrideVoxelSize = true;
        defaultBuildSettings.voxelSize = 0.4f;
        defaultBuildSettings.overrideTileSize = true;
        defaultBuildSettings.tileSize = 16;
        
        var bounds = QuantizedBounds();

        if (asyncUpdate)
            m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        // Quantize the bounds to update only when theres a 20% change in size
        center = m_Tracked ? new Vector3(m_Tracked.position.x, m_Tracked.position.y, m_Tracked.position.z + m_Size.z / 3) : transform.position;
        return new Bounds(Quantize(center, 0.2f * m_Size), m_Size);
    }

    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }
        
        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = m_Tracked ? m_Tracked.position : transform.position;
        Gizmos.DrawWireCube(center, m_Size);
    }
}
