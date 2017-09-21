using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIList<T> : UIBase where T : MonoBehaviour
{
    public readonly Dictionary<string, T> Items = new Dictionary<string, T>();
    public Transform container;
    public T itemPrefab;

    public virtual T SetItem(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;
        if (Items.ContainsKey(id))
            return Items[id];
        var newItemObject = Instantiate(itemPrefab.gameObject);
        var newItem = newItemObject.GetComponent<T>();
        newItemObject.transform.SetParent(container);
        newItemObject.transform.localScale = Vector3.one;
        newItemObject.SetActive(true);
        Items.Add(id, newItem);
        return newItem;
    }

    public virtual bool RemoveItem(string id)
    {
        if (Items.ContainsKey(id))
        {
            var item = Items[id];
            if (Items.Remove(id))
            {
                Destroy(item.gameObject);
                return true;
            }
        }
        return false;
    }

    public virtual void ClearItems()
    {
        foreach (var item in Items.Values)
        {
            Destroy(item.gameObject);
        }
        Items.Clear();
    }
}
