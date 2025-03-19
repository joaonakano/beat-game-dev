using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(roadSection, new Vector3(0, 0, (float)38.05), Quaternion.identity);
        }
    }
}
