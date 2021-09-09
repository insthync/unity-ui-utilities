using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIFollowWorldPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform cacheTransform;
    public RectTransform CacheTransform
    {
        get
        {
            if (cacheTransform == null)
                cacheTransform = GetComponent<RectTransform>();
            return cacheTransform;
        }
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
        CacheTransform.position = pos;
    }
}
