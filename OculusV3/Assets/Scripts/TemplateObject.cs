using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateObject : MonoBehaviour
{
    [SerializeField] GameObject collideTestObject; // rename to menu or something when working on menu

    float initialTime;
    bool modelRenderedSuccessfully;

    MeshCollider modelCollider;
    RayInteractable modelRayInteractable;
    ColliderSurface modelColliderSurface;

    // Start is called before the first frame update
    void Start()
    {
        collideTestObject.SetActive(false);
        modelRenderedSuccessfully = false;
        initialTime = Time.time;
        StartCoroutine(AddColliderToModelChild());
    }

    // Update is called once per frame
    void Update()
    {
        if (!modelRenderedSuccessfully) return;

        if (modelRayInteractable.SelectingInteractors.Count > 0)
        {
            collideTestObject.SetActive(true);
        }
        else collideTestObject.SetActive(false);
    }

    IEnumerator AddColliderToModelChild()
    {
        yield return new WaitUntil(() => transform.childCount > 1 || Mathf.Abs(Time.time - initialTime) > 5f);
        if (Mathf.Abs(Time.time - initialTime) > 5f) yield break;

        yield return new WaitForSecondsRealtime(0.25f);
        GameObject modelChild = transform.GetChild(1).gameObject;

        modelChild.AddComponent<MeshCollider>();
        modelChild.AddComponent<RayInteractable>();
        modelChild.AddComponent<ColliderSurface>();

        modelCollider = modelChild.GetComponent<MeshCollider>();
        modelRayInteractable = modelChild.GetComponent<RayInteractable>();
        modelColliderSurface = modelChild.GetComponent<ColliderSurface>();

        modelColliderSurface.InjectCollider(modelCollider);
        modelRayInteractable.InjectSurface(modelColliderSurface);

        //collideTestObject = Instantiate(collideTestObject, gameObject.transform);
        modelRenderedSuccessfully = true;
    }
}
