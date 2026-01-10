using Cinemachine;
using UnityEngine;

public class AnimationEventsPlayer : MonoBehaviour
{
    // public GameObject arrowPrefab;
    // public Transform arrowSpawnPoint;
    
    // private float rayDistance = 500f;
    // private LayerMask rayMask = ~0;

    

    public void EnableCollider()
    {
        Player.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        Player.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }

    public void Hold()
    {
        Player.instance.sword.SetParent(Player.instance.rightHandPoint);
    }

    public void UnHold()
    {
        Player.instance.sword.SetParent(Player.instance.swordPoint, worldPositionStays: false);
        Player.instance.sword.transform.localPosition = Vector3.zero;
        Player.instance.sword.transform.localRotation = Quaternion.identity;
    }

    // public void Throw()
    // {
    //     Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    //     Vector3 targetPoint;
    //     Vector3 direction;

    //     if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore))
    //     {
    //         targetPoint = hit.point;
    //         direction = (targetPoint - arrowSpawnPoint.position).normalized;
    //     }
    //     else
    //     {
    //         targetPoint = ray.origin + ray.direction * rayDistance;
    //         direction = (targetPoint - arrowSpawnPoint.position).normalized;
    //     }

    //     Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
    //     Instantiate(arrowPrefab, arrowSpawnPoint.position, rot);
    // }

}
