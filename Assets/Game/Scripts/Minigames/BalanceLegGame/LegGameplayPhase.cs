using Game.Body;
using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class LegGameplayPhase : IMinigamePhase
    {
        private readonly LegGameRoundData _round;
        private LegController _leg;
        private readonly Transform _footTarget;
        private bool _injuryRequested;
        private bool _fell;


        public LegGameplayPhase(
            LegGameRoundData round, Transform target)
        {
            _round = round;
            _footTarget = target;

        }

        public IEnumerator Execute(MinigameContext context)
        {

            _leg = context.BodyPart as LegController;
            context.Receiver.Bind(_leg);

            context.State = EMinigameState.Playing;

            float timer = _round.duration;

            _leg.Activate();
            _leg.FellOver += OnFellOver;

            try
            {
                while (timer > 0f)
                {
                    if (_fell)
                    {
                        _fell = false;
                        yield return context.RunPhase(
                            new LegInjuryPhase());
                    }

                    timer -= Time.deltaTime;
                    context.UI.UpdateTimer(timer);

                    yield return null;
                }
            }
            finally
            {
                _leg.FellOver -= OnFellOver;
                _leg.Deactivate();
            }
        }


        private void OnFellOver()
        {
            _fell = true;
        }
    }
}