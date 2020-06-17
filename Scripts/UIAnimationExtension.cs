using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class UIAnimationExtension : BaseUIExtension
{
    public string animShowParam = "Show";
    private bool isShowing;
    private Animation cacheAnimation;
    public Animation CacheAnimation
    {
        get
        {
            if (cacheAnimation == null)
                cacheAnimation = GetComponent<Animation>();
            return cacheAnimation;
        }
    }

    public override void Show()
    {
        isShowing = true;
        ui.root.SetActive(true);
        CacheAnimation[animShowParam].normalizedTime = 0f;
        CacheAnimation[animShowParam].speed = 1f;
        CacheAnimation.Play(animShowParam, PlayMode.StopAll);
    }

    public override void Hide()
    {
        isShowing = false;
        CacheAnimation[animShowParam].normalizedTime = 1f;
        CacheAnimation[animShowParam].speed = -1f;
        CacheAnimation.Play(animShowParam, PlayMode.StopAll);
    }

    public override bool IsVisible()
    {
        return isShowing;
    }
}
