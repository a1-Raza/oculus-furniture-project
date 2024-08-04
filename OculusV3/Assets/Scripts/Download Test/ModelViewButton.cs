using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class ModelViewButton : MonoBehaviour
{
    FileDownloadHandler fileDownloadHandler;
    string modelname;
    string modelid;

    [SerializeField] TMP_Text buttonNameText;
    [SerializeField] TMP_Text buttonIDText;

    // Start is called before the first frame update
    void Start()
    {
        fileDownloadHandler = FindFirstObjectByType<FileDownloadHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowModel()
    {
        int errorCode = fileDownloadHandler.LoadExternalGLB(modelname, modelid);
        if (errorCode != 0)
        {
            fileDownloadHandler.DownloadGLBModel(modelname, modelid);
        }
    }

    public void SetModelInfo(string name, string id)
    {
        modelname = name;
        modelid = id;
        buttonNameText.text = modelname.Replace("_", " ");
        buttonIDText.text = id;
    }
}
