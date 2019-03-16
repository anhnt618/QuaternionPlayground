using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class NodeFactory : MonoBehaviour 
    // Only enable the script once ~~ todo: fix this shit later
    // disable the script when in PlayMode
{
    GameObject nodePrefab;
    void Start()
    {
        CreateNodes(); 
    }

    private void CreateNodes()
    {
        nodePrefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Node.prefab");
        for (int x = -1; x < 24; x++)
        {
            for (int y = -1; y < 13; y++)
            {
                var newNode = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity, this.transform);
            }
        }
    }
}
