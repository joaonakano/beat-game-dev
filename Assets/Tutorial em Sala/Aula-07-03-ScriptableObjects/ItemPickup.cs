using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)     // Errei o "private void OnTriggerEnter(Collider other)" por "public void PickupItem(Collider other)"
    {
        if (other.CompareTag("Player")) {
            Inventory inventory = other.GetComponent<Inventory>();  // Esqueci de como pegar o invent�rio do personagem que interagiu com o ScriptableObject, mas � "other.GetComponent<Inventory>();"

            if (inventory != null)  // Esqueci do "inventory != null"
            {
                Debug.Log($"Voc� pegou o item '{itemData.itemName}' e ele foi armazenado no invent�rio!");
                Destroy(gameObject);
                inventory.AddItem(itemData);
            } else
            {
                Debug.LogWarning("O personagem que interagiu com o objeto n�o tem o Inventory3D!");
            }
        }
    }
}
