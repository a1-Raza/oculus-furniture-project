using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPlacerController : MonoBehaviour
{
    [SerializeField] bool itemPlaceMode = true; // serialized for debug

    [SerializeField] Transform centerEyeTransform;
    [SerializeField] Transform leftRaycasterCursorTransform;

    [SerializeField] List<GameObject> defaultItems;
    int itemsIndex = 0;
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.Y)) itemPlaceMode = !itemPlaceMode;

        if (!itemPlaceMode) text.text = "Placing is Off\n(Y to On)";
        else text.text = "Item:\n" + defaultItems[itemsIndex].name;

        if (!itemPlaceMode || defaultItems.Count == 0) return;

        if (OVRInput.GetUp(OVRInput.RawButton.LThumbstickUp))
        {
            if (itemsIndex < defaultItems.Count - 1) itemsIndex++;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.LThumbstickDown))
        {
            if (itemsIndex > 0) itemsIndex--;
        }

        GameObject selectedItem = defaultItems[itemsIndex];

        if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            Instantiate(selectedItem, new Vector3(leftRaycasterCursorTransform.position.x, 0, leftRaycasterCursorTransform.position.z), Quaternion.identity);
        }

        
    }
}
