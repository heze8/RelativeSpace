using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Shape : ScriptableObject
{
    //9 bits representing from btm left to upper right
    [SerializeField]
    public bool[] shape;
    
    // [System.Serializable]
    // public class Column
    // {
    //     public bool[] rows = new bool[3];
    // }

    // public Column[] columns;
    private void OnEnable()
    {
        
    }
    
    // public void OnValidate()
    // {
    //     if (Application.isPlaying)
    //     {
    //         Debug.Log("playing");return;
    //     }
    //     
    //     int index = 0;
    //     for (int i = 2; i >= 0; i--)
    //     {
    //         for (int j = 2; j >= 0; j--)
    //         {
    //             shape[index++] = columns[i].rows[j];
    //         }
    //     }
    // }

    public Shape( bool[] shape)
    {
        this.shape = shape;
        if (shape.Length > 9)
        {
            Debug.LogError("Error");
        }
    }
}