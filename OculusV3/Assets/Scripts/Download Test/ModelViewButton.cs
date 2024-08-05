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

    [SerializeField] GameObject downloadIcon;

    private void Awake()
    {
        downloadIcon.SetActive(true);
        fileDownloadHandler = FindAnyObjectByType<FileDownloadHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetDownloadIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowModel()
    {
        int errorCode = 0;
        if (fileDownloadHandler.ModelExistsAtPath(modelname, modelid)) errorCode = fileDownloadHandler.LoadExternalGLB(modelname, modelid);
        else fileDownloadHandler.DownloadGLBModel(modelname, modelid, true);
        //if (errorCode != 0) fileDownloadHandler.DownloadGLBModel(modelname, modelid, true);
        StartCoroutine(WaitSetDownloadIcon());
    }

    public void SetModelInfo(string name, string id)
    {
        modelname = name;
        modelid = id;
        buttonNameText.text = modelname.Replace("_", " ");
        buttonIDText.text = id;

        SetDownloadIcon();
    }

    IEnumerator WaitSetDownloadIcon()
    {
        yield return new WaitForSecondsRealtime(1);
        if (!fileDownloadHandler.ModelExistsAtPath(modelname, modelid)) downloadIcon.SetActive(true);
        else downloadIcon.SetActive(false);
    }

    void SetDownloadIcon()
    {
        if (!fileDownloadHandler.ModelExistsAtPath(modelname, modelid)) downloadIcon.SetActive(true);
        else downloadIcon.SetActive(false);
    }

    void SetDownloadIcon(bool tf)
    {
        downloadIcon.SetActive(tf);
    }
}
