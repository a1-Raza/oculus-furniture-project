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

    public string _downloadUrl;
    public string _destinationFolder;


    // Start is called before the first frame update
    void Awake()
    {
        _destinationFolder = Path.Combine(Application.persistentDataPath, "glb");
        _downloadUrl = FindAnyObjectByType<ConnectionHandler>().GetDownloadUrl();
    }

    private void Start()
    {
        modelsInScene = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DownloadGLBModel(string modelname, string modelid)
    {
        DownloadFromURL(_downloadUrl, modelname, modelid, false, Vector3.zero, Vector3.zero);
    }
    public void DownloadGLBModel(string modelname, string modelid, bool loadDownloadedModel, Vector3 spawnPos, Vector3 rotation)
    {
        DownloadFromURL(_downloadUrl, modelname, modelid, loadDownloadedModel, spawnPos, rotation);
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
        string slash = (url[url.Length - 1] == '/') ? "" : "/";
        string downloadUrl = url + slash + modelname + "/" + modelid.Replace(".glb", "") + ".glb";
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

    public GameObject GetExternalGLBGameObject(string modelname, string modelid, Transform parent)
    {
        return GetExternalGLBGameObject(gltfLoader, modelname, modelid, parent);
    }

    public GameObject GetExternalGLBGameObject(GameObject gltfAssetPrefab, string modelname, string modelid, Transform parent)
    {
        string path = Path.Combine(_destinationFolder, modelname, modelid.Replace(".glb", "") + ".glb");
        GameObject glbObject = null;
        if (!File.Exists(path))
        {
            Debug.LogWarning("File doesn't exist at path: " + path);
            return glbObject;
        }
        try
        {
            gltfAssetPrefab.GetComponent<GltfAsset>().Url = path;
            glbObject = Instantiate(gltfAssetPrefab, parent);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception when loading model: " + ex.Message);
            return glbObject;
        }
        return glbObject;
    }

    public bool ModelExistsAtPath(string modelname, string modelid)
    {
        string path = Path.Combine(_destinationFolder, modelname, modelid.Replace(".glb", "") + ".glb"); 
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

    public void DeleteAllDownloadedModels()
    {
        DeleteAllDownloadedModels(_destinationFolder);
    }

    void DeleteAllDownloadedModels(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return;

        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        // Get all subdirectories
        string[] directories = Directory.GetDirectories(folderPath);

        // Delete all subdirectories and their contents
        foreach (string directory in directories)
        {
            DeleteAllDownloadedModels(directory);
        }

        Directory.Delete(folderPath, false);
    }

    public string GetModelPath(string modelname, string modelid)
    {
        return Path.Combine(_destinationFolder, modelname, modelid.Replace(".glb", "") + ".glb");
    }
}
