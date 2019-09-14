using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookRotation : MonoBehaviour {
    public Vector3 right;

    public Vector3 up;

    public bool calculate;

    private bool previous;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (calculate && previous == false) {
            Vector3 forward = Vector3.Cross(right, up);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }

        previous = calculate;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        Handles.Label(transform.position + transform.forward * 2, "Object space's Forward");
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 2);
        Handles.Label(transform.position + transform.right * 2, "Object space's Right");
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 2);
        Handles.Label(transform.position + transform.up * 2, "Object space's Up");

        Vector3 forwardCrossProductRight = Vector3.Cross(transform.forward, transform.right);
        Handles.DrawArrow(0, transform.position, Quaternion.FromToRotation(Vector3.forward, forwardCrossProductRight), 1);
    }
}