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
        
        if (GUILayout.Button("Load From Specified FilePath"))
        {
            editor.LoadFromSpecifiedFilePath();
        }
        if (GUILayout.Button("Save To Specified FilePath"))
        {
            editor.SaveToSpecifiedFilePath();
        }
        if (GUILayout.Button("Save as New Easy Pattern"))
        {
            editor.SaveAsEasy();
        }
        if (GUILayout.Button("Save as New Medium Pattern"))
        {
            editor.SaveAsMedium();
        }
        if (GUILayout.Button("Save as New Hard Pattern"))
        {
            editor.SaveAsHard();
        }
    }
}
