using UnityEditor;
using UnityEngine;

namespace DefaultNamespace {
	public class RotationWithReflection : MonoBehaviour {
		public Vector3 customForward = Vector3.one;
		public Vector3 desiredForward = Vector3.one;

		private void OnGUI() {
			if (GUI.Button(new Rect(0, 0, 100, 100), "rotate")) {
				Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, transform.rotation, DetermineScale());
				Vector3 rotatedCustomFoward = m * customForward;
				Quaternion q = Quaternion.FromToRotation(rotatedCustomFoward, desiredForward);
				transform.rotation = q;
			}
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
			Handles.Label(transform.position + transform.forward * 2, "Quaternion Forward");
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + transform.right * 2);
			Handles.Label(transform.position + transform.right * 2, "Quaternion Right");
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + transform.up);
			Handles.Label(transform.position + transform.up, "Quaternion Up");

			DrawVectorUnderRotation(transform.position, customForward, transform.rotation, "Quaternion Axis");

			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, transform.rotation, DetermineScale());
			DrawVectorUnderRotation(transform.position, customForward, m, "Matrix axis");
			DrawVectorUnderRotation(transform.position, Vector3.forward, m, "Matrix forward");
			DrawVectorUnderRotation(transform.position, Vector3.right, m, "Matrix right");
			DrawVectorUnderRotation(transform.position, Vector3.up, m, "Matrix up");

			DrawVectorUnderRotation(transform.position, desiredForward * 2, Quaternion.identity, "Desired");
		}

		private void DrawVectorUnderRotation(Vector3 placement, Vector3 unrotatedVector, Quaternion rotation,
		                                     string label) {
			Vector3 rotatedVector = rotation * unrotatedVector;
			Vector3 endPoint = placement + rotatedVector;
			Gizmos.DrawLine(placement, endPoint);
			Handles.Label(endPoint, label);
		}

		private void DrawVectorUnderRotation(Vector3 placement, Vector3 unrotatedVector, Matrix4x4 trs, string label) {
			Vector3 rotatedVector = trs * unrotatedVector;
			Vector3 endPoint = placement + rotatedVector;
			Gizmos.DrawLine(placement, endPoint);
			Handles.Label(endPoint, label);
		}

		Vector3 DetermineScale() {
			Vector3 tempScale = new Vector3(1, 1, 1);
			Transform t = this.transform;
			while (t != null) {
				tempScale = new Vector3(
					tempScale.x * t.localScale.x, 
					tempScale.y * t.localScale.y,
					tempScale.z * t.localScale.z
				);
				t = t.parent;
			}

			return tempScale;
		}
	}
}