using Game.Body;
using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class LegGameplayPhase : IMinigamePhase
    {
        private readonly LegGameRoundData _round;
        private readonly LegController _leg;
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
            context.Receiver.Bind(context.BodyPart as LegController);
            context.State = EMinigameState.Playing;
            float timer = _round.duration;
            var xt = context.BodyPart as LegController;

            xt.Activate();
            context.Receiver.Injected += OnInjury;
           // _leg.FellOver += OnFellOver;



            try
            {
                while (timer > 0f)
                {
                    if (_fell)
                    {
                        context.Cancelled = true;
                        yield break;
                    }

                    if (_injuryRequested)
                    {
                        _injuryRequested = false;

                        yield return context.RunPhase(
                            new InjuryPhase());
                    }

                    timer -= Time.deltaTime;

                    context.UI.UpdateTimer(timer);

                    yield return null;
                }

                // survived entire round
            }
            finally
            {
               // _controller.StopBalance();

                context.Receiver.Injected -= OnInjury;
                //_controller.FellOver -= OnFellOver;
            }
        }

        private void OnInjury()
        {
            _injuryRequested = true;
        }

        private void OnFellOver()
        {
            _fell = true;
        }
    }
}