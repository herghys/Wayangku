using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestManager : MonoBehaviour
{
    public Wayang[] wayang;
    public string[] way;
    string str = "Arjuna";
    [SerializeField] Dictionary<string, Wayang> wayangDic = new Dictionary<string, Wayang>();

    public TextMeshProUGUI textt;
    void Start()
    {
        LoadWayang();

        Wayang wy = null;
        if (str == "Arjuna")
        {
            wy = wayangDic["Arjuna"];
        }
        else if (str == "Bima")
        {
            wy = wayangDic["Bima"];
        }

        Debug.Log(wy.Nama);
        textt.text = wy.Nama;
        //Debug.Log(wayangDic.Keys + " - " + wayangDic.Values);
        //textt.text = wayangDic["Arjuna"].Nama;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadWayang()
    {
        Object[] objs = Resources.LoadAll("Wayang", typeof(Wayang));
        wayang = new Wayang[objs.Length];
        way = new string[objs.Length];
       
        for (int i = 0; i < objs.Length; i++)
        {
            wayang[i] = (Wayang)objs[i];
            wayangDic.Add(wayang[i].name, wayang[i]);
        }
    }

    void WayangDict()
    {

    }
}
