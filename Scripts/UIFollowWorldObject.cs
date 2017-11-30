using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIFollowWorldPosition))]
public class UIFollowWorldObject : MonoBehaviour
{
    public Transform targetObject;
    private UIFollowWorldPosition tempPositionFollower;
    public UIFollowWorldPosition TempPositionFollower
    {
        get
        {
            if (tempPositionFollower == null)
                tempPositionFollower = GetComponent<UIFollowWorldPosition>();
            return tempPositionFollower;
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (targetObject == null)
            return;

        TempPositionFollower.targetPosition = targetObject.transform.position;
    }
}
