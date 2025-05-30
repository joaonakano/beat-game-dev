using UnityEngine;
using MilkShake;

public class CameraBounce : MonoBehaviour
{
    public static void ShakeCamera(ShakePreset preset, Shaker shaker)
    {
        shaker.Shake(preset);
    }
}