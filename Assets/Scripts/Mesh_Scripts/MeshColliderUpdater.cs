using UnityEngine;

public class MeshColliderUpdater : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

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