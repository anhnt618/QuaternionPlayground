using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Node))]
public class NodeEditor : MonoBehaviour
{
    Node myNode;
    private void Awake()
    {
        myNode = GetComponent<Node>();
    }

    void Update()
    {
        SnapToPosition();
        UpdateNameBasedOnPosition();
    }

    private void UpdateNameBasedOnPosition()
    {
        string text = myNode.GetPosition().x + "," + myNode.GetPosition().y;
        gameObject.name = text;
    }

    public void SnapToPosition()
    {
        transform.position = new Vector3(myNode.GetPosition().x, myNode.GetPosition().y, 0);
    }
   
}
