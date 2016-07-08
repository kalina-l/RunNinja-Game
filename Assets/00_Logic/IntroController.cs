using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {

    public GameObject text;
    public float textVelocity;
    public float finisheAtY;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float time = Time.deltaTime;
        RectTransform rt = text.GetComponent<RectTransform>();
        Vector2 newPos = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + textVelocity);
        rt.anchoredPosition = newPos;

        Debug.Log("newPos.y "+ newPos.y);

        if (newPos.y > finisheAtY)
        {
            Debug.Log("GOTO Next Scene");
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

    }
}
