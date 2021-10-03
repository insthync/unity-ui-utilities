using UnityEngine;

[RequireComponent(typeof(UIFollowWorldPosition))]
public class UIFollowWorldObject : MonoBehaviour
{
    public Transform targetObject;
    private UIFollowWorldPosition cachePositionFollower;
    public UIFollowWorldPosition CachePositionFollower
    {
        get
        {
            if (cachePositionFollower == null)
                cachePositionFollower = GetComponent<UIFollowWorldPosition>();
            return cachePositionFollower;
        }
    }

    private void Awake()
    {
        // Set target position to hidden position
        CachePositionFollower.targetPosition = Camera.main.transform.position - (Vector3.up * 10000f);
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (targetObject == null)
            return;
        CachePositionFollower.targetPosition = targetObject.transform.position;
    }
}
