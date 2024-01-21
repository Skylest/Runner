using UnityEngine;

[CreateAssetMenu(fileName = "LineItem", menuName = "ScriptableObjects/LineItem", order = 1)]
public class LineItem : ScriptableObject
{

    public enum Type
    {
        Passable,
        Impassable
    }

    [SerializeField]
    public Type type;

    [SerializeField]
    public GameObject gameObjectPrefab;
}