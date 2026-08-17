using Game.Minigames;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FinalGameGameplayPhase : IMinigamePhase
{


 
    private readonly FinalGameRoundData _round;

    private FinalGameQuizController _finalGameQuizController;

    public bool IsPaused { get; private set; }

    private bool _roundFinished;

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;


    private bool _questionAsked = false;


    public FinalGameGameplayPhase(FinalGameRoundData round, FinalGameQuizController quiz)
    {
        _round = round;
        _finalGameQuizController = quiz;
        quiz.Initialize();
    }


    public IEnumerator Execute(MinigameContext context)
    {


        float timer = _round.duration;

        try
        {

            while (!_roundFinished)
            {
                if (!_questionAsked)
                {
                    _questionAsked = true;
                    if (_finalGameQuizController.QuestionsLeft())
                    {
                        yield return context.RunPhase(new QuestionPhase(_finalGameQuizController));
                        _questionAsked = false;
                    }
                    else
                    {
                       // _roundFinished = true;
                        _finalGameQuizController.ShowScore();
                        Debug.Log("quiz finished");
                    }
                }


                yield return null;

            }


        }
        finally
        {


        }
    }

}
