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
        // if (shape.shape == null || shape.shape.Length == 0)
        // {
        //     shape.shape = new bool[9];
        // }
        EditorGUILayout.LabelField( "Shape: ",   "shape of idea");
        EditorGUILayout.BeginHorizontal ();
        for (int y = 2; y >= 0; y--) {
            EditorGUILayout.BeginVertical ();
            for (int x = 0; x < 3; x++)
            {
                shape.shape[y + x*3] = EditorGUILayout.Toggle(shape.shape[y + x*3]);
            }
            EditorGUILayout.EndVertical ();
        }
        EditorGUILayout.EndHorizontal ();

    }
}