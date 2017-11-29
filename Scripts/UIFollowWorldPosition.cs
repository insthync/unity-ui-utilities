using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIFollowWorldPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform tempTransform;
    public RectTransform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<RectTransform>();
            return tempTransform;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
        TempTransform.position = pos;
    }
}
