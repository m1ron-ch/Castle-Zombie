using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource; 
    [SerializeField] private AudioSource _runningSource;
    [SerializeField] private AudioSource _personGunSource;
    [SerializeField] private AudioSource _chopSource;
    [SerializeField] private AudioSource _addResource;
    [SerializeField] private AudioSource _takeResource;

    private static SoundManager s_instance;

    public static SoundManager Instance => s_instance ?? null;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PlayMusic();

        DontDestroyOnLoad(gameObject);
    }

    #region Play sound
    public void PlayRunning()
    {
        return;

        if (!_runningSource.isPlaying)
            _runningSource.Play();
    }

    public void PlayMusic()
    {
        _musicSource.Play();
    }

    public void PlayPersonGun()
    {
        if (!_personGunSource.isPlaying)
            _personGunSource.Play();
    }

    public void PlayChop()
    {
        _chopSource.Play();
    }

    public void PlayAddResource()
    {
        if (!_addResource.isPlaying)
            _addResource.Play();
    }

    public void PlayTakeResource()
    {
        _takeResource.Play();
    }

    #endregion

    #region Stop Sound

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopRunning()
    {
        _runningSource.Stop();
    }
    #endregion
}
