using Game.Body;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class PingPongGameplayPhase : IMinigamePhase
    {
        private readonly PingPongRoundData _round;
        private readonly PingPongBallController _ballController;
        private bool _roundFinished;
        private int _currentHits;

        private int _playerScore;
        private int _enemyScore;

        private int _scoreSinceLast;

        private bool _injuryRequested = false;
        private bool _success = false;
        public bool IsPaused { get; private set; }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;

        public PingPongGameplayPhase(PingPongRoundData round, PingPongBallController ballController)
        {
            _round = round;
            _ballController = ballController;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;
            context.Receiver.Injected += OnInjury;
            _currentHits = 0;
            _roundFinished = false;
            context.Receiver.Bind(context.BodyPart);
            float timer = _round.duration;
            AudioMinigameManager.Instance.PlayMusic(context.Minigame.GameMusic, true);
            UIMinigameManager.Instance.ShowScorePanel();


            HandController hand = context.BodyPart as HandController;
            hand.PingPongHandComponent.SetBallController(_ballController);

            _ballController.Initialize(this);

            SpawnBall(_round, context);

            try
            {

                while (!_roundFinished && timer > 0f)
                {
                    if (_injuryRequested)
                    {
                        _injuryRequested = false;
                        _ballController.StopBall();

                        yield return context.RunPhase(new InjuryPhase());
                        _ballController.StartBall();
                    }

                    timer -= Time.deltaTime;

                    context.UI.UpdateTimer(timer);

                    if (_currentHits >= _round.requiredHits)
                    {
                        _success = true;
                        break;
                    }


                    yield return null;
                }
                _roundFinished = true;
                if (!_roundFinished)
                    context.Cancelled = true;

                _ballController.StopBall();
            }
            finally
            {
                context.Receiver.Injected -= OnInjury;
                UIMinigameManager.Instance.HideScorePanel();
                _ballController.StopBall();
                            
            }
        }

        private void OnInjury()
        {
            _injuryRequested = true;
        }


        public bool RegisterPoint(bool isPlayer)
        {
            if (isPlayer) _playerScore++;
            else
            {
                _enemyScore++;
                _scoreSinceLast++;
            }

            UIMinigameManager.Instance.UpdateScores(_enemyScore, _playerScore);

            if (_scoreSinceLast>=3)
            {
                    OnInjury();
                    _scoreSinceLast = 0;
                return false;
            }
            return true;
        }
        private void SpawnBall(PingPongRoundData round, MinigameContext context)
        {
            // TODO
            //_ballController.SpawnBall();
            _ballController.StartBall();
        }
    }
}