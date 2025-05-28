using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject InGameMenuObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGameMenuObject.activeSelf)
            {
                InGameMenuObject.SetActive(false);
                SongManager.Instance.UnpauseSong();
            }
            else
            {
                SongManager.Instance.PauseSong();
                InGameMenuObject.SetActive(true);
            }
        }
    }
}
