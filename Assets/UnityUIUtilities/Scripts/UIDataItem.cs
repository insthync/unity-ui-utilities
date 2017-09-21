using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIDataItemClickMode
{
    Default,
    Selection,
    Disable,
}

public abstract class UIDataItem<T> : UIBase where T : class
{
    // Decoration
    public bool selected;
    private bool dirtySelected;
    public GameObject selectedObject;
    // Events
    public UIDataItemClickMode clickMode = UIDataItemClickMode.Default;
    public System.Action<UIDataItem<T>> eventClick;
    public System.Action<UIDataItem<T>> eventSelect;
    public System.Action<UIDataItem<T>> eventDeselect;
    // Data
    public T data;
    private T dirtyData;

    public virtual bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    public bool IsDirty()
    {
        return data != dirtyData;
    }

    private void Update()
    {
        if (IsDirty())
            UpdateLogic();

        if (Selected != dirtySelected)
        {
            if (selectedObject != null)
                selectedObject.SetActive(Selected);
            dirtySelected = Selected;
        }
    }

    private void UpdateLogic()
    {
        Clear();
        UpdateData();
        dirtyData = data;
    }

    public void ForceUpdate()
    {
        UpdateLogic();
    }

    public virtual void OnClick()
    {
        switch (clickMode)
        {
            case UIDataItemClickMode.Selection:
                Selected = !Selected;
                if (Selected)
                    Select();
                else
                    Deselect();
                break;
            case UIDataItemClickMode.Default:
                Click();
                break;
        }
    }

    public virtual void Select(bool invokeEvent = true)
    {
        Selected = true;
        if (invokeEvent)
            eventSelect(this);
    }

    public virtual void Deselect(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventDeselect(this);
    }

    public virtual void Click(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventClick(this);
    }

    public abstract void UpdateData();
    public abstract void Clear();
}
