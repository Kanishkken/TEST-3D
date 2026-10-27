using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    //public GameObject Note;
    //public bool isOn = false;
    public void Interact()
    {
        NoteViewer noteViewer = FindAnyObjectByType<NoteViewer>();


        noteViewer.ViewNote();


    }
}
