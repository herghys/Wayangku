using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class QuizGameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] SceneFader fader;
    private DataXML data = new DataXML();

    [SerializeField] QuizGameEvents events = null;

    [Header("Start Up")]
    [SerializeField] CanvasGroup warmUpGroup = null;
    [SerializeField] TextMeshProUGUI warmUpTimer = null;
    [SerializeField] Slider WarmUpSlider = null;
    [SerializeField] Image warmUpFill = null;
    [SerializeField] Gradient warmUpGradient = null;

    [Header("Timer")]
    [SerializeField] Gradient timerGradient = null;
    [SerializeField] Slider timerSlider = null;
    [SerializeField] Image timerFill = null;
    [SerializeField] Animator timerAnimator = null;
    [SerializeField] Animator sliderAnimator = null;
    [SerializeField] TextMeshProUGUI timerText = null;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuetions = new List<int>();
    private int currentQuestion = 0;
    private int timerStateParaHash = 0;

    private string sceneName;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;
    private IEnumerator IE_WarmUp = null;

    #endregion
    private bool IsFinished
    {
        get
        {
            return (FinishedQuetions.Count < 10) ? false : true;
        }
    }

    #region Unity Defaults
    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    private void Awake()
    {
        
        events.currentFinalScore = 0;
        LoadQuestion();
    }

    private void Start()
    {
        events.StartUpHighScore = PlayerPrefs.GetInt(GameData.QuizHichcoreSaveKey);

        sceneName = SceneManager.GetActiveScene().name.ToString();

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        //AudioManager.Instance.PlaySound("QuizMusic");

        IE_WarmUp = WarmUp();
        StartCoroutine(IE_WarmUp);

        /*Display();*/
    }
    #endregion
    
    #region GameControl
    public void GoToScene(string scene)
    {
        //AudioManager.Instance.StopAll();
        //AudioManager.Instance.PlaySound("MainMusic");
        fader.FadeTo(scene);
    }

    public void RestartGame()
    {
        fader.FadeTo(sceneName);
    }
    #endregion

    IEnumerator WarmUp()
    {
        warmUpGroup.alpha = 1;
        float t = WarmUpSlider.maxValue = 4f;
        WarmUpSlider.minValue = 1f;
        while (t>1)
        {
            t--;
            WarmUpSlider.value = t;
            warmUpFill.color = warmUpGradient.Evaluate(WarmUpSlider.normalizedValue);

            warmUpTimer.text = t.ToString();
            yield return new WaitForSeconds(1.0f);
            //AudioManager.Instance.PlaySound("CountdownSFX");
        }
        warmUpGroup.alpha = 0;
        Display();
    }
    
    /// <summary>
    /// Display Screen
    /// </summary>
    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Something Error in QuizGameManager. Display() Method");
        }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    /// <summary>
    /// Accept Answers
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        string sca = string.Join(",", data.Questions[currentQuestion].GetCorrectAnswersInfo());

        bool isCorrect = CheckAnswers();
        FinishedQuetions.Add(currentQuestion);

        //UpdateScore
        UpdateScore((isCorrect ? data.Questions[currentQuestion].AddScore : 0));

        UpdateCorrectAnswers(sca);
 
        if (IsFinished)
        {
            SetHighScore();
        }

        var type
            = (IsFinished)
            ? QuizUIManager.ResolutionScreenType.Finish
            : (isCorrect) ? QuizUIManager.ResolutionScreenType.Correct
            : QuizUIManager.ResolutionScreenType.Incorrect;

        events.DisplayResolutionScreen?.Invoke(type, data.Questions[currentQuestion].AddScore);

        //AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != QuizUIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameData.ResolutionDelayTime);
        Display();
    }

    private void SetHighScore()
    {
        var highscore = PlayerPrefs.GetInt(GameData.QuizHichcoreSaveKey);
        if (highscore < events.currentFinalScore)
        {
            PlayerPrefs.SetInt(GameData.QuizHichcoreSaveKey, events.currentFinalScore);
        }
    }

    #region Answers
    /// <summary>
    /// Update Answers For Selected Type
    /// </summary>
    /// <param name="newAnswer"></param>
    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (data.Questions[currentQuestion].Type == AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }
    /// <summary>
    /// Erase Picked Answers
    /// </summary>
    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = data.Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }
    #endregion

    #region Get Questions
    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return data.Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuetions.Count < data.Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, data.Questions.Length);
            } while (FinishedQuetions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    /// <summary>
    /// Koad Questions
    /// </summary>
    void LoadQuestion()
    {
        var streamingPath = Path.Combine(GameData.streamingXmlPath, GameData.xmlFile + ".xml");
        var gameFile = GameData.xmlFile + ".xml";

        #if UNITY_EDITOR
        data = DataXML.Fetch(streamingPath);
        #else
        data= DataXML.Fetch(gameFile);
        #endif
    }
#endregion

#region Timer
    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimator.SetInteger(timerStateParaHash, 2);
                sliderAnimator.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimator.SetInteger(timerStateParaHash, 1);
                sliderAnimator.SetInteger(timerStateParaHash, 1);
                break;
        }
    }

    IEnumerator StartTimer()
    {
        var totalTime = data.Questions[currentQuestion].Timer;
        var timeleft = totalTime;
        timerSlider.maxValue = totalTime - 1;
        timerFill.color = timerGradient.Evaluate(timerSlider.normalizedValue);

        while (timeleft > 0)
        {
            timeleft--;
            timerSlider.value = timeleft;

            timerText.color = timerGradient.Evaluate(timerSlider.normalizedValue);
            timerFill.color = timerGradient.Evaluate(timerSlider.normalizedValue);

            timerText.text = timeleft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }
#endregion

#region Event Updater
    private void UpdateCorrectAnswers(string ans)
    {
        events.correctAnswers = ans;
        if (events.CorrectAnswers != null)
        {
            events.CorrectAnswers();
        }
    }
    private void UpdateScore(int add)
    {
        events.currentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }
#endregion
}