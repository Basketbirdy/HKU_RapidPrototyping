using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Reset initial stats"))
        {
            PlayerMovement script = (PlayerMovement)target;
            script.ResetStats();
        }
    }
}
