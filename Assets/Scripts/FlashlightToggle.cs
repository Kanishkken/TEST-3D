using UnityEngine;
using UnityEngine.Rendering;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlightLIGHT;
    private bool isOn = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    protected void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLIGHT.enabled = isOn;
    }
}
