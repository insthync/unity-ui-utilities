using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputDialog : UIMessageDialog
{
    public InputField inputContent;
    public Text inputPlaceHolderText;
    public string defaultPlaceHolder = "Enter text...";
    public int defaultCharacterLimit = 0;
    private UnityAction<string> onConfirmText;
    private UnityAction<int> onConfirmInteger;
    private UnityAction<float> onConfirmDecimal;
    private InputField.ContentType contentType;
    private int characterLimit;
    private int intDefaultAmount;
    private int? intMinAmount;
    private int? intMaxAmount;
    private float floatDefaultAmount;
    private float? floatMinAmount;
    private float? floatMaxAmount;
    public InputField.CharacterValidation defaultCharacterValidation = InputField.CharacterValidation.None;
    public InputField.ContentType defaultContentType = InputField.ContentType.Standard;
    public InputField.InputType defaultInputType = InputField.InputType.Standard;
    public InputField.LineType defaultLineType = InputField.LineType.SingleLine;

    public string InputContent
    {
        get { return inputContent == null ? "" : inputContent.text; }
        set { if (inputContent != null) inputContent.text = value; }
    }

    public string InputPlaceHolder
    {
        get { return inputPlaceHolderText == null ? "" : inputPlaceHolderText.text; }
        set { if (inputPlaceHolderText != null) inputPlaceHolderText.text = value; }
    }

    public int InputCharacterLimit
    {
        get { return inputContent == null ? 0 : inputContent.characterLimit; }
        set { if (inputContent != null) inputContent.characterLimit = value; }
    }

    public InputField.CharacterValidation InputCharacterValidation
    {
        get { return inputContent == null ? InputField.CharacterValidation.None : inputContent.characterValidation; }
        set { if (inputContent != null) inputContent.characterValidation = value; }
    }

    public InputField.ContentType InputContentType
    {
        get { return inputContent == null ? InputField.ContentType.Standard : inputContent.contentType; }
        set { if (inputContent != null) inputContent.contentType = value; }
    }

    public InputField.InputType InputInputType
    { 
        get { return inputContent == null ? InputField.InputType.Standard : inputContent.inputType; }
        set { if (inputContent != null) inputContent.inputType = value; }
    }

    public InputField.LineType InputLineType
    {
        get { return inputContent == null ? InputField.LineType.SingleLine : inputContent.lineType; }
        set { if (inputContent != null) inputContent.lineType = value; }
    }

    public override void Hide()
    {
        base.Hide();
        InputContent = "";
    }

    public void SetInputPropertiesToDefault()
    {
        InputPlaceHolder = defaultPlaceHolder;
        InputCharacterLimit = defaultCharacterLimit;
        InputCharacterValidation = defaultCharacterValidation;
        InputContentType = defaultContentType;
        InputInputType = defaultInputType;
        InputLineType = defaultLineType;
    }

    public override void Show()
    {
        if (inputContent != null)
        {
            inputContent.contentType = contentType;
            inputContent.characterLimit = characterLimit;
        }
        base.Show();
    }

    public void Show(string title,
        string content,
        UnityAction<string> onConfirmText,
        string defaultText = "",
        InputField.ContentType contentType = InputField.ContentType.Standard,
        int characterLimit = 0)
    {
        Title = title;
        Content = content;
        InputContent = defaultText;
        this.contentType = contentType;
        this.characterLimit = characterLimit;
        this.onConfirmText = onConfirmText;
        Show();
    }

    public void Show(string title,
        string content,
        UnityAction<int> onConfirmInteger,
        int? minAmount = null,
        int? maxAmount = null,
        int defaultAmount = 0)
    {
        if (!minAmount.HasValue)
            minAmount = int.MinValue;
        if (!maxAmount.HasValue)
            maxAmount = int.MaxValue;

        intDefaultAmount = defaultAmount;
        intMinAmount = minAmount;
        intMaxAmount = maxAmount;

        Title = title;
        Content = content;
        InputContent = defaultAmount.ToString();
        if (inputContent != null)
        {
            if (minAmount.Value > maxAmount.Value)
            {
                minAmount = null;
                Debug.LogWarning("min amount is more than max amount");
            }
            inputContent.onValueChanged.RemoveAllListeners();
            inputContent.onValueChanged.AddListener(ValidateIntAmount);
        }
        contentType = InputField.ContentType.IntegerNumber;
        characterLimit = 0;
        this.onConfirmInteger = onConfirmInteger;
        Show();
    }

    protected void ValidateIntAmount(string result)
    {
        int amount = intDefaultAmount;
        if (int.TryParse(result, out amount))
        {
            inputContent.onValueChanged.RemoveAllListeners();
            if (intMinAmount.HasValue && amount < intMinAmount.Value)
                InputContent = intMinAmount.Value.ToString();
            if (intMaxAmount.HasValue && amount > intMaxAmount.Value)
                InputContent = intMaxAmount.Value.ToString();
            inputContent.onValueChanged.AddListener(ValidateIntAmount);
        }
    }

    public void Show(string title,
        string content,
        UnityAction<float> onConfirmDecimal,
        float? minAmount = null,
        float? maxAmount = null,
        float defaultAmount = 0f)
    {
        if (!minAmount.HasValue)
            minAmount = float.MinValue;
        if (!maxAmount.HasValue)
            maxAmount = float.MaxValue;

        floatDefaultAmount = defaultAmount;
        floatMinAmount = minAmount;
        floatMaxAmount = maxAmount;
        Title = title;
        Content = content;
        InputContent = defaultAmount.ToString();
        if (inputContent != null)
        {
            if (minAmount.Value > maxAmount.Value)
            {
                minAmount = null;
                Debug.LogWarning("min amount is more than max amount");
            }
            inputContent.onValueChanged.RemoveAllListeners();
            inputContent.onValueChanged.AddListener(ValidateFloatAmount);
        }
        contentType = InputField.ContentType.DecimalNumber;
        characterLimit = 0;
        this.onConfirmDecimal = onConfirmDecimal;
        Show();
    }

    protected void ValidateFloatAmount(string result)
    {
        float amount = floatDefaultAmount;
        if (float.TryParse(result, out amount))
        {
            inputContent.onValueChanged.RemoveAllListeners();
            if (floatMinAmount.HasValue && amount < floatMinAmount.Value)
                InputContent = floatMinAmount.Value.ToString();
            if (floatMaxAmount.HasValue && amount > floatMaxAmount.Value)
                InputContent = floatMaxAmount.Value.ToString();
            inputContent.onValueChanged.AddListener(ValidateFloatAmount);
        }
    }

    public override void OnClickYes()
    {
        switch (contentType)
        {
            case InputField.ContentType.IntegerNumber:
                int intAmount = int.Parse(InputContent);
                if (onConfirmInteger != null)
                    onConfirmInteger.Invoke(intAmount);
                break;
            case InputField.ContentType.DecimalNumber:
                float floatAmount = float.Parse(InputContent);
                if (onConfirmDecimal != null)
                    onConfirmDecimal.Invoke(floatAmount);
                break;
            default:
                string text = InputContent;
                if (onConfirmText != null)
                    onConfirmText.Invoke(text);
                break;
        }
        base.OnClickYes();
    }
}
