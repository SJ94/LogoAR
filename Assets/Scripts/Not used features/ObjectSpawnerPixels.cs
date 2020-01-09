using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawnerPixels : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Text forTest;
    List<GameObject> spawnedObjects = new List<GameObject>();
    List<Vector2> spawnPositionsScreen = new List<Vector2>();
    int touchID;

    TouchPhase tPhase;
    Vector2 startPos;
    float distance;
    float worldDistance;

    void Start()
    {
    }

    void Update()
    {
        List<Vector3> spawnPositionsWorld = new List<Vector3>();

        // converts spawn positions to world coordinates so strokes can align based on their initial spawn position
        for (int i = 0; i < spawnPositionsScreen.Count; i++)
        {
            spawnPositionsWorld.Add(Camera.main.ScreenToWorldPoint(new Vector3(spawnPositionsScreen[i].x, spawnPositionsScreen[i].y, Camera.main.nearClipPlane + 0.35f)));
        }

        if (Input.touchCount > 0)
        {
            // Registers if first touch is on record button - if so, get second touch
            if (Input.touches.Length == 2 && Input.touches[0].position.y < Screen.height / 8)
                touchID = 1;
            else
                touchID = 0;

            // Determines the length of the Touch movement at which Touch should be registered as "Moving" to start drawing
            DrawingHandler();

            if (Input.touches[touchID].phase == tPhase && Input.touches[touchID].position.y > Screen.height / 8)
            {
                var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[touchID].position.x, Input.touches[touchID].position.y, Camera.main.nearClipPlane + 0.35f));

                // gets spawn positions in screen coordinates
                spawnPositionsScreen.Add(Input.touches[touchID].position);

                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                GameObject obj = Instantiate(objectToSpawn, spawnPosition, rot);
                spawnedObjects.Add(obj);
            }
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            var objPosition = spawnedObjects[i].transform.position;

            var objcamDistanceX = spawnPositionsWorld[i].x - objPosition.x;
            var objcamDistanceY = spawnPositionsWorld[i].y - objPosition.y;
            var objcamDistanceZ = spawnPositionsWorld[i].z - objPosition.z;

            forTest.text = /*"Distance X: " + objcamDistanceX + "\nDistance Y: " + objcamDistanceY + */"\nDistance Z: " + objcamDistanceZ;

            for (int j = 0; j < spawnedObjects[i].transform.childCount; j++)
            {
                spawnedObjects[i].transform.GetChild(j).gameObject.SetActive(false);
            }

            if (Mathf.Abs(objcamDistanceZ) < 0.02f)
                spawnedObjects[i].transform.GetChild(0).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.04f && Mathf.Abs(objcamDistanceZ) < 0.06f)
                spawnedObjects[i].transform.GetChild(1).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.06f && Mathf.Abs(objcamDistanceZ) < 0.08f)
                spawnedObjects[i].transform.GetChild(2).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.08f && Mathf.Abs(objcamDistanceZ) < 0.1f)
                spawnedObjects[i].transform.GetChild(3).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.1f && Mathf.Abs(objcamDistanceZ) < 0.12f)
                spawnedObjects[i].transform.GetChild(4).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.12f && Mathf.Abs(objcamDistanceZ) < 0.14f)
                spawnedObjects[i].transform.GetChild(5).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.14f && Mathf.Abs(objcamDistanceZ) < 0.16f)
                spawnedObjects[i].transform.GetChild(6).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.16f && Mathf.Abs(objcamDistanceZ) < 0.18f)
                spawnedObjects[i].transform.GetChild(7).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.18f && Mathf.Abs(objcamDistanceZ) < 0.2f)
                spawnedObjects[i].transform.GetChild(8).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.2f && Mathf.Abs(objcamDistanceZ) < 0.22f)
                spawnedObjects[i].transform.GetChild(9).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.22f && Mathf.Abs(objcamDistanceZ) < 0.24f)
                spawnedObjects[i].transform.GetChild(10).gameObject.SetActive(true);
            else if (Mathf.Abs(objcamDistanceZ) > 0.24f)
                spawnedObjects[i].transform.GetChild(11).gameObject.SetActive(true);

        }
    }

    void DrawingHandler()
    {
        Touch touch = Input.GetTouch(touchID);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPos = touch.position;
                tPhase = TouchPhase.Began;
                break;

            case TouchPhase.Moved:

                //get length of Touch movement in screen and world space
                distance = Vector2.Distance(touch.position, startPos);
                worldDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane)), Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, Camera.main.farClipPlane)));

                if (distance > 50 || worldDistance > 0.01)
                {
                    tPhase = TouchPhase.Moved;
                }
                else
                {
                    tPhase = TouchPhase.Began;
                }
                break;
        }
    }
}