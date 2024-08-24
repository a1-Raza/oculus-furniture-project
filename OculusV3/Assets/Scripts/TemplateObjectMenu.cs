using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateObjectMenu : MonoBehaviour
{
    TemplateObject templateObject;
    List<ValidModel> validModels;

    private void Awake()
    {
        templateObject = GetComponentInParent<TemplateObject>();
        validModels = templateObject.GetValidModelsList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeButtons()
    {

    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void ChangeModelObject()
    {
        templateObject.ChangeModel("Anywhere_Beanbag_Regular_Ribbed_Chamois", "9160997.glb");
    }
}
