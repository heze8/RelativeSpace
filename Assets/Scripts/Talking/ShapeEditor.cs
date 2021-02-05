using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Shape))]
public class ShapeEditor : Editor {

    Shape shape;

    void OnEnable(){
        shape = target as Shape;
    }

    public override void OnInspectorGUI()
    {
        // Script.X = EditorGUILayout.IntField(3);
        // Script.Y = EditorGUILayout.IntField(3);
        EditorGUILayout.LabelField( "Shape: ",   "shape of idea");
        EditorGUILayout.BeginHorizontal ();
        for (int y = 0; y < 3; y++) {
            EditorGUILayout.BeginVertical ();
            for (int x = 0; x < 3; x++)
            {

                shape.columns[x].rows[y] = EditorGUILayout.Toggle(shape.columns[x].rows[y]);
            }
            EditorGUILayout.EndVertical ();
        }
        EditorGUILayout.EndHorizontal ();

    }
}