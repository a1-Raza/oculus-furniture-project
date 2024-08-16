using GLTFast;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TemplateObjectsController : MonoBehaviour
{
    [SerializeField] string template = "bedroom";

    [SerializeField] string[] fileUrls;
    [SerializeField] GltfAsset[] gltfAssets;

    ConnectionHandler connectionHandler;

    string templateJsonString;

    // Start is called before the first frame update
    void Awake()
    {
        connectionHandler = FindAnyObjectByType<ConnectionHandler>();
        connectionHandler.DownloadTemplateJson(template);
        for (int i = 0; i < fileUrls.Length; i++)
        {
            gltfAssets[i].Url = fileUrls[i];
        }
    }

    private void Start()
    {
        StartCoroutine(AwaitJsonResponse());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AwaitJsonResponse()
    {
        yield return new WaitForSecondsRealtime(1);
        templateJsonString = connectionHandler.GetLatestJsonResponse();
        StartCoroutine(SetupRoomTemplate());
    }

    IEnumerator SetupRoomTemplate()
    {
        yield return new WaitForEndOfFrame();
    }
}
