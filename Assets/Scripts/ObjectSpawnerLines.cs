using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerLines : MonoBehaviour
{
    public GameObject objectToSpawn;
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
            spawnPositionsWorld.Add(Camera.main.ScreenToWorldPoint(new Vector3 (spawnPositionsScreen[i].x, spawnPositionsScreen[i].y, Camera.main.nearClipPlane + 0.35f)));
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

            if (Input.touches[touchID].phase == tPhase && Input.touches[touchID].position.y > Screen.height / 8 && Input.touches[touchID].position.y < Screen.height * 0.95f)
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

            float k = 0.1f;

            // aligning strokes relative to the distance between camera and initial spawn position
            for (int j = 1; j < spawnedObjects[i].transform.childCount; j++)
            {
                Transform stroke = spawnedObjects[i].transform.GetChild(j);
                stroke.position = new Vector3(objPosition.x + objcamDistanceX * k, objPosition.y + objcamDistanceY * k, objPosition.z + objcamDistanceZ * k);
                k += 0.1f;
            }
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

