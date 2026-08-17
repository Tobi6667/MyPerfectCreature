using Game.Body;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Minigames
{
    public class QuestionPhase : IMinigamePhase
    {
        private readonly FinalGameQuizController _quizController;

        private bool _lastAnswerCorrect = false;
        private bool _answered = false;
        private bool _resultShown = false;
        public QuestionPhase(FinalGameQuizController quizController)
        {
            _quizController = quizController;
        }



        public IEnumerator Execute(MinigameContext context)
        {

            _quizController.SetupQuestion((result) =>
            {
                _lastAnswerCorrect = _quizController.Submit(result);
                _answered = true;
                _quizController.ShowResult(_lastAnswerCorrect, () =>
                {
                    _resultShown = true;
                });
            });

            try
            {
                while (!_resultShown)
                    yield return null;


            }
            finally
            {
                Debug.Log("question done");
            }

        }


    }
    
}
