using UnityEditor;
using UnityEngine;

namespace DefaultNamespace {
	public class GeneralLookRotation : MonoBehaviour {
		public Vector3 customForward = Vector3.one;
		public Vector3 desiredDirectionToLookAt = Vector3.right;
		public Vector3 up = Vector3.up;
		public bool calculate;

		private bool previous;
		Vector3 parallelOfUp;
		Vector3 projectionOfUpToPlane;
		Vector3 parallelOfDesiredUp;
		Vector3 projectionOfDesiredUpToPlane;
		Vector3 planeN;
		
		// Start is called before the first frame update
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			if (calculate && previous == false) {
				transform.rotation = AlternativeLookRotation(customForward.normalized, desiredDirectionToLookAt.normalized, up.normalized);
				planeN = desiredDirectionToLookAt.normalized;
				Vector3 planeP = transform.position;
				float planeD = Vector3.Dot(planeP, planeN);
				
				Vector3 upPosition = transform.position + transform.up;
				float distanceFromUpToPlane = Vector3.Dot(upPosition, planeN) - planeD;
				parallelOfUp = -distanceFromUpToPlane * planeN;
				projectionOfUpToPlane = transform.up + parallelOfUp;
				
				Vector3 desiredUpPosition = transform.position + up;
				float distanceFromDesiredUpToPlane = Vector3.Dot(desiredUpPosition, planeN) - planeD;
				parallelOfDesiredUp = -distanceFromDesiredUpToPlane * planeN;
				projectionOfDesiredUpToPlane = up + parallelOfDesiredUp;
				
				Quaternion rotationFromPUpToPDesiredUp = Quaternion.FromToRotation(projectionOfUpToPlane, projectionOfDesiredUpToPlane);
				transform.rotation = rotationFromPUpToPDesiredUp * transform.rotation;
			}

			if (!calculate && previous) {
				transform.rotation = Quaternion.identity;
			}

			previous = calculate;
		}

		private static Quaternion AlternativeLookRotation(Vector3 alternativeForward, Vector3 desiredDirection,
		                                                  Vector3 up) {
			return Quaternion.LookRotation(desiredDirection, up) * Quaternion.Inverse(Quaternion.LookRotation(alternativeForward, up));
		}
		
		private void OnDrawGizmos() {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
			Handles.Label(transform.position + transform.forward * 2, "Object space's Forward");
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + transform.right * 2);
			Handles.Label(transform.position + transform.right * 2, "Object space's Right");
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + transform.up);
			Handles.Label(transform.position + transform.up, "Object space's Up");

			DrawVectorUnderRotation(transform.position, customForward, transform.rotation, "Custom forward");
			DrawVectorUnderRotation(transform.position, up, Quaternion.identity, "Desired up");
			DrawVectorUnderRotation(transform.position + transform.up, parallelOfUp, Quaternion.identity, "Parallel");
			DrawVectorUnderRotation(transform.position, projectionOfUpToPlane, Quaternion.identity, "Projection");
			
			DrawVectorUnderRotation(transform.position + up, parallelOfDesiredUp, Quaternion.identity, "Parallel");
			DrawVectorUnderRotation(transform.position, projectionOfDesiredUpToPlane, Quaternion.identity, "Projection");
		}

		private void DrawVectorUnderRotation(Vector3 placement, Vector3 unrotatedVector, Quaternion rotation, string label) {
			Vector3 rotatedVector = rotation * unrotatedVector;
			Vector3 endPoint = placement + rotatedVector;
			Gizmos.DrawLine(placement, endPoint);
			Handles.Label(endPoint, label);
		}
	}
}