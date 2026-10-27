    using UnityEngine;

    public class InteractablePickup : MonoBehaviour, IInteractable
    {

        public ItemType itemType;



        

        public void Interact()
        {
            Debug.Log("PICKED UP" + itemType);

            PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>(); 

            inventory.AddItem(itemType);

            Destroy(gameObject);
        }
    }
