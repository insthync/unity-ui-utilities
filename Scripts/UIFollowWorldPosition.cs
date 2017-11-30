using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIFollowWorldPosition : MonoBehaviour
{
    public readonly float ShowDelay = 0.2f;
    public Vector3 targetPosition;
    private float awakeTime;
    private bool alreadyShown;
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

    private void Awake()
    {
        awakeTime = Time.unscaledTime;
        TempTransform.EnableComponentsInChildren<Graphic>(false);
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        if (Time.unscaledTime - awakeTime >= ShowDelay && !alreadyShown)
        {
            TempTransform.EnableComponentsInChildren<Graphic>(true);
            alreadyShown = true;
        }
    }

    public void UpdatePosition()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
        TempTransform.position = pos;
    }
}
