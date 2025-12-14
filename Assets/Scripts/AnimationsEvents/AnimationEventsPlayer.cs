using Cinemachine;
using UnityEngine;

public class AnimationEventsPlayer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    
    private float rayDistance = 500f;
    private LayerMask rayMask = ~0;  

    public void EnableCollider()
    {
        GameObject.Find("Punch").GetComponent<SphereCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        GameObject.Find("Punch").GetComponent<SphereCollider>().enabled = false;
    }

    public void Throw()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        Vector3 direction;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
            direction = (targetPoint - arrowSpawnPoint.position).normalized;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * rayDistance;
            direction = (targetPoint - arrowSpawnPoint.position).normalized;
        }

        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(arrowPrefab, arrowSpawnPoint.position, rot);


    }

}
