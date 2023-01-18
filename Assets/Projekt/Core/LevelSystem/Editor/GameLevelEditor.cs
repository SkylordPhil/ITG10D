using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameLevel), true)]
public class GameLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as GameLevel;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.levelPath);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            
            var scenePathProperty = serializedObject.FindProperty("levelPath");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
