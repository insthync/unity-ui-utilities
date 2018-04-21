using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatorExtension : BaseUIExtension
{
    public string animShowParam = "Show";
    private Animator cacheAnimator;
    public Animator CacheAnimator
    {
        get
        {
            if (cacheAnimator == null)
                cacheAnimator = GetComponent<Animator>();
            return cacheAnimator;
        }
    }

    public override void Show()
    {
        CacheAnimator.SetBool(animShowParam, true);
    }

    public override void Hide()
    {
        CacheAnimator.SetBool(animShowParam, false);
    }

    public override bool IsVisible()
    {
        return CacheAnimator.GetBool(animShowParam);
    }
}
