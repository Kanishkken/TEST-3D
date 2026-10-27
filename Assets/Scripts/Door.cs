using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    [SerializeField]private bool isLocked = true;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    public void Interact()
    {
        if (isLocked)
        {
            PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>();

            if (inventory.hasKey)
            {
                isLocked = false;
                Debug.Log("DOOR UNLOCKED");
            }
            else
            {
                Debug.Log("DOOR IS LOCKED");
                return;
            }
        }
        isOpen = !isOpen;
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.rotation =  Quaternion.Slerp(transform.rotation, targetRotation, openSpeed*Time.deltaTime);
    }
}
