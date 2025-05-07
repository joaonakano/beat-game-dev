using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject InGameMenuObject;
    //private bool isMenuBeingShown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGameMenuObject.activeSelf)
            {
                InGameMenuObject.SetActive(false);
                //isMenuBeingShown = true;
            }
            else
            {
                SongManager.Instance.PauseSong();
                InGameMenuObject.SetActive(true);
            }
        }
    }
}
