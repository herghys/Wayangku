using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public SceneFader fader;

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Panel
    public void ContextPanelOpener(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
    #endregion

    #region SceneFader
    public void GoToSceneByName(string scene)
    {
        fader.FadeTo(scene);
    }

    public void GoToSceneByIndex(int index)
    {
        fader.FadeToIndex(index);
    }
    #endregion
}
