using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateObject : MonoBehaviour
{
    float initialTime;

    // Start is called before the first frame update
    void Start()
    {
        initialTime = Time.time;
        StartCoroutine(AddColliderToChild());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AddColliderToChild()
    {
        yield return new WaitUntil(() => transform.childCount > 0 || Mathf.Abs(Time.time - initialTime) > 5f);
        if (Mathf.Abs(Time.time - initialTime) > 5f) yield break;

        yield return new WaitForSecondsRealtime(0.25f);
        GameObject child = transform.GetChild(0).gameObject;
        child.AddComponent<MeshCollider>();
    }
}
