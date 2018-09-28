// Source: https://breakdownblogs.wordpress.com/2015/11/13/adding-image-target-to-cloud-database-use-api-vuforia-and-www-unity/

using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class PostNewTrackableRequest
{
    public string name;
    public float width;
    public string image;
    public string application_metadata;
}

public class CloudUploading : MonoBehaviour
{
    public Texture2D texture;
    public string metadataStr;

    // Server Access Key
    public string access_key = "e620edcd9e50d369339c21a83248d05738cc842b";
    // Server Secret Key
    public string secret_key = "ef82996acfbc92cdbc4d9a753ba90d422216aa92";
    private string url = @"https://vws.vuforia.com";
    private string targetName = "MyTarget";
    private byte[] requestBytesArray;

    public void CallPostTarget()
    {
        StartCoroutine(PostNewTarget());
    }


    private IEnumerator PostNewTarget()
    {
        targetName += UnityEngine.Random.Range(1, 1000);
        string requestPath = "/targets";
        string serviceURI = url + requestPath;
        string httpAction = "POST";
        string contentType = "application/json";
        string date = string.Format("{0:r}", DateTime.Now.ToUniversalTime());

        byte[] image = texture.EncodeToJPG(80);

        byte[] metadata = System.Text.ASCIIEncoding.ASCII.GetBytes(metadataStr);
        PostNewTrackableRequest model = new PostNewTrackableRequest();
        model.name = targetName;
        model.width = 0.5f; // the distance of the camera will be twice in Unity scale
        model.image = System.Convert.ToBase64String(image);
        model.application_metadata = System.Convert.ToBase64String(metadata);
        string requestBody = JsonUtility.ToJson(model);

        WWWForm form = new WWWForm();

        var headers = form.headers;
        byte[] rawData = form.data;
        headers["host"] = url;
        headers["date"] = date;
        headers["content-type"] = contentType;

        MD5 md5 = MD5.Create();
        byte[] contentMD5bytes = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(requestBody));
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < contentMD5bytes.Length; i++)
        {
            sb.Append(contentMD5bytes[i].ToString("x2"));
        }

        string contentMD5 = sb.ToString();

        string stringToSign = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", httpAction, contentMD5, contentType, date, requestPath);

        HMACSHA1 sha1 = new HMACSHA1(System.Text.Encoding.ASCII.GetBytes(secret_key));
        byte[] sha1Bytes = System.Text.Encoding.ASCII.GetBytes(stringToSign);
        MemoryStream stream = new MemoryStream(sha1Bytes);
        byte[] sha1Hash = sha1.ComputeHash(stream);
        string signature = System.Convert.ToBase64String(sha1Hash);

        headers["authorization"] = string.Format("VWS {0}:{1}", access_key, signature);
        Debug.Log(string.Format("VWS {0}:{1}", access_key, signature));
        Debug.Log("<color=green>Signature: " + signature + "</color>");
        byte[] jsonPost = System.Text.Encoding.UTF8.GetBytes(requestBody);
        
        WWW request = new WWW(serviceURI, jsonPost, headers);
        yield return request;

        if (request.error != null)
        {
            Debug.Log("request error: " + request.error);
        }else
        {
            Debug.Log("request success");
            Debug.Log("returned data" + request.text);
        }

    }
}