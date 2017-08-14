using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PatternEditor))]
public class PatternEditorControl : Editor {

	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PatternEditor editor = (PatternEditor)target;
        if (GUILayout.Button("Save Pattern"))
        {
            editor.Save();
        }
    }
}
