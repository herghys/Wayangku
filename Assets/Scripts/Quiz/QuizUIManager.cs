using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}

[Serializable]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [SerializeField] Image questionImage;
    public Image QuestionImage { get { return questionImage; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}

public class QuizUIManager : MonoBehaviour
{
    #region Variables
    public enum         ResolutionScreenType   { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField]    QuizGameEvents         events                       = null;

    [Header("UI Elements (Prefabs)")]
    [SerializeField]    AnswerData             answerPrefab                 = null;

    [SerializeField]    UIElements             uiElements                   = new UIElements();

    [Space]
    [SerializeField]    UIManagerParameters    parameters                   = new UIManagerParameters();

    private             List<AnswerData>       currentAnswers               = new List<AnswerData>();
    private             int                    resStateParaHash             = 0;

    private             IEnumerator            IE_DisplayTimedResolution    = null;
    #endregion

    #region Unity Defaults
    private void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }
    private void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    private void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }
    #endregion

    private void UpdateQuestionUI(Question question)
    {
        switch (question.questionType)
        {
            case QuestionType.Text:
                uiElements.QuestionImage.enabled = false;
                uiElements.QuestionInfoTextObject.alignment = TextAlignmentOptions.Center;
                break;
            case QuestionType.Image:
                uiElements.QuestionInfoTextObject.alignment = TextAlignmentOptions.Left;
                uiElements.QuestionImage.enabled = true;
                uiElements.QuestionImage.sprite = Resources.Load<Sprite>("Quiz/" + question.questionImageName);
                break;
        }
        uiElements.QuestionInfoTextObject.text = question.Info;
        CreateAnswers(question);
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uiElements.MainCanvasGroup.blocksRaycasts = false;
        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimeResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }

    IEnumerator DisplayTimeResolution()
    {
        yield return new WaitForSeconds(GameData.ResolutionDelayTime);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uiElements.MainCanvasGroup.blocksRaycasts = true;
    }

    private void UpdateResUI(ResolutionScreenType type, int score)
    {
        var highScore = PlayerPrefs.GetInt(GameData.QuizHichcoreSaveKey);
        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBG.color = parameters.CorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "BENAR!";
                uiElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "SALAH!";
                uiElements.ResolutionScoreText.text = $"Jawaban: \n {events.correctAnswers}";
                break;
            case ResolutionScreenType.Finish:
                uiElements.ResolutionBG.color = parameters.FinalBGColor;
                uiElements.ResolutionStateInfoText.text = "TOTAL SKOR";

                if (events.currentFinalScore == 0)
                {
                    uiElements.ResolutionScoreText.text = "0";
                }else StartCoroutine(CalculateScore());
                uiElements.FinishUIElements.gameObject.SetActive(true);
                uiElements.HighScoreText.gameObject.SetActive(true);
                uiElements.HighScoreText.text = ((highScore > events.StartUpHighScore) ? "<color=yellow> New </color>" : String.Empty) + "Highscore: " + highScore;
                break;
        }
    }

    IEnumerator CalculateScore()
    {
        var scoreValue = 0;

        while (scoreValue < events.currentFinalScore)
        {
            scoreValue++;
            uiElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();
        float offet = 0 - parameters.Margins;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = Instantiate(answerPrefab, uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].AnswerInfo, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offet);

            offet -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uiElements.AnswersContentArea.sizeDelta = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offet * -1);

            currentAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    private void UpdateScoreUI()
    {
        uiElements.ScoreText.text = "Score: " + events.currentFinalScore;
    }
}
