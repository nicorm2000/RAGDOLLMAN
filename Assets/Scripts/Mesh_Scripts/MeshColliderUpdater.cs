using UnityEngine;

/// <summary>
/// This class is responsible for updating a MeshCollider based on changes to the underlying Mesh.
/// It provides a method to recalculate the bounds and normals of the mesh and assign it to the MeshCollider.
/// </summary>
public class MeshColliderUpdater : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    /// <summary>
    /// Recalculates the bounds and normals of the mesh and assigns the updated mesh to the MeshCollider component.
    /// </summary>
    public void RecalculateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        // Get the current mesh
        Mesh mesh = meshFilter.sharedMesh;

        // Recalculate the bounds and normals of the mesh
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Assign the updated mesh to the MeshCollider
        meshCollider.sharedMesh = mesh;
    }
}