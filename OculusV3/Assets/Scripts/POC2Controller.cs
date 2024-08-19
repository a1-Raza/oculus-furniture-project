using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POC2Controller : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        FindAnyObjectByType<ConnectionHandler>().GetTemplateJson(GetComponent<TemplateObjectsController>().template);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
