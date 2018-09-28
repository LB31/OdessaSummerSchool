using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class PostReconstructor : MonoBehaviour, ITrackableEventHandler {
    private LineManager lineManager = null;
    private TextManager textManager = null;

    private TrackableBehaviour mTrackableBehaviour;
    public SimpleCloudHandler CloudHandler;
    public Material TrailMaterial;
    public GameObject PostParent;
    public Text text;
    public GameObject textRetracer;
    public GameObject panel;

    public static bool ObjectDetected;

    public void setMetadataContainer(Metadata mdc) {
        MetadataContainer.metadataContainer.metadata = mdc;
    }

    public void setLineManager(LineManager lm)
    {
        lineManager = lm;
    }

    public void setTextManager(TextManager tm)
    {
        textManager = tm;
    }


    void Start () {
        Screen.autorotateToPortrait = true;
       
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    void ShowDownloadedPost()
    {
        if(MetadataContainer.metadataContainer.metadata.AllLines != null) {

            for (int i = 0; i < MetadataContainer.metadataContainer.metadata.AllLines.Count; i++) {
                CreateLineObject(i);
            }
        }

        if (MetadataContainer.metadataContainer.metadata.AllImages != null) {

            for (int i = 0; i < MetadataContainer.metadataContainer.metadata.AllImages.Count; i++) {
                CreateImages();
            }
        }

        else if(MetadataContainer.metadataContainer.metadata.AllText != "")
        {
            textRetracer.SetActive(true);
            text.text = MetadataContainer.metadataContainer.metadata.AllText;
            // Sets the text background panel to its size
            float width = text.gameObject.GetComponent<RectTransform>().rect.width;
            float height = text.gameObject.GetComponent<RectTransform>().rect.height;
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }



    }

    private void CreateImages() {
        List<ImageManager> images = MetadataContainer.metadataContainer.metadata.AllImages;
        
        foreach (ImageManager image in images) {
            GameObject newImage = Instantiate(MainAppManager.mainAppManager.imagesPrefab, transform.Find("ImageRetrace").transform);
            newImage.GetComponent<Renderer>().material.SetTexture("_MainTex", MainAppManager.mainAppManager.allImages[image.ImageNumber].texture);
            newImage.transform.position = image.position;
        }

    }


    void CreateLineObject(int lineCounter)
    {
        GameObject newLine = new GameObject();
       
        newLine.transform.parent = PostParent.transform;

        newLine.name = "Line segment " + lineCounter;
        newLine.AddComponent<LineRenderer>();

        RetraceLine(newLine.GetComponent<LineRenderer>(), lineCounter);

    }

    void RetraceLine(LineRenderer line, int lineNumber)
    {

        Line currentLine = MetadataContainer.metadataContainer.metadata.AllLines[lineNumber];

        // Set the width of the Line Renderer
        line.SetWidth(0.01f, 0.01f);
        // Set the number of vertex fo the Line Renderer
        line.positionCount = currentLine.Positions.Length;
        line.material = TrailMaterial;
        //line.useWorldSpace = false;
        SetTrailColour(line, currentLine.colour);
        line.SetPositions(currentLine.Positions);
    }

    void SetTrailColour(LineRenderer lineRenderer, Vector3 colour)
    {
        float r = colour.x;
        float g = colour.y;
        float b = colour.z;
        lineRenderer.startColor = new Color(r, g, b);
        lineRenderer.endColor = new Color(r, g, b);
    }




    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }


    private void OnTrackingFound()
    {
        ShowDownloadedPost();
        ObjectDetected = true;
    }


    private void OnTrackingLost()
    {
        // Activates the cloud scanning again
        CloudHandler.mCloudRecoBehaviour.CloudRecoEnabled = true;

        foreach (Transform child in PostParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        lineManager = null;
        textManager = null;


        textRetracer.SetActive(false);
        ObjectDetected = false;
    }


}
