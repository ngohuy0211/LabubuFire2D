using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : BasePopup
{
    [SerializeField] Text textTitle;
    [SerializeField] Text textMessage;
    [SerializeField] Text textAction1;
    [SerializeField] Text textAction2;
    [SerializeField] GameObject buttonAction1;
    [SerializeField] GameObject buttonAction2;
    [SerializeField] GameObject boxCheck;
    [SerializeField] GameObject goTick;
    [SerializeField] Button btnClose;
    [SerializeField] ListSprite imgButton;
    
    private System.Action _action1Cb, _action2Cb;
    private System.Action<bool> _actClickCheckBoxCb;
    
    protected override void Awake()
    {
        base.Awake();
        if (boxCheck != null)
            boxCheck.SetActive(false);
        UIHelper.AddButtonClickNormal(buttonAction1.GetComponent<Button>(), ButtonAction1Click);
        UIHelper.AddButtonClickNormal(buttonAction2.GetComponent<Button>(), ButtonAction2Click);
        UIHelper.AddButtonClickNormal(boxCheck.transform.GetChild(0).GetComponent<Button>(), OnClickCheckBox);
        UIHelper.AddButtonClickClose(btnClose, ClosePopup);
    }

    private void SetData(string title, string message, string action1 = "", System.Action action1Cb = null,
        string action2 = "", System.Action action2Cb = null)
    {
        this.textTitle.text = title;

        this.textMessage.text = message;
        this._action1Cb = action1Cb;
        this._action2Cb = action2Cb;
        //
        if (Utils.IsStringEmpty(action1) && Utils.IsStringEmpty(action2))
            action1 = "OK";
        //
        if (!Utils.IsStringEmpty(action1))
        {
            buttonAction1.SetActive(true);
            textAction1.text = action1;
        }
        else
            buttonAction1.SetActive(false);

        //
        if (!Utils.IsStringEmpty(action2))
        {
            buttonAction2.SetActive(true);
            textAction2.text = action2;
        }
        else
            buttonAction2.SetActive(false);
    }

    private void ButtonAction1Click()
    {
        if (_action1Cb != null)
            _action1Cb();

        ClosePopup();
    }

    private void ButtonAction2Click()
    {
        if (_action2Cb != null)
            _action2Cb();

        ClosePopup();
    }
    
    //Confirm
    private void SetupConfirm(string msg, System.Action action)
    {
        textTitle.text = "";
        textMessage.text = msg;
        textAction1.text = "Đồng ý";
        textAction2.text = "Hủy";
        buttonAction1.GetComponent<Image>().sprite = imgButton.GetSprite(1);
        buttonAction1.GetComponent<Image>().sprite = imgButton.GetSprite(3);
        buttonAction1.transform.GetChild(0).GetComponent<Text>().color = new Color(36 / 255f, 65 / 255f, 93 / 255f);
        _action1Cb = action;
    }

    private void SetupWarning(string msg, System.Action action)
    {
        textTitle.text = "";
        textMessage.text = msg;
        textAction1.text = "Đồng ý";
        textAction2.text = "Hủy";
        buttonAction1.GetComponent<Image>().sprite = imgButton.GetSprite(1);
        buttonAction1.GetComponent<Image>().sprite = imgButton.GetSprite(3);
        buttonAction1.transform.GetChild(0).GetComponent<Text>().color = new Color(84 / 255f, 27 / 255f, 27 / 255f);
        _action1Cb = action;
    }


    public static PopupMessage ShowUpConfirm(string message, System.Action action)
    {
        PopupMessage popupMess = BasePopup.ShowPopup<PopupMessage>(PopupPath.POPUP_MESSAGE);
        popupMess.SetupConfirm(message, action);
        //
        return popupMess;
    }

    public static PopupMessage ShowUpWarning(string message, System.Action action)
    {
        PopupMessage popupMess = BasePopup.ShowPopup<PopupMessage>(PopupPath.POPUP_MESSAGE);
        popupMess.SetupWarning(message, action);
        //
        return popupMess;
    }
    
    public static PopupMessage ShowUp(string title, string message, string action1 = "", Action action1Cb = null,
        string action2 = "", System.Action action2Cb = null)
    {
        PopupMessage popupMess = BasePopup.ShowPopup<PopupMessage>(PopupPath.POPUP_MESSAGE);
        popupMess.SetData(title, message, action1, action1Cb, action2, action2Cb);
        //
        return popupMess;
    }

    private bool _isSelectCheckBox = false;

    private void OnClickCheckBox()
    {
        _isSelectCheckBox = !_isSelectCheckBox;
        goTick.SetActive(_isSelectCheckBox);
        _actClickCheckBoxCb?.Invoke(_isSelectCheckBox);
    }

    public void SetCbCheckBox(System.Action<bool> cb)
    {
        _actClickCheckBoxCb = cb;
    }

    public void ShowCheckBox(bool valueShow)
    {
        _isSelectCheckBox = false;
        goTick.SetActive(_isSelectCheckBox);
        boxCheck.SetActive(valueShow);
    }
}
