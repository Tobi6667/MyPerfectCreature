using System.Collections;


namespace Game.Minigames
{
    public class CountdownPhase : IMinigamePhase
    {
        private int _count;

        public CountdownPhase(int count)
        {
            _count = count;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Countdown;
            yield return context.UI.PlayCountdown(_count);
        }
    }
}