using UnityEngine;
using System.Collections;

public class PitchController : MonoBehaviour
{
    private AudioSource audioSource;
    private float basePitch = 1.0f;
    private float semitoneRatio = Mathf.Pow(2, 1.0f / 12.0f);
    public float pitchChangeSpeed = 20.0f; // Speed of pitch change (you can adjust this value)
    public float volumeRiseTime = 0.05f; // Time to reach max volume
    public float volumeFallTime = 0.5f; // Time to reach min volume
    private Coroutine volumeCoroutine;
    private int keysPressed = 0;

    private float targetPitch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = basePitch;
        audioSource.volume = 0;
        targetPitch = basePitch; // Initialize target pitch to base pitch
    }

    void Update()
    {
        HandleKeyPress(KeyCode.A, 0);
        HandleKeyPress(KeyCode.S, 1);
        HandleKeyPress(KeyCode.D, 2);
        HandleKeyPress(KeyCode.F, 3);
        HandleKeyPress(KeyCode.G, 4);
        HandleKeyPress(KeyCode.H, 5);
        HandleKeyPress(KeyCode.J, 6);
        HandleKeyPress(KeyCode.K, 7);
        HandleKeyPress(KeyCode.L, 8);
        HandleKeyPress(KeyCode.Semicolon, 9);

        // Slerp towards the target pitch
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchChangeSpeed);
    }

    void HandleKeyPress(KeyCode key, int semitoneChange)
    {
        if (Input.GetKeyDown(key))
        {
            keysPressed++;
            targetPitch = basePitch * Mathf.Pow(semitoneRatio, semitoneChange);
            StartVolumeFade(CalculateMaxVolume(targetPitch), volumeRiseTime);
        }
        if (Input.GetKeyUp(key))
        {
            keysPressed--;
            if (keysPressed <= 0)
            {
                StartVolumeFade(0, volumeFallTime);
                keysPressed = 0; // Reset to ensure it doesn't go negative
            }
        }
    }

    void StartVolumeFade(float targetVolume, float duration)
    {
        if (volumeCoroutine != null)
        {
            StopCoroutine(volumeCoroutine);
        }
        volumeCoroutine = StartCoroutine(FadeVolume(targetVolume, duration));
    }

    IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    float CalculateMaxVolume(float pitch)
    {
        // Adjust max volume based on pitch (higher pitch = lower volume)
        return Mathf.Clamp(1.0f - (pitch - basePitch) / 10.0f, 0.1f, 1.0f);
    }
}
