using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertSkinToMesh : MonoBehaviour
{
    /// <summary>
    /// This method converts the skinned mesh to normal mesh.
    /// </summary>
    [ContextMenu("Convert to mesh")]
    private void ConvertToMesh()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
        meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;

        DestroyImmediate(skinnedMeshRenderer);
        DestroyImmediate(this);
    }

}
