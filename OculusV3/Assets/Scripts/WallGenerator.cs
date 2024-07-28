using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GenerateWalls(List<GameObject> wallAnchors, GameObject wallPrefab)
    {
        List<GameObject> walls = new List<GameObject>();
        
        for (int i = 0; i < wallAnchors.Count - 1; i++) 
        {
            Vector3 pos1 = wallAnchors[i].transform.position;
            Vector3 pos2 = wallAnchors[i+1].transform.position;
            Vector3 wallCenter = (pos1 + pos2) / 2;
            float wallRotation = Vector3.Angle(Vector3.right, (pos2 - pos1).normalized);
            GameObject newWall = Instantiate(wallPrefab, wallCenter, Quaternion.identity);
            newWall.transform.localEulerAngles = new Vector3(0, wallRotation, 0);
            newWall.transform.localScale = new Vector3((pos2 - pos1).x/2, 1, 1);
            walls.Add(newWall);
        }

        return walls;
    }
}
