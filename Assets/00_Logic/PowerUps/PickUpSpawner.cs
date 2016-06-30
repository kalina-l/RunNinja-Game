using UnityEngine;
using System.Collections;

public class PickUpSpawner : MonoBehaviour 
{
    public GameObject pickUpPrefab;
    public float respawnTime = 5;
    private float respawnTimer;

    private GameObject currentObject;


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentObject == null)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                Spawn();
            }
        }
	
	}

    private void Spawn()
    {
        currentObject = GameObject.Instantiate(pickUpPrefab) as GameObject;
        currentObject.transform.SetParent(transform);
        currentObject.transform.localPosition = Vector3.zero;
        currentObject.transform.localScale = Vector3.one;
        respawnTimer = respawnTime;
    }
}
