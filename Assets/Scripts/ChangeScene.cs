using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string nextScene;

    [Header("Options")]
    [SerializeField] private bool loadOnlyOnce = true;
    [SerializeField] private float delay = 0f;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered && loadOnlyOnce) return;

        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;

            if (delay > 0f)
            {
                Invoke(nameof(LoadScene), delay);
            }
            else
            {
                LoadScene();
            }
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(nextScene))
        {
            GameManager.Instance.scena += 1;
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("ChangeScene: nome scena non assegnato!");
        }
    }
}
