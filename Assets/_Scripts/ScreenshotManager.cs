using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour {

    private GameObject mainUIElements;
    private string lastScreenshotPath;
    private string filePath;


    private void Start() {
        mainUIElements = MainAppManager.mainAppManager.MainUIElements;
    }


    public void TakeAShot()
    {
        HideUI(true);
        StartCoroutine(CaptureIt());
    }

    private IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".jpg";
        lastScreenshotPath = fileName;
        ScreenCapture.CaptureScreenshot(lastScreenshotPath);
        yield return new WaitForEndOfFrame();
        HideUI(false);

        SaveLastFilePath();
    }

    private void SaveLastFilePath()
    {
#if UNITY_EDITOR
        filePath = Application.dataPath + "/../" + lastScreenshotPath;
#elif UNITY_ANDROID || UNITY_IOS
        filePath = Application.persistentDataPath + "/" + lastScreenshotPath;
#endif
    }

    /// <summary>
    /// Hides all UI elements to get the clean camera input
    /// </summary>
    /// <param name="on">True when the UI should be hidden; false to show it again</param>
    private void HideUI(bool hide)
    {
        mainUIElements.SetActive(!hide);
    }



    public Texture2D GetScreenshotImage()
    {

        Texture2D texture = null;
        byte[] fileBytes;
      
        fileBytes = File.ReadAllBytes(filePath);
        texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
        texture.LoadImage(fileBytes);

        return texture;
    }

    public void DeleteLastScreenshot()
    {
        File.Delete(filePath);
    }





}
