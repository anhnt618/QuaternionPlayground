using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuaternionPlayground : MonoBehaviour {
    public float rotationDuration = 1;
    public float angularSpeed = 90;//90 degrees per second
    public Vector3 rotationAxis = Vector3.up;
    public bool movingTowardRotationAxis;
    public float movingSpeed;

    private Quaternion originalOrientation;
    private Quaternion d;
    private Quaternion desiredOrientation;
    private float elapsed;

    private float previousAngle = -1;
    // Start is called before the first frame update
    void Start() {
        originalOrientation = transform.rotation;
        /*Quaternion multiplication = Quaternion.Euler(60, 0, 0) * Quaternion.Euler(30, 0, 0);
        transform.rotation *= multiplication;*/
        
        /*d = Quaternion.FromToRotation(transform.forward, Vector3.right);
        desiredOrientation = originalOrientation * d;*/
    }

    // Update is called once per frame
    void Update() {
        /*elapsed += Time.deltaTime;
        float progress = elapsed / rotationDuration;
        transform.rotation = Quaternion.Lerp(originalOrientation, desiredOrientation, progress);*/

        float deltaAngularSpeed = angularSpeed * Time.deltaTime;
        Quaternion deltaD = Quaternion.AngleAxis(deltaAngularSpeed, rotationAxis);
        transform.rotation *= deltaD;
        if (movingTowardRotationAxis) {
            transform.position += rotationAxis.normalized * movingSpeed * Time.deltaTime;
        }
    }

    private void OnDrawGizmos() {
        Handles.color = Color.magenta;
        Handles.DrawArrow(0, transform.position, Quaternion.FromToRotation(Vector3.forward, rotationAxis), .5f);
        Vector3 normalizedRotationAxis = rotationAxis.normalized;
        Handles.Label(transform.position + normalizedRotationAxis * 0.5f, string.Format("Rotation axis (from input) [{0}, {1}, {2}]", normalizedRotationAxis.x, normalizedRotationAxis.y, normalizedRotationAxis.z));

        float computedAngle;
        Vector3 computedAxis;
        transform.rotation.ToAngleAxis(out computedAngle, out computedAxis);
        int flipFactor = 1;
        if (previousAngle == -1) {
            previousAngle = computedAngle;
        }
        else {
            if (previousAngle > computedAngle) {
                flipFactor = -1;
            }
            previousAngle = computedAngle;
        }
        Vector3 normalizedComputedAxis = computedAxis.normalized;
        float dotProduct = Vector3.Dot(normalizedComputedAxis, transform.up);
//        if (dotProduct < 0) {
//            normalizedComputedAxis *= -1;
//        }
        double sin = Math.Sin(Mathf.Deg2Rad * computedAngle / 2f);
        float x = (float) (transform.rotation.x / sin);
        float y = (float) (transform.rotation.y / sin);
        float z = (float) (transform.rotation.z / sin);
        normalizedComputedAxis = new Vector3(x, y, z).normalized * flipFactor;
        Handles.DrawArrow(0, transform.position, Quaternion.FromToRotation(Vector3.forward, normalizedComputedAxis), 1);
        Handles.Label(transform.position + normalizedComputedAxis, string.Format("Rotation axis (from computing)  [{0}, {1}, {2}] - Dot product with Up {3} - angle {4} - sin(theta / 2) {5}", normalizedComputedAxis.x, normalizedComputedAxis.y, normalizedComputedAxis.z, dotProduct, computedAngle, sin));
    }
}
