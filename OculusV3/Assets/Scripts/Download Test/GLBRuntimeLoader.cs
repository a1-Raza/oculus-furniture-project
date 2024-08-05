using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLBRuntimeLoader : MonoBehaviour
{
    List<List<string>> modelInfoList; // will change how this is set later, for now am hardcoding; template is {modelname, modelid}

    [SerializeField] Transform spawnLocation;

    [SerializeField] GameObject scrollViewContent;
    [SerializeField] GameObject modelViewButtonPrefab;
    [SerializeField] List<GameObject> modelViewButtons; // serialized for debug


    private void Awake()
    {
        modelInfoList = new List<List<string>>();
        modelInfoList.Add(new List<string> { "Acrylic_Bookcase", "3205306" });
        modelInfoList.Add(new List<string> { "Acrylic_Bookcase", "8577167" });
        modelInfoList.Add(new List<string> { "Acrylic_Table_Nesting", "3969611" });
        modelInfoList.Add(new List<string> { "Acrylic_Table_Nesting", "9398081" });
        modelInfoList.Add(new List<string> { "Adirondack_Chair_Arm_MyFirst", "360841" });
        modelInfoList.Add(new List<string> { "Adirondack_Chair_Arm_MyFirst", "7693324" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "25568" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "1103523" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "1800161" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "4201260" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "4717775" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "6884553" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "8736438" });
        modelInfoList.Add(new List<string> { "Angled_Bookcase", "8965880" });
        modelInfoList.Add(new List<string> { "Anywhere_Beanbag_Regular_Ribbed_Chamois", "9160997" });
        modelInfoList.Add(new List<string> { "Anywhere_Beanbag_Regular_Ribbed_Chamois", "9587530" });
        modelInfoList.Add(new List<string> { "Anywhere_Beanbag_Regular_Ribbed_Chamois", "9606560" });
        modelInfoList.Add(new List<string> { "Anywhere_Beanbag_Regular_Ribbed_Chamois", "9768405" });

    }

    // Start is called before the first frame update
    void Start()
    {
        ResetButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetButtons()
    {
        if (modelViewButtons.Count > 0) 
        { 
            foreach (GameObject button in modelViewButtons) { Destroy(button); }
            modelViewButtons.Clear(); 
        }
        foreach (List<string> modelInfo in modelInfoList)
        {
            GameObject newButton = Instantiate(modelViewButtonPrefab, scrollViewContent.transform);
            newButton.GetComponent<ModelViewButton>().SetModelInfo(modelInfo[0], modelInfo[1]);
            modelViewButtons.Add(newButton);
        }
    }
}
