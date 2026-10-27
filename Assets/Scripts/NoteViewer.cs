using UnityEngine;

public class NoteViewer : MonoBehaviour
{
    public GameObject heldNote;
    public bool isViewingNote = false;
    public void ViewNote()
    {
        heldNote.SetActive(true);
        isViewingNote = true;
    }
    public void CloseNote()
    {
        heldNote.SetActive(false);
        isViewingNote = false;
    }

    //private void Update()
    //{
    //    if(isViewingNote == true)
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
                
    //            //CloseNote();
    //        }
    //    }
    //}
}
