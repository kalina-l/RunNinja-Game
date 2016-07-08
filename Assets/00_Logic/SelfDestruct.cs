using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Destroy ());
	}

	private IEnumerator Destroy()
	{
		yield return new WaitForSeconds (2);

		GameObject.Destroy (gameObject);
	}
}
