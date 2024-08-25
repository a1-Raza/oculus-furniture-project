using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateObjectMenuButton : MonoBehaviour
{
    TemplateObjectMenu templateObjectMenu;

    [SerializeField] TMP_Text buttonName;
    [SerializeField] TMP_Text buttonID;
    [SerializeField] GameObject downloadIcon;

    FileDownloadHandler fileDownloadHandler;

    string modelname;
    string modelid;

    // Start is called before the first frame update
    void Start()
    {
        fileDownloadHandler = FindAnyObjectByType<FileDownloadHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        SetDownloadIcon();
    }

    public void SetupButton(TemplateObjectMenu tom, string name, string id)
    {
        templateObjectMenu = tom;
        modelname = name;
        modelid = id;
        buttonName.text = modelname;
        buttonID.text = modelid;
    }

    public void ChangeModel()
    {
        templateObjectMenu.ChangeModelObject(modelname, modelid);
    }

    void SetDownloadIcon()
    {
        if (fileDownloadHandler.ModelExistsAtPath(modelname, modelid)) downloadIcon.SetActive(false);
        else downloadIcon.SetActive(true);
    }

    public void SetDownloadIcon(bool tof)
    {
        downloadIcon.SetActive(tof);
    }
}
