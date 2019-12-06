using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(LevelBuilder))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilder level = (LevelBuilder)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Build Level"))
        {
            level.DestroyLevel();
            level.BuildLevel();
        }
            
        if (GUILayout.Button("Destroy Level"))
            level.DestroyLevel();
    }
}
