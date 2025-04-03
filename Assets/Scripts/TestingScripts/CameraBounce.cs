using UnityEngine;

public class CameraBounce : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float rotationAmplitude = 10f;
    [SerializeField] private float bounceFrequency = 2f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    [Header("Smooth Damp Settings")]
    [SerializeField] private float smoothTime = 0.3f;

    private float currentAngle;
    private float velocity;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation set in the Inspector
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate target angle using sine wave
        float targetAngle = rotationAmplitude * Mathf.Sin(Time.time * bounceFrequency);

        // Smoothly damp towards the target angle
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime, rotationSpeed);

        // Apply rotation relative to initial rotation
        transform.localRotation = initialRotation * Quaternion.Euler(rotationAxis * currentAngle);
    }
}