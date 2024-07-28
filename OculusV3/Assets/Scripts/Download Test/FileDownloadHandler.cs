using System;
using System.IO;
using System.Net.Http;
using UnityEngine;
using TMPro;
using UnityEditor;

public class FileDownloadHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField inputURL;

    //static readonly string _destinationFolder = "@\"C:\\path\\to\\destination\\folder\"";
    static readonly string _destinationFolder = "C:\\Users\\abdur\\Documents\\OculusFurnitureDownloadedAssets";
    static readonly string _destinationUSDZ = "\\usdz";
    static readonly string _destinationBundles = "\\bundles";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DownloadFromURLTextbox()
    {
        Debug.Log(inputURL.text);
        DownloadFromURL(inputURL.text);
    }

    public static async void DownloadFromURL(string url)
    {
        string downloadUrl = url; // The URL of the file to download
        string destinationFolder = _destinationFolder + _destinationUSDZ; // The destination folder

        // Ensure the destination folder exists
        if (!Directory.Exists(destinationFolder)) Directory.CreateDirectory(destinationFolder);

        string fileName = Path.GetFileName(downloadUrl); // Get the file name from the URL
        string destinationPath = Path.Combine(destinationFolder, fileName); // Combine the folder and file name

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

                CreateAssetBundle(destinationPath);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred: " + ex.Message);
        }
    }

    public static void CreateAssetBundle(string externalFilePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(externalFilePath);
        string tempPath = "Assets/Temp/" + Path.GetFileName(externalFilePath);
        File.Copy(externalFilePath, tempPath, true);
        AssetDatabase.ImportAsset(tempPath);
        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(tempPath);

        //string externalBundleDirectory = Path.Combine(Application.persistentDataPath, "AssetBundles");
        string externalBundleDirectory = _destinationFolder + _destinationBundles;
        if (!Directory.Exists(externalBundleDirectory))
        {
            Directory.CreateDirectory(externalBundleDirectory);
        }

        AssetBundleBuild buildMap = new AssetBundleBuild();
        buildMap.assetBundleName = fileName + ".bundle";
        buildMap.assetNames = new[] { tempPath };

        BuildPipeline.BuildAssetBundles(externalBundleDirectory, new[] { buildMap }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        AssetDatabase.DeleteAsset(tempPath);
        AssetDatabase.Refresh();
    }
}
