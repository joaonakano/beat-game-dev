using UnityEngine;

public class BarAimBot : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Note"))
        {
            other.gameObject.SetActive(false);
            ScoreManager.Hit();
        }
    }
}
