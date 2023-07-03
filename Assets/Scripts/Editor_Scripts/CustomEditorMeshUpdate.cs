using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshColliderUpdater))]
public class CustomEditorMeshUpdate : Editor
{
    /// <summary>
    /// This method overrides the default OnInspectorGUI() method from Unity's Editor class.
    /// It displays the default inspector GUI elements and adds a button.
    /// When the button is clicked, it calls the RecalculateMesh() method of the MeshColliderUpdater component.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshColliderUpdater user = (MeshColliderUpdater)target;

        if (GUILayout.Button("Button"))
        {
            user.RecalculateMesh();
        }
    }
}