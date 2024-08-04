using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OBJLoader : MonoBehaviour
{
    readonly string objPath = "C:\\Users\\abdur\\Documents\\OculusFurnitureDownloadedAssets\\usdz\\Nissan_350Z.obj";

    void Start()
    {
        LoadOBJ(objPath);
    }

    public void LoadOBJ(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return;
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> vertexIndices = new List<int>();
        List<int> uvIndices = new List<int>();
        List<int> normalIndices = new List<int>();

        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (line.StartsWith("#")) continue; // Skip comments
            string[] parts = line.Trim().Split(' ');

            switch (parts[0])
            {
                case "v":
                    vertices.Add(new Vector3(
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3])));
                    break;
                case "vt":
                    uvs.Add(new Vector2(
                        float.Parse(parts[1]),
                        float.Parse(parts[2])));
                    break;
                case "vn":
                    normals.Add(new Vector3(
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3])));
                    break;
                case "f":
                    List<int> faceVertices = new List<int>();
                    List<int> faceUVs = new List<int>();
                    List<int> faceNormals = new List<int>();

                    for (int i = 1; i < parts.Length; i++)
                    {
                        string[] indices = parts[i].Split('/');
                        faceVertices.Add(ParseIndex(indices[0], vertices.Count));
                        if (indices.Length > 1 && !string.IsNullOrEmpty(indices[1]))
                        {
                            faceUVs.Add(ParseIndex(indices[1], uvs.Count));
                        }
                        if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2]))
                        {
                            faceNormals.Add(ParseIndex(indices[2], normals.Count));
                        }
                    }

                    // Triangulate the face if it has more than 3 vertices
                    for (int i = 1; i < faceVertices.Count - 1; i++)
                    {
                        vertexIndices.Add(faceVertices[0]);
                        vertexIndices.Add(faceVertices[i]);
                        vertexIndices.Add(faceVertices[i + 1]);

                        if (faceUVs.Count > 0)
                        {
                            uvIndices.Add(faceUVs[0]);
                            uvIndices.Add(faceUVs[i]);
                            uvIndices.Add(faceUVs[i + 1]);
                        }

                        if (faceNormals.Count > 0)
                        {
                            normalIndices.Add(faceNormals[0]);
                            normalIndices.Add(faceNormals[i]);
                            normalIndices.Add(faceNormals[i + 1]);
                        }
                    }
                    break;
            }
        }

        Mesh mesh = new Mesh();
        Vector3[] meshVertices = new Vector3[vertexIndices.Count];
        Vector2[] meshUVs = new Vector2[vertexIndices.Count];
        Vector3[] meshNormals = new Vector3[vertexIndices.Count];

        for (int i = 0; i < vertexIndices.Count; i++)
        {
            meshVertices[i] = vertices[vertexIndices[i]];
            if (uvIndices.Count > i) meshUVs[i] = uvs[uvIndices[i]];
            if (normalIndices.Count > i) meshNormals[i] = normals[normalIndices[i]];
        }

        mesh.vertices = meshVertices;
        if (uvIndices.Count > 0) mesh.uv = meshUVs;
        if (normalIndices.Count > 0) mesh.normals = meshNormals;

        int[] triangles = new int[vertexIndices.Count];
        for (int i = 0; i < vertexIndices.Count; i++)
        {
            triangles[i] = i;
        }
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GameObject obj = new GameObject("LoadedOBJ");
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }

    private int ParseIndex(string indexString, int count)
    {
        int index = int.Parse(indexString);
        return index > 0 ? index - 1 : count + index;
    }
}
