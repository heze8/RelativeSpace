using UnityEngine;

[CreateAssetMenu]
public class Idea : ScriptableObject
{
    public Idea antitheticalIdea;
    public Usage usage;
    [SerializeField]
    public Shape shape;
}


public class Usage : ScriptableObject
{
    
}
