using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.XR.ARFoundation;


public class ObjectSpawnerFont : MonoBehaviour
{
    public GameObject objectToSpawn;
    List<GameObject> spawnedObjects = new List<GameObject>();
    int touchID;

    private ARRaycastManager arRaycastManager;

    string enteredText;
    public GameObject inputField;
    public Canvas inputCanvas;

    void Start()
    {
    }

    void Update()
    {
        if (Input.touchCount > 0 && inputCanvas.enabled == false)
        {
            // Registers if first touch is on record button - if so, get second touch
            if (Input.touches.Length == 2 && Input.touches[0].position.y < Screen.height / 8)
                touchID = 1;
            else
                touchID = 0;


            if (Input.touches[touchID].phase == TouchPhase.Began && Input.touches[touchID].position.y > Screen.height / 8 && Input.touches[touchID].position.y < Screen.height * 0.95f)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[touchID].position);
                RaycastHit hitObject;
                bool hitInput = false;

                if (Physics.Raycast(ray, out hitObject, 25.0f))
                {
                    if (hitObject.transform.tag == "TextInput")
                    {
                        hitInput = true;
                    }
                }

                if (hitInput == false)
                {
                    var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[touchID].position.x, Input.touches[touchID].position.y, Camera.main.nearClipPlane + 0.35f));

                    Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                    GameObject obj = Instantiate(objectToSpawn, spawnPosition, rot);
                    spawnedObjects.Add(obj);
                }
            }
        }
    }

    public void StoreText()
    {
        enteredText = inputField.GetComponent<Text>().text;
        objectToSpawn.GetComponent<TextMeshPro>().text = enteredText;
        inputCanvas.enabled = false;
    }

    public void enableInputCanvas()
    {
        inputCanvas.enabled = true;
    }
}
