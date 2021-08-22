using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class UIList : UIBase
{
    public int SelectedAmount { get; protected set; }
    [Header("Selection options/configs")]
    [FormerlySerializedAs("selectable")]
    public UIDataItemSelectionMode selectionMode;
    public int limitSelection;
    public UIDataItemEvent eventSelect;
    public UIDataItemEvent eventDeselect;
    public UnityEvent eventSelectionChange;
}

public abstract class UIList<T> : UIList
    where T : MonoBehaviour
{
    public readonly Dictionary<string, T> UIEntries = new Dictionary<string, T>();

    [Header("Generic Elements")]
    public GameObject emptyInfoObject;
    public Transform container;
    public T itemPrefab;

    protected virtual void Update()
    {
        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(UIEntries.Count == 0);
    }

    public virtual T SetListItem(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;
        if (UIEntries.ContainsKey(id))
            return UIEntries[id];

        var newItemObject = Instantiate(itemPrefab.gameObject);
        newItemObject.transform.SetParent(container);
        newItemObject.transform.localScale = Vector3.one;
        newItemObject.SetActive(true);

        var newItem = newItemObject.GetComponent<T>();
        UIEntries.Add(id, newItem);
        return newItem;
    }

    public virtual T GetListItem(string id)
    {
        if (UIEntries.ContainsKey(id))
            return UIEntries[id];
        return null;
    }

    public virtual bool RemoveListItem(string id)
    {
        if (UIEntries.ContainsKey(id))
        {
            var item = UIEntries[id];
            if (UIEntries.Remove(id))
            {
                Destroy(item.gameObject);
                return true;
            }
        }
        return false;
    }

    public virtual void ClearListItems()
    {
        var values = new List<T>(UIEntries.Values);
        for (var i = values.Count - 1; i >= 0; --i)
        {
            var item = values[i];
            Destroy(item.gameObject);
        }
        UIEntries.Clear();
    }
}

public abstract class UIDataItemList<TUIDataItem, TUIDataItemType> : UIList<TUIDataItem>
    where TUIDataItem : UIDataItem<TUIDataItemType>
{
    protected bool isDirtySelection;
    protected readonly List<TUIDataItem> selectedUIList = new List<TUIDataItem>();
    protected readonly List<TUIDataItemType> selectedDataList = new List<TUIDataItemType>();
    protected readonly List<string> selectedIdList = new List<string>();

    public override TUIDataItem SetListItem(string id)
    {
        var newItem = base.SetListItem(id);
        if (newItem != null)
        {
            newItem.list = this;
            newItem.selectionMode = selectionMode;
            if (selectionMode != UIDataItemSelectionMode.Disable)
            {
                newItem.eventSelect.RemoveListener(OnSelect);
                newItem.eventSelect.AddListener(OnSelect);
                newItem.eventDeselect.RemoveListener(OnDeselect);
                newItem.eventDeselect.AddListener(OnDeselect);
            }
        }
        return newItem;
    }

    public override bool RemoveListItem(string id)
    {
        isDirtySelection = true;
        return base.RemoveListItem(id);
    }

    public override void ClearListItems()
    {
        isDirtySelection = true;
        base.ClearListItems();
    }

    public void DeselectedItems(UIDataItem exceptUI)
    {
        var items = UIEntries;
        foreach (var keyValuePair in items)
        {
            var item = keyValuePair.Value;
            if (item == exceptUI)
                continue;
            item.Deselect(false);
        }
    }

    protected virtual void OnSelect(UIDataItem ui)
    {
        isDirtySelection = true;
        if (selectionMode != UIDataItemSelectionMode.MultipleToggle)
            DeselectedItems(ui);
        eventSelect.Invoke(ui);
    }

    protected virtual void OnDeselect(UIDataItem ui)
    {
        isDirtySelection = true;
        eventDeselect.Invoke(ui);
    }

    protected void MakeSelectedLists(bool forceRebuild = false)
    {
        if (isDirtySelection || forceRebuild)
        {
            ClearSelectedLists();
            var items = UIEntries;
            foreach (var keyValuePair in items)
            {
                var id = keyValuePair.Key;
                var uiEntry = keyValuePair.Value;

                if (uiEntry.Selected)
                    MakeSelectedList(id, uiEntry);
            }
            isDirtySelection = false;
        }
    }

    protected virtual void MakeSelectedList(string id, TUIDataItem uiEntry)
    {
        ++SelectedAmount;
        selectedUIList.Add(uiEntry);
        selectedDataList.Add(uiEntry.data);
        selectedIdList.Add(id);
    }

    protected virtual void ClearSelectedLists()
    {
        SelectedAmount = 0;
        selectedUIList.Clear();
        selectedDataList.Clear();
        selectedIdList.Clear();
    }

    public List<TUIDataItem> GetSelectedUIList(bool forceRebuild = false)
    {
        MakeSelectedLists(forceRebuild);
        return selectedUIList;
    }

    public List<TUIDataItemType> GetSelectedDataList(bool forceRebuild = false)
    {
        MakeSelectedLists(forceRebuild);
        return selectedDataList;
    }

    public List<string> GetSelectedIdList(bool forceRebuild = false)
    {
        MakeSelectedLists(forceRebuild);
        return selectedIdList;
    }
}
