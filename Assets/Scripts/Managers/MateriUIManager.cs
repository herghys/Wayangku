using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable()]
public struct MainUIElements
{

}

[Serializable()]
public struct MateriUIElements
{
    [SerializeField] TextMeshProUGUI _nama;
    public TextMeshProUGUI Nama { get { return _nama; } }

    [SerializeField] TextMeshProUGUI _materi;
    public TextMeshProUGUI Materi { get { return _materi; } }

    [SerializeField] Image _imageWayang;
    public Image ImageWayang { get { return _imageWayang; } }

    [SerializeField] CanvasGroup _materiGroup;
    public CanvasGroup MateriGroup { get { return _materiGroup; } }
}

public class MateriUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WayangEvents events = null;

    [Header("UIElements")]
    [SerializeField] RectTransform layoutToRebuild;
    [SerializeField] MainUIElements mainUI;
    [SerializeField] MateriUIElements descUI;

    #region Unity Defaults
    private void OnEnable()
    {
        //events.WayangUpdated += WayangUpdate;
        events.UpdateWayang += WayangUpdate;
        events.UIUpdate += UIUpdate;
    }

    private void OnDisable()
    {
        //events.WayangUpdated -= WayangUpdate;
        events.UpdateWayang -= WayangUpdate;
        events.UIUpdate -= UIUpdate;
    }
    #endregion

    private void WayangUpdate(Wayang wayang)
    {
        descUI.Nama.text = wayang.Nama;
        descUI.Materi.text = (wayang.Desc.Length < 1) ? wayang.Desc[0] : string.Join("\n\n", wayang.Desc);
        descUI.ImageWayang.sprite = wayang.Gambar;

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutToRebuild);
    }

    #region UI Updater
    private void UIUpdate(bool state)
    {
        if (state) ShowMateri();
        else HideMateri();
    }

    private void ShowMateri()
    {
        descUI.MateriGroup.alpha = 1;
        descUI.MateriGroup.blocksRaycasts = true;
        descUI.MateriGroup.interactable = true;
    }

    private void HideMateri()
    {
        descUI.MateriGroup.alpha = 0;
        descUI.MateriGroup.blocksRaycasts = false;
        descUI.MateriGroup.interactable = false;
    }
    #endregion
}
