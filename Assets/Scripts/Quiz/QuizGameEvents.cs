using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizGameEvents", menuName = "Quiz/ New Game Events")]
public class QuizGameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(QuizUIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback DisplayResolutionScreen;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated;
    
    public delegate void CorrectAnswersCallback();
    public CorrectAnswersCallback CorrectAnswers;    

    public string correctAnswers;

    [HideInInspector]
    public int currentFinalScore;
    [HideInInspector]
    public int StartUpHighScore;


}
