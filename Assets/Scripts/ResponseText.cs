using UnityEngine;

public class ResponseText : MonoBehaviour
{
    Vector3 newPosition;

    private void Start()
    {
        float xPos = UnityEngine.Random.Range(-1f, 1f);
        float yPos = UnityEngine.Random.Range(-1f, 1f);
        float zPos = UnityEngine.Random.Range(-1f, 1f);
        newPosition = new Vector3(xPos, yPos, zPos);
    }

    void Update()
    {            
        transform.Translate(newPosition * Time.deltaTime);        
    }
}
