using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUIExtension : MonoBehaviour
{
    public UIBase ui;
    public abstract void Show();
    public abstract void Hide();
    public abstract bool IsVisible();
}
