using System;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;

    public GameObject interactionPrompt;

    private IInteractable currentInteractable;

    private void Update()
    {
        NoteViewer noteViewer = FindAnyObjectByType<NoteViewer>();

        if (noteViewer.isViewingNote)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                noteViewer.CloseNote();
            }

            return;
        }
            
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }

           
        }
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;
        interactionPrompt.SetActive(false);

        Ray ray = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
            if(currentInteractable != null )
            {
                interactionPrompt.SetActive(true);
            }
        }
    }
}
