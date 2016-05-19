using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    // zum Szenen laden bzw austauschen

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
