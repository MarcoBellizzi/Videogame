using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    [SerializeField] private AudioClip sottofondo;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.clip = sottofondo;
        source.Play();
    }

    public void PlayGame()
    {
        // Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
    
    // chude l'applicazione
    public void QuitGame()
    {
        Application.Quit();
    }
}
