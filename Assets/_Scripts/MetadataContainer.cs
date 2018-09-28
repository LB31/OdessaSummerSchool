using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetadataContainer : MonoBehaviour {
    public static MetadataContainer metadataContainer;

    private string jsonObject;

    public Metadata metadata = new Metadata();
    

    private void Awake() {
        if (metadataContainer == null) {
            metadataContainer = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void AddImage(int imagenumber, Vector3 pos) {
        ImageManager im = new ImageManager();
        im.ImageNumber = imagenumber;
        im.position = pos;
        metadata.AllImages.Add(im);
    }

    public void AddAnimation(string animationname, Transform transform) {
        //AllImages.Add(new ImageManager(animationname, transform));
    }

    public void AddLines(List<Line> lines) {
        metadata.AllLines = lines;
    }

    public void AddText(string text) {
        metadata.AllText = text;
    }

    public string GetJSON() {
        jsonObject = JsonUtility.ToJson(metadata);
        return jsonObject;
    }
}
[System.Serializable]
public class ImageManager
{
    public int ImageNumber = -1;
    public Vector3 position;

    }

[System.Serializable]
public class AnimationManager
{
    public string AnimationName = "";
    public Transform transform;

    public AnimationManager(string animationname, Transform transform) {
        AnimationName = animationname;
        this.transform = transform;
    }

}

[System.Serializable]
public class Metadata
{
    public List<Line> AllLines = new List<Line>();
    //public List<string> AllText = new List<string>();
    public string AllText = "";
    public List<ImageManager> AllImages = new List<ImageManager>();
    public List<AnimationManager> AllAnimations = new List<AnimationManager>();


}


