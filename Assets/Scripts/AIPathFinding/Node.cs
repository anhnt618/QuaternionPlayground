using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]bool _isExplored = false;
    public bool IsExplored { get { return _isExplored; } set { _isExplored = value; } }

    [SerializeField]Node _exploredFrom;
    public Node ExploredFrom { get { return _exploredFrom; } set { _exploredFrom = value; } }

    public Vector2Int GetPosition() 
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x ), Mathf.RoundToInt(transform.position.y));
    }
}
