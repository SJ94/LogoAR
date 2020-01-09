using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    List<GameObject> spawnedObjects = new List<GameObject>();
    List<Vector3> axes = new List<Vector3>();
    List<Vector3> objAxes = new List<Vector3>();
    int touchID;

    TouchPhase tPhase;
    Vector2 startPos;
    float distance;
    float worldDistance;

    void Start()
    {
        // adding axes x, y, z to later select random axis to rotate object around
        axes.Add(new Vector3(5f, 0, 0));
        axes.Add(new Vector3(0, 5f, 0));
        axes.Add(new Vector3(0, 0, 5f));
    }

    void Update()
    {
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

                GameObject obj = Instantiate(objectToSpawn, spawnPosition, Random.rotation);
                spawnedObjects.Add(obj);
                // sets random axis for object to rotate around
                objAxes.Add(axes[Random.Range(0, axes.Count)]);
            }
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            spawnedObjects[i].transform.RotateAround(spawnedObjects[i].transform.position, objAxes[i], 40 * Time.deltaTime);
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
