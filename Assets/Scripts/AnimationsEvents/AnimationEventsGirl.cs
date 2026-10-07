using UnityEngine;

public class AnimationEventsGirl : MonoBehaviour
{
    public void Hold()
    {
        Prosos.instance.sword.SetParent(Prosos.instance.rightHandPoint);
    }

    public void UnHold()
    {
        Prosos.instance.sword.SetParent(Prosos.instance.swordPoint, worldPositionStays: false);
        Prosos.instance.sword.transform.localPosition = Vector3.zero;
        Prosos.instance.sword.transform.localRotation = Quaternion.identity;
    }

    public void EnableCollider()
    {
        Prosos.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        Prosos.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }
}
