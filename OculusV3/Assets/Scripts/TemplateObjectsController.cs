using GLTFast;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.VisualScripting;

public class TemplateObjectsController : MonoBehaviour
{
    public string template = "bedroom";
    public GameObject templateObject;

    ConnectionHandler connectionHandler;
    FileDownloadHandler fileDownloadHandler;

    string templateJsonString;

    // Start is called before the first frame update
    void Awake()
    {
        connectionHandler = FindAnyObjectByType<ConnectionHandler>();
        fileDownloadHandler = FindAnyObjectByType<FileDownloadHandler>();
        /*connectionHandler.DownloadTemplateJson(template);
        for (int i = 0; i < fileUrls.Length; i++)
        {
            gltfAssets[i].Url = fileUrls[i];
        }*/
    }

    private void Start()
    {
        StartCoroutine(AwaitJsonAndSetupTemplate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AwaitJsonAndSetupTemplate()
    {
        float initialTime = Time.time;
        yield return new WaitUntil(() => (connectionHandler.GetLatestJsonResponse() != null && connectionHandler.GetLatestJsonResponse().Length > 0) || Mathf.Abs(Time.time - initialTime) > 5f);
        if (connectionHandler.GetLatestJsonResponse() == null || connectionHandler.GetLatestJsonResponse().Length == 0) yield break;
        
        templateJsonString = connectionHandler.GetLatestJsonResponse();
        List<JsonItem> jsonItems = JsonConvert.DeserializeObject<List<JsonItem>>(templateJsonString);
        foreach (JsonItem item in jsonItems)
        {
            GameObject newPos = new GameObject();
            newPos.transform.SetParent(transform, false);
            newPos.name = item.description;
            newPos.transform.position = new Vector3(item.world_pos[0], item.world_pos[1], item.world_pos[2]);
            newPos.transform.eulerAngles = new Vector3(item.euler_angles[0], item.euler_angles[1], item.euler_angles[2]);

            SetTemplateObject(templateObject, item.default_model.name, item.default_model.file, newPos.transform, item.valid_models);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator DownloadAndLoadGLB(GameObject templateobject, string modelname, string modelid, Transform parent, List<ValidModel> validmodellist)
    {
        fileDownloadHandler.DownloadGLBModel(modelname, modelid);
        yield return new WaitForSecondsRealtime(1);
        GetExternalGLBAndAssignValidModels(templateobject, modelname, modelid, parent, validmodellist);
        
    }

    void GetExternalGLBAndAssignValidModels(GameObject templateobject, string modelname, string modelid, Transform parent, List<ValidModel> validmodellist)
    {
        GameObject modelObject = fileDownloadHandler.GetExternalGLBGameObject(templateobject, modelname, modelid, parent);
        //templateObject.GetComponent<GltfAsset>().Url = fileDownloadHandler.GetModelPath(modelname, modelid);
        if (!modelObject || modelObject == null) return;

        if (validmodellist != null) modelObject.GetComponent<TemplateObject>().SetValidModelsList(validmodellist);
    }

    public void SetTemplateObject(GameObject templateobject, string modelname, string modelid, Transform parent, List<ValidModel> validmodellist)
    {
        if (!fileDownloadHandler.ModelExistsAtPath(modelname, modelid))
        {
            StartCoroutine(DownloadAndLoadGLB(templateobject, modelname, modelid, parent, validmodellist));
        }
        else
        {
            GetExternalGLBAndAssignValidModels(templateobject, modelname, modelid, parent, validmodellist);
        }
    }
}


[System.Serializable]
public class DefaultModel
{
    public string name { get; set; }
    public string file { get; set; }
}
[System.Serializable]
public class ValidModel
{
    public string name { get; set; }
    public List<string> files { get; set; }
}
[System.Serializable]
public class JsonItem
{
    public string description { get; set; }
    public List<float> world_pos { get; set; }
    public List<float> euler_angles { get; set; }
    public List<ValidModel> valid_models { get; set; }
    public DefaultModel default_model { get; set; }
}