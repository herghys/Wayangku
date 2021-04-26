using System;
using System.Collections;
using System.Collections.Generic;

public enum AnswerType { Multi, Single }
public enum QuestionType { Text, Image}

[Serializable()]
public class Answer
{
    public string AnswerInfo = string.Empty;
    public bool IsCorrect = false;
    public Answer() { }
}
[Serializable()]
public class Question
{
    public string questionHint;
    public string Info = null;
    public QuestionType questionType = QuestionType.Text;
    public string questionImageName;
    public Answer[] Answers = null;
    public bool UseTimer = false;
    public int Timer = 0;
    public AnswerType Type = AnswerType.Single;
    public int AddScore = 0;

    public Question() { }

    #region Functions
    public List<int> GetCorrectAnswers()
    {
        List<int> CorrectAnswers = new List<int>();  
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }

    public List<string> GetCorrectAnswersInfo()
    {
        List<string> CorrectAnswerInfo = new List<string>();
        CorrectAnswerInfo.Clear();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswerInfo.Add(Answers[i].AnswerInfo);
            }
        }
        return CorrectAnswerInfo;
    }
    #endregion
}
