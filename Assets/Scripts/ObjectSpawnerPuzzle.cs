using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerPuzzle : MonoBehaviour
{
    public GameObject objectToSpawn1;
    public GameObject objectToSpawn2;
    public GameObject objectToSpawn3;
    int touchID;
    List<GameObject> PuzzleList = new List<GameObject>();
    List<GameObject> spawnedObjects = new List<GameObject>();
    List<Vector2> spawnPositionsScreen = new List<Vector2>();
    int puzzleIndex;

    TouchPhase tPhase;
    Vector2 startPos;
    float distance;
    float worldDistance;

    void Start()
    {
        PuzzleList.Add(objectToSpawn1);
        PuzzleList.Add(objectToSpawn2);
        PuzzleList.Add(objectToSpawn3);
        puzzleIndex = 0;
    }

    void Update()
    {
        List<Vector3> spawnPositionsWorld = new List<Vector3>();

        // converts spawn positions to world coordinates
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

            if (Input.touches[touchID].phase == tPhase && Input.touches[touchID].position.y > Screen.height / 8 && Input.touches[touchID].position.y < Screen.height * 0.95f)
            {
                var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[touchID].position.x, Input.touches[touchID].position.y, Camera.main.nearClipPlane + 0.35f));

                // gets spawn positions in screen coordinates
                spawnPositionsScreen.Add(Input.touches[touchID].position);

                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                GameObject objectToSpawn = PuzzleList[puzzleIndex];

                GameObject obj = Instantiate(objectToSpawn, spawnPosition, rot);
                spawnedObjects.Add(obj);

                if (puzzleIndex < PuzzleList.Count - 1)
                    puzzleIndex++;
                else
                    puzzleIndex = 0;
            }
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            var objPosition = spawnedObjects[i].transform.position;

            var objcamDistanceX = spawnPositionsWorld[i].x - objPosition.x;
            var objcamDistanceY = spawnPositionsWorld[i].y - objPosition.y;
            var objcamDistanceZ = spawnPositionsWorld[i].z - objPosition.z;

            List<Transform> puzzlePieces = new List<Transform>();

            for (int j = 0; j < spawnedObjects[i].transform.childCount; j++)
            {
                puzzlePieces.Add(spawnedObjects[i].transform.GetChild(j));
            }

            if (spawnedObjects[i].transform.tag == "Puzzle1")
            {
                Puzzle1Transform(i, objPosition, objcamDistanceX, puzzlePieces);
            }
            else if (spawnedObjects[i].transform.tag == "Puzzle2")
            {
                Puzzle2Transform(i, objPosition, objcamDistanceZ, puzzlePieces);
            }
            else if (spawnedObjects[i].transform.tag == "Puzzle3")
            {
                Puzzle3Transform(i, objPosition, objcamDistanceX + objcamDistanceZ, puzzlePieces);
            }
        }
    }

    private void Puzzle1Transform(int i, Vector3 objPosition, float objcamDistance, List<Transform> puzzlePieces)
    {
        int m = 1;
        int n = 1;
        int k = 1;

        for (int j = 0; j < puzzlePieces.Count; j++)
        {
            puzzlePieces[j].localEulerAngles = new Vector3(0f, 0f, objcamDistance * 500f * k);
            m += 1;
            n *= -1;
            k = m * n;
        }

        puzzlePieces[0].position = new Vector3(objPosition.x + objcamDistance * 3, objPosition.y + objcamDistance * -1, objPosition.z);
        puzzlePieces[1].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * -3, objPosition.z);
        puzzlePieces[2].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * 2, objPosition.z);
        puzzlePieces[3].position = new Vector3(objPosition.x + objcamDistance * -3, objPosition.y + objcamDistance * 2, objPosition.z);
    }

    private void Puzzle2Transform(int i, Vector3 objPosition, float objcamDistance, List<Transform> puzzlePieces)
    {
        int m = 1;
        int n = 1;
        int k = 1;

        for (int j = 0; j < puzzlePieces.Count; j++)
        {
            puzzlePieces[j].localEulerAngles = new Vector3(0f, 0f, objcamDistance * 300f * k);
            m += 1;
            n *= -1;
            k = m * n;
        }

        puzzlePieces[0].position = new Vector3(objPosition.x + objcamDistance * 3, objPosition.y + objcamDistance * -1, objPosition.z);
        puzzlePieces[1].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * -3, objPosition.z);
        puzzlePieces[2].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * 3, objPosition.z);
        puzzlePieces[3].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * 2, objPosition.z);
        puzzlePieces[4].position = new Vector3(objPosition.x + objcamDistance * 3, objPosition.y + objcamDistance * -1, objPosition.z);
        puzzlePieces[5].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * 2, objPosition.z);
    }

    private void Puzzle3Transform(int i, Vector3 objPosition, float objcamDistance, List<Transform> puzzlePieces)
    {
        int m = 1;
        int n = 1;
        int k = 1;

        for (int j = 0; j < puzzlePieces.Count; j++)
        {
            puzzlePieces[j].localEulerAngles = new Vector3(0f, 0f, objcamDistance * 200f * k);
            m += 1;
            n *= -1;
            k = m * n;
        }

        puzzlePieces[0].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * -1, objPosition.z);
        puzzlePieces[1].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * -2, objPosition.z);
        puzzlePieces[2].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * 1, objPosition.z);
        puzzlePieces[3].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * 2, objPosition.z);
        puzzlePieces[4].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * -1, objPosition.z);
        puzzlePieces[5].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * -2, objPosition.z);
        puzzlePieces[6].position = new Vector3(objPosition.x + objcamDistance * 1, objPosition.y + objcamDistance * 1, objPosition.z);
        puzzlePieces[7].position = new Vector3(objPosition.x + objcamDistance * -2, objPosition.y + objcamDistance * 2, objPosition.z);
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
