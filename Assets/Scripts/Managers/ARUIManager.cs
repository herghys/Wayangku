using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable()]
public struct UIBio
{
    [SerializeField] TextMeshProUGUI targetNameText;
    public TextMeshProUGUI TargetNameText { get { return targetNameText; } }

    [SerializeField] TextMeshProUGUI namaText;
    public TextMeshProUGUI NamaText { get { return namaText; } }

    [SerializeField] TextMeshProUGUI namaKecilText;
    public TextMeshProUGUI NamaKecilText { get { return namaKecilText; } }

    [SerializeField] TextMeshProUGUI namaLainText;
    public TextMeshProUGUI NamaLainText { get { return namaLainText; } }

    [SerializeField] TextMeshProUGUI patihText;
    public TextMeshProUGUI PatihText { get { return patihText; } }

    [SerializeField] TextMeshProUGUI kasatriyanText;
    public TextMeshProUGUI KasatriyanText { get { return kasatriyanText; } }
}

[Serializable()]
public struct UIFamily
{
    [SerializeField] TextMeshProUGUI ayahText;
    public TextMeshProUGUI AyahText { get { return ayahText; } }

    [SerializeField] TextMeshProUGUI ibuText;
    public TextMeshProUGUI IbuText { get { return ibuText; } }

    [SerializeField] TextMeshProUGUI kandungText;
    public TextMeshProUGUI KandungText { get { return kandungText; } }

    [SerializeField] TextMeshProUGUI tiriText;
    public TextMeshProUGUI TiriText { get { return tiriText; } }

    [SerializeField] TextMeshProUGUI istriText;
    public TextMeshProUGUI IstriText { get { return istriText; } }

    [SerializeField] TextMeshProUGUI anakText;
    public TextMeshProUGUI AnakText { get { return anakText; } }
}

[Serializable()]
public struct UICharacter
{
    [SerializeField] TextMeshProUGUI posturText;
    public TextMeshProUGUI PostureText { get { return posturText; } }

    [SerializeField] TextMeshProUGUI watakText;
    public TextMeshProUGUI WatakText { get { return watakText; } }

    [SerializeField] TextMeshProUGUI kesaktianText;
    public TextMeshProUGUI KesaktianText { get { return kesaktianText; } }

    [SerializeField] TextMeshProUGUI ajianText;
    public TextMeshProUGUI AjianText { get { return ajianText; } }

    [SerializeField] TextMeshProUGUI pusakaText;
    public TextMeshProUGUI PusakaText { get { return pusakaText; } }
}

public class ARUIManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WayangEvents events = null;

    [Header("UI Elements")]
    [SerializeField] RectTransform contentPanel;
    public RectTransform ContentPanel { get { return contentPanel; } }
    [SerializeField] UIBio uiBio;
    [SerializeField] UIFamily uiFamily;
    [SerializeField] UICharacter uiChar;

	[SerializeField] ScrollRect scroll;
	#endregion

	#region Unity Defaults
	private void OnEnable()
    {
        events.UpdateWayang += UpdateWayangDescription;
    }
    private void OnDisable()
    {
        events.UpdateWayang -= UpdateWayangDescription;
    }
    void Start()
    {
        SetDescriptionEmpty();
    }
    #endregion

    #region Set UI
    private void UpdateWayangDescription(Wayang wayang)
    {
		//Debug.Log(wayang);
		LayoutRebuilder.MarkLayoutForRebuild(ContentPanel);
		scroll.enabled = false;
		if (wayang != null) SetDescription(wayang);
        else SetDescriptionEmpty();
		scroll.enabled = true;
	}
    private void SetDescription(Wayang wayang)
    {
        //Bio
        uiBio.TargetNameText.text = wayang.name;

        uiBio.NamaText.text = wayang.Nama;
        uiBio.NamaKecilText.text = wayang.NamaKecil;
        uiBio.NamaLainText.text = wayang.NamaLain;
        uiBio.PatihText.text = wayang.NamaLain;
        uiBio.KasatriyanText.text = wayang.Kasatriyan;

        //Keluarga
        uiFamily.AyahText.text = wayang.Ayah;
        uiFamily.IbuText.text = wayang.Ibu;
        uiFamily.KandungText.text = string.Join(", ", wayang.SaudaraKandung);
        uiFamily.TiriText.text = string.Join(", ", wayang.SaudaraTiri);

        string _istri = (wayang.Istri.Length<2)? string.Join(", ", wayang.Istri) : string.Join("\n", wayang.Istri);
        uiFamily.IstriText.text = _istri;
        string _anak = (wayang.Anak.Length < 2) ? string.Join(", ", wayang.Anak) : string.Join("\n", wayang.Anak);
        uiFamily.AnakText.text = _anak;

        //Karakter
        uiChar.PostureText.text = wayang.Postur;
        uiChar.WatakText.text = wayang.Sifat;
        uiChar.KesaktianText.text = wayang.Kesaktian;
        uiChar.AjianText.text = wayang.Ajian;
        uiChar.PusakaText.text = wayang.Pusaka;

		LayoutRebuilder.ForceRebuildLayoutImmediate(ContentPanel);
		Canvas.ForceUpdateCanvases();
	}
    private void SetDescriptionEmpty()
    {
        uiBio.TargetNameText.text = "Tokoh";

        uiBio.NamaText.text = string.Empty;
        uiBio.NamaKecilText.text = string.Empty;
        uiBio.NamaLainText.text = string.Empty;
        uiBio.PatihText.text = string.Empty;
        uiBio.KasatriyanText.text = string.Empty;

        uiFamily.AyahText.text = string.Empty;
        uiFamily.IbuText.text = string.Empty;
        uiFamily.KandungText.text = string.Empty;
        uiFamily.TiriText.text = string.Empty;
        uiFamily.IstriText.text = string.Empty;
        uiFamily.AnakText.text = string.Empty;

        uiChar.PostureText.text = string.Empty;
        uiChar.WatakText.text = string.Empty;
        uiChar.KesaktianText.text = string.Empty;
        uiChar.AjianText.text = string.Empty;
        uiChar.PusakaText.text = string.Empty;

		LayoutRebuilder.ForceRebuildLayoutImmediate(ContentPanel);
		Canvas.ForceUpdateCanvases();

	}
    #endregion
}
