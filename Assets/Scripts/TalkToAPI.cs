using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAPI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetFaces(byte[] image)
    {
        
    }

    string subscriptionKeyFace = "";
    string faceEndpoint = "";

    IEnumerator RunFacialRecognition(byte[] image)
    {
        var headers = new Dictionary<string, string>()
        {
            {"Ocp-Apim-Subscription-Key", "" },
            { "Content-Type", "application/octet-stream"}
        };

        WWW www = new WWW(faceEndpoint, image, headers);
        yield return www;

        string jsonResults = www.text;
        jsonResults = "{\"faces\" : " + jsonResults + "}";
        
        List<string> faces

    }

}
