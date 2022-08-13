using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMessageDialog : UIBase
{
    public struct Data
    {
        public string title;
        public string content;
        public UnityAction actionYes;
        public UnityAction actionNo;
        public UnityAction actionCancel;
        public bool showButtonYes;
        public bool showButtonNo;
        public bool showButtonCancel;

        public Data(string title, string content, UnityAction actionYes = null, UnityAction actionNo = null, UnityAction actionCancel = null, bool showButtonYes = true, bool showButtonNo = true, bool showButtonCancel = true)
        {
            this.title = title;
            this.content = content;
            this.actionYes = actionYes;
            this.actionNo = actionNo;
            this.actionCancel = actionCancel;
            this.showButtonYes = showButtonYes;
            this.showButtonNo = showButtonNo;
            this.showButtonCancel = showButtonCancel;
        }
    }

    public Text textTitle;
    public Text titleContent;
    public Button buttonYes;
    public Button buttonNo;
    public Button buttonCancel;
    public UnityAction actionYes;
    public UnityAction actionNo;
    public UnityAction actionCancel;

    public string Title
    {
        get { return textTitle == null ? "" : textTitle.text; }
        set { if (textTitle != null) textTitle.text = value; }
    }

    public string Content
    {
        get { return titleContent == null ? "" : titleContent.text; }
        set { if (titleContent != null) titleContent.text = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        if (textTitle == null) Debug.LogWarning("`Text Title` for " + name + " has not been set");
        if (titleContent == null) Debug.LogWarning("`Text Content` for " + name + " has not been set");
    }

    public void SetData(Data data)
    {
        Title = data.title;
        Content = data.content;
        actionYes = data.actionYes;
        actionNo = data.actionNo;
        actionCancel = data.actionCancel;
        if (buttonYes != null)
            buttonYes.gameObject.SetActive(data.showButtonYes);
        if (buttonNo != null)
            buttonNo.gameObject.SetActive(data.showButtonNo);
        if (buttonCancel != null)
            buttonCancel.gameObject.SetActive(data.showButtonCancel);
    }

    public virtual void OnClickYes()
    {
        if (actionYes != null)
            actionYes.Invoke();
        Hide();
    }

    public virtual void OnClickNo()
    {
        if (actionNo != null)
            actionNo.Invoke();
        Hide();
    }

    public virtual void OnClickCancel()
    {
        if (actionCancel != null)
            actionCancel.Invoke();
        Hide();
    }

    public virtual void Show(string msg)
    {
        Content = msg;
        Show();
    }
}
