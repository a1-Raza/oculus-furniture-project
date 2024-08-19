using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    readonly string _apiUrl = "http://192.168.1.21:5000/";
    readonly string _apiDownload = "download/";
    readonly string _apiPOC2Jsons = "get/poc2/templates/";
    string _poc2JsonDestinationFolder;

    static string latestJsonResponse = null;


    private void Awake()
    {
        _poc2JsonDestinationFolder = Path.Combine(Application.persistentDataPath, "templates");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public string GetDownloadUrl()
    {
        return _apiUrl + _apiDownload;
    }

    public string GetPOC2JsonsUrl()
    {
        return _apiUrl + _apiPOC2Jsons;
    }

    public string GetLatestJsonResponse() { return latestJsonResponse; }

    public async void GetTemplateJson(string templateFileName)
    {
        string apiUrl = GetPOC2JsonsUrl() + templateFileName.Replace(".json", "") + ".json";
        //Debug.Log(apiUrl);

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Make the GET request to the API
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Ensure the response was successful
                response.EnsureSuccessStatusCode();

                // Read the JSON content from the response
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse != null) latestJsonResponse = jsonResponse;
                //Debug.Log($"{jsonResponse}");
                /*
                // Save the JSON response to a file
                File.WriteAllText(_poc2JsonDestinationFolder, jsonResponse);

                Debug.Log("JSON response saved to " + _poc2JsonDestinationFolder);*/
            }
            catch (HttpRequestException e)
            {
                Debug.Log("Request error: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.Log("General error: " + e.Message);
            }
        }
    }
}
