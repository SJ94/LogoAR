using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    bool showPreview;

    void Start()
    {
        showPreview = true;

        if (PlayerPrefs.HasKey("key"))
        {
            // Don't show help
        }
        else
        {
            PlayerPrefs.SetInt("key", 0);
            PlayerPrefs.Save();
            // Show help at first app launch
            transform.Find("Instruction").gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            transform.Find("Instruction").gameObject.SetActive(false);
        }

        // only shows preview directly after recording is finished
        if (ReplayKit.recordingAvailable && showPreview)
        {
            ReplayKit.Preview();
            showPreview = false;
        }
    }

    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RecordScreen()
    {
        //for (int i = 2; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().enabled = false;
        //}
        ReplayKit.microphoneEnabled = false;
        ReplayKit.StartRecording();
    }

    public void StopRecord()
    {
        ReplayKit.StopRecording();
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        showPreview = true;
    }
}
