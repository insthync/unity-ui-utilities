using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIList<T> : UIBase where T : MonoBehaviour
{
    public readonly Dictionary<string, T> Items = new Dictionary<string, T>();
    public GameObject emptyInfoObject;
    public Transform container;
    public T itemPrefab;

    private void Update()
    {
        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(Items.Count == 0);
    }

    public virtual T SetListItem(string id)
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

    public virtual bool RemoveListItem(string id)
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

    public virtual void ClearListItems()
    {
        var values = new List<T>(Items.Values);
        for (var i = values.Count - 1; i >= 0; --i)
        {
            var item = values[i];
            Destroy(item.gameObject);
        }
        Items.Clear();
    }
}
