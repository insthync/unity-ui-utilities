using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum UIDataItemSelectionMode
{
    Disable,
    Default,
    Toggle,
    MultipleToggle,
}

[System.Serializable]
public class UIDataItemEvent : UnityEvent<UIDataItem> { }

public abstract class UIDataItem : UIBase
{
    // Decoration
    public bool selected;
    protected bool dirtySelected;
    public GameObject selectedObject;
    public GameObject emptyInfoObject;
    // Events
    [FormerlySerializedAs("clickMode")]
    public UIDataItemSelectionMode selectionMode = UIDataItemSelectionMode.Default;
    public UIDataItemEvent eventSelect;
    public UIDataItemEvent eventDeselect;
    public UIDataItemEvent eventUpdate;
    public abstract object GetData();
    public abstract void SetData(object data);
    public abstract void ForceUpdate();
    [HideInInspector]
    public UIList list;

    public virtual bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }
}

public abstract class UIDataItem<T> : UIDataItem
{
    // Data
    public T data;
    protected T dirtyData;

    public bool IsDirty()
    {
        if (typeof(T).IsClass)
        {
            if ((data == null && dirtyData != null) || (data != null && dirtyData == null))
                return true;
            return data != null && !data.Equals(dirtyData);
        }
        return !data.Equals(dirtyData);
    }

    protected override void Awake()
    {
        base.Awake();
        if (selectedObject != null)
            selectedObject.SetActive(false);
        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (IsDirty())
            UpdateLogic();

        if (Selected != dirtySelected)
        {
            if (selectedObject != null)
                selectedObject.SetActive(Selected);
            dirtySelected = Selected;
        }

        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(IsEmpty());
    }

    private void UpdateLogic()
    {
        Clear();
        UpdateData();
        dirtyData = data;
        eventUpdate.Invoke(this);
    }

    public override sealed void ForceUpdate()
    {
        UpdateLogic();
    }

    public virtual void OnClick()
    {
        switch (selectionMode)
        {
            case UIDataItemSelectionMode.Toggle:
            case UIDataItemSelectionMode.MultipleToggle:
                if (list == null ||
                    list.limitSelection <= 0 ||
                    list.SelectedAmount < list.limitSelection ||
                    Selected)
                {
                    Selected = !Selected;
                    if (Selected)
                        Select();
                    else
                        Deselect();
                }
                break;
            case UIDataItemSelectionMode.Default:
                Click();
                break;
        }
    }

    public virtual void Select(bool invokeEvent = true)
    {
        Selected = true;
        if (invokeEvent)
            eventSelect.Invoke(this);
    }

    public virtual void Deselect(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventDeselect.Invoke(this);
    }

    public virtual void Click(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventSelect.Invoke(this);
    }

    public override object GetData()
    {
        return data;
    }

    public override void SetData(object newData)
    {
        data = (T)newData;
        ForceUpdate();
    }

    public void CopyDataFromUI(UIDataItem ui)
    {
        SetData(ui.GetData());
    }

    public abstract void UpdateData();
    public abstract void Clear();
    public abstract bool IsEmpty();
}
