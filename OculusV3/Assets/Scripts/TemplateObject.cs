using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateObject : MonoBehaviour
{
    [SerializeField] GameObject modelMenuObject; // rename to menu or something when working on menu
    GameObject modelChildObject;

    List<ValidModel> validModels = new List<ValidModel>();

    //float initialTime;
    bool modelRenderedSuccessfully;

    MeshCollider modelCollider;
    RayInteractable modelRayInteractable;
    ColliderSurface modelColliderSurface;

    TemplateObjectsController templateObjectsController;

    // Start is called before the first frame update
    void Start()
    {
        templateObjectsController = FindAnyObjectByType<TemplateObjectsController>();
        modelMenuObject.SetActive(false);
        modelRenderedSuccessfully = false;
        StartCoroutine(AddColliderToModelChild());
    }

    // Update is called once per frame
    void Update()
    {
        if (!modelRenderedSuccessfully) return;

        if (modelRayInteractable.SelectingInteractors.Count > 0)
        {
            modelMenuObject.SetActive(true);
        }
    }

    IEnumerator AddColliderToModelChild()
    {
        float initialTime = Time.time;
        modelRenderedSuccessfully = false;
        yield return new WaitUntil(() => transform.childCount > 1 || Mathf.Abs(Time.time - initialTime) > 5f);
        if (Mathf.Abs(Time.time - initialTime) > 5f) yield break;

        yield return new WaitForSecondsRealtime(0.25f);
        modelChildObject = transform.GetChild(1).gameObject;

        modelChildObject.AddComponent<MeshCollider>();
        modelChildObject.AddComponent<RayInteractable>();
        modelChildObject.AddComponent<ColliderSurface>();

        modelCollider = modelChildObject.GetComponent<MeshCollider>();
        modelRayInteractable = modelChildObject.GetComponent<RayInteractable>();
        modelColliderSurface = modelChildObject.GetComponent<ColliderSurface>();

        modelColliderSurface.InjectCollider(modelCollider);
        modelRayInteractable.InjectSurface(modelColliderSurface);

        //collideTestObject = Instantiate(collideTestObject, gameObject.transform);
        modelRenderedSuccessfully = true;
    }

    public void SetValidModelsList(List<ValidModel> input)
    {
        validModels = input;
    }

    public List<ValidModel> GetValidModelsList()
    {
        return validModels;
    }

    public void ChangeModel(string modelname, string modelid)
    {
        ValidModel newModel = null;
        foreach (ValidModel model in validModels)
        {
            if (model.name == modelname)
            {
                newModel = model;
                break;
            }
        }
        if (newModel == null) return;

        string newId = null;
        foreach (string id in newModel.files)
        {
            if (id == modelid) 
            {
                newId = id;
                break;
            }
        }
        if (newId == null) return;

        templateObjectsController.SetTemplateObject(templateObjectsController.templateObject, modelname, modelid, transform.parent, validModels);
        Destroy(gameObject);
    }
}
