using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public GameObject heldFlashLight;
    public GameObject heldNote;

    public void EquipFlashlight()
    {
        heldFlashLight.SetActive(true);
    }

    //public void ViewNote()
    //{
    //    heldNote.SetActive(true);
    //}
}

