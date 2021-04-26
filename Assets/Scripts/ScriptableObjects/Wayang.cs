using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tokoh", menuName = "Wayang/New Tokoh Wayang")]
public class Wayang : ScriptableObject
{

    [SerializeField]private string _nama;
    public string Nama { get { return _nama; } }

    [SerializeField] private string _namaKecil;
    public string NamaKecil { get { return _namaKecil; } }

    [SerializeField] private string _namaLain;
    public string NamaLain { get { return _namaLain; } }

    [SerializeField] private string _patih;
    public string Patih { get { return _patih; } }

    [SerializeField] private string _kasatriyan;
    public string Kasatriyan { get { return _kasatriyan; } }

    [Header("Keluarga")]
    [SerializeField] private string _ayah;
    public string Ayah { get { return _ayah; } }

    [SerializeField] private string _ibu;
    public string Ibu { get { return _ibu; } }

    [SerializeField] private string[] _saudaraKandung;
    public string[] SaudaraKandung { get { return _saudaraKandung; } }

    [SerializeField] private string[] _saudaraTiri;
    public string[] SaudaraTiri { get { return _saudaraTiri; } }

    [SerializeField] private string[] _istri;
    public string[] Istri { get { return _istri; } }

    [SerializeField] private string[] _anak;
    public string[] Anak { get { return _anak; } }

    [Header("Karakter")]
    [SerializeField] private string _postur;
    public string Postur { get { return _postur; } }

    [SerializeField] private string _sifat;
    public string Sifat { get { return _sifat; } }

    [SerializeField] private string _kesaktian;
    public string Kesaktian { get { return _kesaktian; } }

    [SerializeField] private string _ajian;
    public string Ajian { get { return _ajian; } }

    [SerializeField] private string _pusaka;
    public string Pusaka { get { return _pusaka; } }


    [Header("Digital")]
    [SerializeField] GameObject _model;
    public GameObject Model { get { return _model; } }

    [SerializeField] GameObject _prefabs;
    public GameObject Prefabs { get { return _prefabs; } }

    [SerializeField] Sprite _gambar;
    public Sprite Gambar { get { return _gambar; } }

    [SerializeField] Texture _marker;
    public Texture Marker { get { return _marker; } }

    [Header("Description")]
    [SerializeField] string[] _desc;
    public string[] Desc { get { return _desc; } }
}
