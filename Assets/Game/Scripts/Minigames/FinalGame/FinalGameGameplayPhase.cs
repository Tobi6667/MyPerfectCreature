using Game.Minigames;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FinalGameGameplayPhase : IMinigamePhase
{

    [SerializeField] private FinalGameQuizController _quizController;

    private readonly FinalGameRoundData _round;
    public bool IsPaused { get; private set; }
    private bool _injuryRequested = false;

    private bool _roundFinished;

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;



    public FinalGameGameplayPhase(FinalGameRoundData round)
    {
        _round = round;
        _quizController.Initialize();
    }

    public IEnumerator Execute(MinigameContext context)
    {

        context.Receiver.Injected += OnInjury;
        float timer = _round.duration;

        try
        {

            while (!_roundFinished && timer > 0f)
            {
                if (_injuryRequested)
                {
                    _injuryRequested = false;

                    yield return context.RunPhase(new InjuryPhase());
                }

                timer -= Time.deltaTime;

                context.UI.UpdateTimer(timer);


                yield return null;
            }
            _roundFinished = true;
            if (!_roundFinished)
                context.Cancelled = true;


        }
        finally
        {
            context.Receiver.Injected -= OnInjury;
            UIMinigameManager.Instance.HideScorePanel();

        }
    }

    private void OnInjury()
    {
        _injuryRequested = true;
    }
}
