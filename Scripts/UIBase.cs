using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public GameObject root;
    public bool hideOnAwake;
    public UnityEvent eventShow;
    public UnityEvent eventHide;
    private bool isAwaken;

    protected virtual void Awake()
    {
        if (isAwaken)
            return;
        isAwaken = true;
        ValidateRoot();
        if (hideOnAwake)
            Hide();
        else
            Show();
    }

    public void ValidateRoot()
    {
        if (root == null)
            root = gameObject;
    }

    public virtual void Show()
    {
        isAwaken = true;
        ValidateRoot();
        root.SetActive(true);
        eventShow.Invoke();
    }

    public virtual void Hide()
    {
        isAwaken = true;
        ValidateRoot();
        root.SetActive(false);
        eventHide.Invoke();
    }

    public virtual bool IsVisible()
    {
        ValidateRoot();
        return root.activeSelf;
    }

    public void SetEnableGraphics(bool isEnable)
    {
        var graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = isEnable;
        }
    }

    public void SetGraphicsAlpha(float alpha)
    {
        var graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            var color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            graphic.color = color;
        }
    }
}
