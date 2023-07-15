using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class RestApi : MonoBehaviour
{
   private string URL = "https://64b1ac45062767bc48268582.mockapi.io/PirateGames";

    public TMP_Text UpdateText;
    public TMP_Text ExplanationText;
    public int index;
    void Start()
    {
        StartCoroutine(GetDatas());
    }

  IEnumerator GetDatas () 
    { 
        using UnityWebRequest request= UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.LogError(request.error);
        else
        {
            string json = request.downloadHandler.text;
            SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
            UpdateText.text = "Update Version:" + stats[index]["update"];
            ExplanationText.text = "Explanation:" + stats[index]["explanation"];
        }

      
    }
}
