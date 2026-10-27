using System.Xml;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasFlashlight;
    public bool hasKey;
    public bool hasNote;

    public void AddItem(ItemType item)
    {
        switch(item)
        {
            case ItemType.Flashlight:
                hasFlashlight = true; 

                PlayerEquipment equipment = FindAnyObjectByType<PlayerEquipment>();

                equipment.EquipFlashlight();
                break;

            case ItemType.Key:
                hasKey = true;

                break;

           
        }
    }
}
