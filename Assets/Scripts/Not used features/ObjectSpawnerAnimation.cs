using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerAnimation : MonoBehaviour
{
    public GameObject objectToSpawn;
    List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        // get first finger on screen and check if it's the first frame of the finger touching the screen 
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && Input.touches[0].position.y > Screen.height / 8 && Input.touches[0].position.y < Screen.height * 0.94f)
        {
            var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Camera.main.nearClipPlane + 0.35f));

            Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

            GameObject obj = Instantiate(objectToSpawn, spawnPosition, rot);
            spawnedObjects.Add(obj);
        }
    }
    public void PlayAll()
    {
        GameObject[] spawnees = GameObject.FindGameObjectsWithTag("spawnee");

        foreach (GameObject spawnee in spawnees)
            Destroy(spawnee);

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            spawnedObjects[i] = Instantiate(objectToSpawn, spawnedObjects[i].transform.position, spawnedObjects[i].transform.rotation);
        }
    }
}
