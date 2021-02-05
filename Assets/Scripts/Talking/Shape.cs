using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Shape : ScriptableObject
{
    //9 bits representing from btm left to upper right
    public BitArray shape;
    
    [System.Serializable]
    public class Column
    {
        public bool[] rows = new bool[3];
    }

    public Column[] columns;

    public void OnEnable()
    {
        columns = new Column[3];
        shape = new BitArray(9);
    }

    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            Debug.Log("playing");return;
        }
        
        int index = 0;
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 2; j >= 0; j--)
            {
                shape[index++] = columns[i].rows[j];
            }
        }
    }

    public Shape(BitArray shape)
    {
        this.shape = shape;
        if (shape.Length > 9)
        {
            Debug.LogError("Error");
        }
    }
}