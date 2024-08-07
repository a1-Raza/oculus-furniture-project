using System;
using System.IO;
using System.Net.Http;
using UnityEngine;
using GLTFast;
using System.Collections.Generic;

public class FileDownloadHandler : MonoBehaviour
{

    public bool _poc1 = false;
    public Vector3 _poc1Position = new Vector3(0, 0, 3);
    public Vector3 _poc1Rotation = new Vector3(0, 180, 0);

    [SerializeField] GameObject gltfLoader;

    List<GameObject> modelsInScene;

    //readonly string objPath = "C:\\Users\\abdur\\Documents\\OculusFurnitureDownloadedAssets\\usdz\\Nissan_350Z.obj";

    public readonly string _downloadUrl = "http://192.168.1.21:5000/download";
    public string _destinationFolder;


    // Start is called before the first frame update
    void Awake()
    {
        _destinationFolder = Path.Combine(Application.persistentDataPath, "glb");
    }

    private void Start()
    {
        modelsInScene = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DownloadGLBModel(string modelname, string modelid, Vector3 spawnPos, Vector3 rotation)
    {
        DownloadFromURL(_downloadUrl, modelname, modelid, true, spawnPos, rotation);
    }

    public void DownloadGLBModel(string modelname, string modelid, bool loadDownloadedModel)
    {
        DownloadFromURL(_downloadUrl, modelname, modelid, loadDownloadedModel, Vector3.zero, Vector3.zero);
    }

    async void DownloadFromURL(string url, string modelname, string modelid, bool loadDownloadedModel, Vector3 spawnPos, Vector3 rotation)
    {
        string downloadUrl = url + "/" + modelname + "/" + modelid + ".glb";
        // Ensure the destination folder exists
        if (!Directory.Exists(_destinationFolder)) Directory.CreateDirectory(_destinationFolder);

        Uri downloadUri = new Uri(downloadUrl);
        string folderName = Path.GetDirectoryName(downloadUri.AbsolutePath).Replace("/download/", "").Replace("\\download\\", "");
        if (!Directory.Exists(Path.Combine(_destinationFolder, folderName))) Directory.CreateDirectory(Path.Combine(_destinationFolder, folderName));

        string fileName = Path.GetFileName(downloadUrl); // Get the file name from the URL
        string destinationPath = Path.Combine(_destinationFolder, folderName, fileName); // Combine the folder and file name

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();

                using (FileStream fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                    Debug.Log("Download completed. File saved to " + destinationPath);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(gameObject.name + " - An error occurred: " + ex.Message);
            return;
        }

        if (loadDownloadedModel) LoadExternalGLB(modelname, modelid, spawnPos, rotation);
    }

    public int LoadExternalGLB(string modelname, string modelid, Vector3 spawnPos, Vector3 rotation)
    {
        return LoadExternalGLB(Path.Combine(_destinationFolder, modelname, modelid + ".glb"), spawnPos, rotation);
    }

    public int LoadExternalGLB(string modelname, string modelid)
    {
        return LoadExternalGLB(Path.Combine(_destinationFolder, modelname, modelid + ".glb"), Vector3.zero, Vector3.zero);
    }

    int LoadExternalGLB(string path, Vector3 spawnPos, Vector3 rotation)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("File doesn't exist at path: " + path);
            return 1;
        }
        try
        {
            gltfLoader.GetComponent<GltfAsset>().Url = path;
            GameObject newGltf = Instantiate(gltfLoader, spawnPos, Quaternion.identity);
            newGltf.transform.eulerAngles = rotation;
            modelsInScene.Add(newGltf);
        }
        catch (Exception ex) 
        {
            Debug.LogError("Exception when loading model: " + ex.Message);
            return 2;
        }
        return 0;
    }

    public bool ModelExistsAtPath(string modelname, string modelid)
    {
        string path = Path.Combine(_destinationFolder, modelname, modelid + ".glb");
        return File.Exists(path);
    }

    public void DeleteAllModelsInScene()
    {
        for (int i = modelsInScene.Count-1; i >= 0; i--)
        {
            GameObject toDestroy = modelsInScene[i];
            modelsInScene.RemoveAt(i);
            Destroy(toDestroy);
        }
    }
}
