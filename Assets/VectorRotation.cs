using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Vector3 forward = Vector3.forward;
        Quaternion forwardQ = new Quaternion(forward.x, forward.y, forward.z, 0);
        Quaternion rotation = Quaternion.AngleAxis(90, Vector3.up);
        Quaternion product = rotation * forwardQ * Quaternion.Inverse(rotation);
        Vector3 rotatedForward = new Vector3(product.x, product.y, product.z);
//        Debug.Log(rotatedForward);

        rotatedForward = Quaternion.Inverse(rotation) * (rotation * forward);
//        Debug.Log(rotatedForward);
        rotatedForward = rotation * (Quaternion.Inverse(rotation) * forward);
//        Debug.Log(rotatedForward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
