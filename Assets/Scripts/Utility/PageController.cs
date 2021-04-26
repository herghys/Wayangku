using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PageController : MonoBehaviour
{
    [SerializeField] TMP_Text descText;
    [SerializeField] int totalPage, currentPage;

    private void Awake()
    {
        descText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        totalPage = descText.textInfo.pageInfo[descText.textInfo.pageCount].lastCharacterIndex;
        Debug.Log(descText.textInfo.pageCount);
    }

    public void NextPage()
    {
            descText.pageToDisplay += 1;
        
    }

    public void PrevPage()
    {
        if (descText.pageToDisplay > totalPage - 1)
        {
            descText.pageToDisplay -= 1;
        }
    }
}
