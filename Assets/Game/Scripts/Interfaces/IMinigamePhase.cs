using System.Collections;

namespace Game.Minigames
{
    public interface IMinigamePhase
    {
        IEnumerator Execute(MinigameContext context);
    }
}