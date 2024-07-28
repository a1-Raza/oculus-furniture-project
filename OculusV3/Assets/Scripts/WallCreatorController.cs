using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreatorController : MonoBehaviour
{
    [SerializeField] bool wallEditModeEnabled = false;
    [SerializeField] GameObject floorPlane;
    [SerializeField] GameObject wallCornerAnchorPrefab;
    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] List<GameObject> wallAnchors; //debug serialize
    [SerializeField] List<GameObject> anchorLines; //debug serialize
    [SerializeField] List<GameObject> generatedWalls; //debug serialize

    WallGenerator wallGenerator;

    // Start is called before the first frame update
    void Start()
    {
        wallGenerator = GetComponent<WallGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        ClearDeletedAnchors();

        if (wallEditModeEnabled)
        {
            if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
            {
                Vector3 controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
                AddWallAnchor(controllerPos);
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                Vector3 controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);
                AddWallAnchor(controllerPos);
            }

            if (OVRInput.GetUp(OVRInput.RawButton.Y))
            {
                ToggleWallsAndFloor();
            }

            UpdateAnchorLines();
        }

    }

    void ToggleWallsAndFloor()
    {
        floorPlane.SetActive(!floorPlane.activeSelf);
        foreach (GameObject wall in generatedWalls) { Destroy(wall); }
        generatedWalls.Clear();
        if (floorPlane.activeSelf) generatedWalls = wallGenerator.GenerateWalls(wallAnchors, wallPrefab);
        //SetWallAnchorsAndLinesActive(!floorPlane.activeSelf);
    }

    void SetWallAnchorsAndLinesActive(bool tf)
    {
        for (int i = 0; i < wallAnchors.Count; i++)
        {
            wallAnchors[i].SetActive(tf);
            if (i < anchorLines.Count) anchorLines[i].SetActive(tf);
        }
    }

    void AddWallAnchor(Vector3 controllerPos)
    {
        GameObject newAnchor = Instantiate(wallCornerAnchorPrefab, controllerPos, Quaternion.identity, transform);
        wallAnchors.Add(newAnchor);
        if (wallAnchors.Count >= 2)
        {
            GameObject newLine = Instantiate(lineRendererPrefab, transform);
            LineRenderer lr = newLine.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            anchorLines.Add(newLine);
        }
    }

    void UpdateAnchorLines()
    {
        for (int i = 0; i < wallAnchors.Count-1; i++) 
        {
            LineRenderer lr = anchorLines[i].GetComponent<LineRenderer>();
            lr.SetPosition(0, wallAnchors[i].transform.position);
            lr.SetPosition(1, wallAnchors[i+1].transform.position);
        }
    }

    void ClearDeletedAnchors()
    {
        int linesToDelete = 0;
        if (wallAnchors.Count > 0)
        {
            for (int i = wallAnchors.Count - 1; i >= 0; i--)
            {
                if (wallAnchors[i] == null)
                {
                    wallAnchors.RemoveAt(i);
                    linesToDelete++;
                }
            }
        }
        if (linesToDelete > 0)
        {
            for (int i = 0; i < linesToDelete; i++)
            {
                GameObject line = anchorLines[anchorLines.Count - 1 - i];
                anchorLines.RemoveAt(anchorLines.Count - 1 - i);
                Destroy(line);
            }
        }
    }
}
