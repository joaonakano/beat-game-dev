using UnityEngine;
using MilkShake;

public class ShakingCamera : MonoBehaviour
{
    //Inspector field for the Shaker component.
    public Shaker MyShaker;
    //Inspector field for a Shake Preset to use as the shake parameters.
    public ShakePreset ShakePreset;

    //The saved shake instance we will be modifying
    private ShakeInstance myShakeInstance;

    private void Start()
    {
        //The Shake method returns a Shake Instance that you can save and modify.
        myShakeInstance = Shaker.ShakeAll(ShakePreset);
    }

    private void Update()
    {
        //Start the shake, with a fade-in time of 1 second.
        if (Input.GetKeyDown(KeyCode.Q))
            myShakeInstance.Start(1f);

        //Stop the shake, with a fade-out time of 1 second. Also make sure the shake is not removed by the Shaker once it is fully stopped.
        if (Input.GetKeyDown(KeyCode.E))
            myShakeInstance.Stop(1f, false);

        //Pause or unpause the shake, with a fade-out time of 1 second. Pausing will stop the shake from moving, but keep the offset position and rotation.
        if (Input.GetKeyDown(KeyCode.P))
            myShakeInstance.TogglePaused(1f);

        //Increase the shake strength when holding the Plus key on the numpad.
        if (Input.GetKey(KeyCode.KeypadPlus))
            myShakeInstance.StrengthScale += Time.deltaTime;

        //Decrease the shake strength when holding the Minus key on the numpad.
        if (Input.GetKey(KeyCode.KeypadMinus))
            myShakeInstance.StrengthScale -= Time.deltaTime;
    }
}