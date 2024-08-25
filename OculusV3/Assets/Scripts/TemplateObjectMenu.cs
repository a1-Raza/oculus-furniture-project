using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateObjectMenu : MonoBehaviour
{
    TemplateObject templateObject;
    List<ValidModel> validModels;

    [SerializeField] GameObject modelButtonPrefab;
    [SerializeField] Transform scrollViewContent;

    private void Awake()
    {
        validModels = null;
        templateObject = GetComponentInParent<TemplateObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeButtons());
    }

    // Update is called once per frame
    void Update()
    {
        /*string f = "";
        foreach (ValidModel model in validModels) f += model.name + ", ";
        Debug.Log(f);*/
    }

    IEnumerator InitializeButtons()
    {
        float initialTime = Time.time;
        if (validModels == null)
        {
            yield return new WaitUntil(() => (templateObject.GetValidModelsList() != null && templateObject.GetValidModelsList().Count >= 1) || Mathf.Abs(Time.time - initialTime) > 5f);
            validModels = templateObject.GetValidModelsList();
            if (validModels == null) yield break;
        }

        foreach (ValidModel model in validModels) 
        {
            foreach (string file in model.files)
            {
                GameObject newButton = Instantiate(modelButtonPrefab, scrollViewContent);
                newButton.GetComponent<TemplateObjectMenuButton>().SetupButton(this, model.name, file);
                if (FindAnyObjectByType<FileDownloadHandler>().ModelExistsAtPath(model.name, file)) newButton.GetComponent<TemplateObjectMenuButton>().SetDownloadIcon(false);
                else newButton.GetComponent<TemplateObjectMenuButton>().SetDownloadIcon(true);
            }
        }
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void DeleteModelFiles()
    {
        FindAnyObjectByType<FileDownloadHandler>().DeleteAllDownloadedModels();
    }

    public void ChangeModelObject(string name, string file)
    {
        templateObject.ChangeModel(name, file);
    }
}
