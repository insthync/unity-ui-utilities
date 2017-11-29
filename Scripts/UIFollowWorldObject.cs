using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIFollowWorldPosition))]
public class UIFollowWorldObject : MonoBehaviour
{
    public GameObject targetObject;
    private UIFollowWorldPosition tempPositionFollower;
    public UIFollowWorldPosition TempPositionFollower
    {
        get
        {
            if (tempPositionFollower == null)
                tempPositionFollower = new UIFollowWorldPosition();
            return tempPositionFollower;
        }
    }

    private void FixedUpdate()
    {
        TempPositionFollower.targetPosition = targetObject.transform.position;
    }
}
