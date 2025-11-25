using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip flipSfx;
    public AudioClip matchSfx;
    public AudioClip mismatchSfx;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFlip()
    {
        if (flipSfx != null)
            audioSource.PlayOneShot(flipSfx);
    }

    public void PlayMatch()
    {
        if (matchSfx != null)
            audioSource.PlayOneShot(matchSfx);
    }

    public void PlayMismatch()
    {
        if (mismatchSfx != null)
            audioSource.PlayOneShot(mismatchSfx);
    }
}
