using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        HandleWeaponToggle();
        HandleAttack();
        ResetAttackFlag();
    }

    void HandleWeaponToggle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!player.isHolding)
            {
                player.animator.SetTrigger("unsheathe");
                // Nota: La logica per cambiare 'isHolding' a true dovrebbe essere idealmente
                // chiamata da un Animation Event, ma per ora la gestiamo qui o nell'animator.
            }
            else
            {
                player.animator.SetTrigger("sheathe");
            }
        }
    }

    void HandleAttack()
    {
        if (player.isHolding && player.canAttack && Input.GetKeyDown(KeyCode.Mouse0))
        {
            player.animator.SetTrigger("attack");
        }
    }

    void ResetAttackFlag()
    {
        // Evita di tirare un pugno quando si chiude un menu
        if (player.canAttackNext)
        {
            player.canAttack = true;
            player.canAttackNext = false;
        }
    }
}