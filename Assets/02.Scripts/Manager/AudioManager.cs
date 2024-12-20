using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singleton = new GameObject("AudioManager");
                instance = singleton.AddComponent<AudioManager>();
                DontDestroyOnLoad(singleton);
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioSource BGM;
    public AudioSource PlayerMove;
    public AudioSource PlayerAction;
    public AudioSource Zombie;
    public AudioResource[] PlayerMoveAudio;
    public AudioResource[] PlayerActionAudio;
    public AudioResource ZombieAudio;

    public void MoveSound(int value)
    {
        PlayerMove.resource = PlayerMoveAudio[value];
        if (PlayerMove.isPlaying) 
            return;
        else
            PlayerMove.Play();
    }
    public void ActionSound(int value)
    {
        PlayerAction.resource = PlayerActionAudio[value];
        PlayerAction.Play();
    }
    public void ZombieSound()
    {
        Zombie.resource = ZombieAudio;
        Zombie.Play();
    }
}
