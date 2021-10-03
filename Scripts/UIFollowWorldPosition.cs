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
        CacheTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
    }
}
