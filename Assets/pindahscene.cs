using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pindahscene : MonoBehaviour
{
    /// <summary>
    /// Pindah Scene to BuilIndex
    /// </summary>
    /// <param name="index"></param>
    public void pindahScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}