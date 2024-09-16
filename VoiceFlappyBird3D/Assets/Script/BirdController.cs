using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float jumpValue = 5f;      // Varsayılan zıplama değeri
    [SerializeField] private float rotationSpeed = 5f;  // Kuşun dönme hızı
    [SerializeField] private string microphoneName;     // Mikrofon cihazının adı
    [SerializeField] private float minJumpValue = 2f;   // Minimum zıplama değeri
    [SerializeField] private float maxJumpValue = 10f;  // Maksimum zıplama değeri
    [SerializeField] private float loudnessThreshold = 0.1f; // Zıplama için gereken minimum ses seviyesi

    private Rigidbody rb;
    private AudioClip microphoneInput;
    private bool micInitialized = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            microphoneInput = Microphone.Start(microphoneName, true, 10, AudioSettings.outputSampleRate);
            micInitialized = true;
        }
        else
        {
            Debug.LogError("Mikrofon bulunamadı!");
        }
    }

    private void Update()
    {
        if (micInitialized)
        {
            float loudness = GetLoudnessFromMicrophone();

            if (loudness > loudnessThreshold)
            {
                float scaledJumpValue = Mathf.Lerp(minJumpValue, maxJumpValue, loudness); // Ses seviyesine göre zıplama değerini ölçekle
                rb.velocity = Vector3.up * scaledJumpValue;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.RestartGame();
    }

    private float GetLoudnessFromMicrophone()
    {
        int sampleSize = 128;
        float[] samples = new float[sampleSize];
        int startPosition = Microphone.GetPosition(microphoneName) - sampleSize + 1;

        if (startPosition < 0) return 0;

        microphoneInput.GetData(samples, startPosition);

        float sum = 0;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        return sum / sampleSize;
    }
}
