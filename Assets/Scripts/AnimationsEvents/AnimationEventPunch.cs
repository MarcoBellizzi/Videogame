using UnityEngine;

public class AnimationEventPunch : MonoBehaviour
{
    public void EnableCollider()
    {
        GameObject.Find("Punch").GetComponent<SphereCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        GameObject.Find("Punch").GetComponent<SphereCollider>().enabled = false;
    }
}
