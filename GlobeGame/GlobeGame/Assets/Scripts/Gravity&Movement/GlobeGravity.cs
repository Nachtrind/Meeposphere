using UnityEngine;
using System.Collections;

public class GlobeGravity : MonoBehaviour
{

	public float gravity = -10.0f;

	public void Gravitate (Transform _meepleTrans)
	{
		Vector3 gravityUp = (_meepleTrans.position - transform.position).normalized;
		Vector3 meepleUp = _meepleTrans.up;

		_meepleTrans.GetComponentInParent<Rigidbody> ().AddForce (gravityUp * gravity);

		Quaternion targetRot = Quaternion.FromToRotation (meepleUp, gravityUp) * _meepleTrans.rotation;
		_meepleTrans.rotation = Quaternion.Slerp (_meepleTrans.rotation, targetRot, 50 * Time.deltaTime);

	}
}
