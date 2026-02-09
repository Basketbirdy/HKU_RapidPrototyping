using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerMovement script = (PlayerMovement)target;
        if(GUILayout.Button("Reset initial stats"))
        {
            script.ResetStats();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Print current stats"))
        {
            script.PrintStats();
        }
    }
}
