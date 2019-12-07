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
            level.CleanLevel();
            level.BuildLevel();
        }
            
        if (GUILayout.Button("Clean Level"))
            level.CleanLevel();
    }
}
