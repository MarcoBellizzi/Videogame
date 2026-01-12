using UnityEngine;

public class AnimationEventsGirl : MonoBehaviour
{
    public void Hold()
    {
        Girl.instance.sword.SetParent(Girl.instance.rightHandPoint);
    }

    public void UnHold()
    {
        Girl.instance.sword.SetParent(Girl.instance.swordPoint, worldPositionStays: false);
        Girl.instance.sword.transform.localPosition = Vector3.zero;
        Girl.instance.sword.transform.localRotation = Quaternion.identity;
    }
}
