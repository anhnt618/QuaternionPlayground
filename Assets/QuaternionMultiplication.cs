using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuaternionMultiplication : MonoBehaviour {
	public Quaternion a = FromToRotation(Vector3.forward, Vector3.right);
	public Quaternion b = FromToRotation(Vector3.right, Vector3.up);
	public Quaternion c = FromToRotation(Vector3.up, Vector3.forward);
	public Quaternion d = FromToRotation(Vector3.up, Vector3.right);
	public bool calculate;

	private bool previous;
	private Quaternion original;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (calculate && previous == false) {
			original = transform.rotation;
//			transform.rotation = b * a * transform.rotation;
//			transform.rotation = QuaternionRotationChaining.StartWith(transform.rotation).Then(a).Then(b).Calculate();
			QuaternionRotationChaining.StartWith(transform.rotation).Then(a).Then(b).Then(c).Then(d).Animate(this, transform);
			Vector3 axis;
			float angle;
			b.ToAngleAxis(out angle, out axis);
			Debug.Log("rotate " + angle + " " + axis);
		}

		if (!calculate && previous == true) {
			transform.rotation = original;
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
	}
	
	public static Quaternion FromToRotation(Vector3 v1, Vector3 v2)
	{
		var a = Vector3.Cross(v1, v2);
		double w = System.Math.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude) + Vector3.Dot(v1, v2);
		if (a.sqrMagnitude < 0.0001f)
		{
			//the vectors are parallel, check w to find direction
			//if w is 0 then values are opposite, and we should rotate 180 degrees around some axis
			//otherwise the vectors in the same direction and no rotation should occur
			return (System.Math.Abs(w) < 0.0001d) ? new Quaternion(0f, 1f, 0f, 0f) : Quaternion.identity;
		}
		else
		{
			return new Quaternion(a.x, a.y, a.z, (float)w).normalized;
		}
	}

	private class QuaternionRotationChaining {
		private Stack<Quaternion> chain = new Stack<Quaternion>();
		private float elapsed;
		private float period = 1;
		private Quaternion currentQ = Quaternion.identity;

		public static QuaternionRotationChaining StartWith(Quaternion initValue) {
			return new QuaternionRotationChaining(initValue);
		}

		private QuaternionRotationChaining(Quaternion initValue) {
			chain.Push(initValue);
		}

		public QuaternionRotationChaining Then(Quaternion q) {
			chain.Push(q);
			return this;
		}

		public Quaternion Calculate() {
			Quaternion baked = Quaternion.identity; 
			int count = chain.Count;
			for (int i = 0; i < count; i++) {
				baked *= chain.Pop();
			}
			return baked;
		}

		public void Animate(MonoBehaviour mb, Transform t) {
			mb.StartCoroutine(_Animate(t));
		}

		private IEnumerator _Animate(Transform t) {
			Stack<Quaternion> reversedChain = new Stack<Quaternion>(chain);
			currentQ *= reversedChain.Pop();
			t.rotation = currentQ;
			while (true) {
				yield return new WaitForEndOfFrame();
				elapsed += Time.deltaTime;
				float progress = elapsed / period;
				t.rotation = Quaternion.Slerp(currentQ, reversedChain.Peek() * currentQ, progress);
				if (elapsed >= period) {
					elapsed = 0;
					currentQ = reversedChain.Pop() * currentQ;
				}
				if(elapsed == 0 && reversedChain.Count < 1) break;
			}
		}
	}
}