using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour
{
    private Player player;

    [Header("Stats")]
    public float healthPoints = 100f;

    void Start()
    {
        player = GetComponent<Player>();
        healthPoints = 100f;
    }

    void Update()
    {
        HandleDebugInput();
    }

    void HandleDebugInput()
    {
        // Tasti debug per testare danni
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetHit(10f);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (PanelHeathBars.instance != null)
                PanelHeathBars.instance.enemyHealtPoints -= 10f;
        }
    }

    public void GetHit(float damage)
    {
        if (!player.canGetHit) return;

        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Die();
        }
        else
        {
            player.animator.SetTrigger("getHit");
        }
    }

    private void Die()
    {
        player.animator.SetTrigger("death");
        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(6f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }
}