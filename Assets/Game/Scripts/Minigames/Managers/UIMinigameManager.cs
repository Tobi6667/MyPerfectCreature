using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Minigames
{
    public class UIMinigameManager : MonoBehaviour
    {

        public static UIMinigameManager Instance { get; private set; }

        [SerializeField] private UIDocument _document;


        private VisualElement _tutorialPanel;
        private VisualElement _introPanel;
        private VisualElement _countdownPanel;
        private VisualElement _hudPanel;
        private VisualElement _readyPanel;

        private VisualElement _injuryPanel;
        private Label _countdownLabel;
        private Label _introLabel;
        private Label _tutorialLabel;
        private Label _timerLabel;

        void OnEnable()
        {
            var root = _document.rootVisualElement;

            _tutorialPanel = root.Q("tutorial");
            _introPanel = root.Q("intro");
            _countdownPanel = root.Q("countdown");
            _hudPanel = root.Q("HUD");
            _readyPanel = root.Q("ready");
            _injuryPanel = root.Q("Injury");

            _countdownLabel = root.Q<Label>("countdown-label");
            _introLabel = _introPanel.Q<Label>("intro-label");
            _tutorialLabel = _tutorialPanel.Q<Label>("tutorial-label");
            _timerLabel = _hudPanel.Q<Label>("timer-label");
            HideAll();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


        public IEnumerator PlayCountdown(int from = 3)
        {
            yield return CoPlayCountdown(from);
        }

        private IEnumerator CoPlayCountdown(int from = 3)
        {
            HideAll();
            Debug.Log("Starting countdown from " + from);
            SetVisible(_countdownPanel, true);

            for (int i = from; i >= 1; i--)
            {
                _countdownLabel.text = i.ToString();
                //   AudioMinigameManager.Instance?.PlayCountdownTick();
                yield return AnimateCountdownPop(_countdownLabel);
                yield return new WaitForSeconds(0.3f);  // gap between numbers
            }

            _countdownLabel.text = "GO!";
            //  AbilityGameAudioManager.Instance?.PlayCountdownGo();
            yield return AnimateCountdownPop(_countdownLabel);
            yield return new WaitForSeconds(0.4f);

            SetVisible(_countdownPanel, false);
        }


        public void HideAll()
        {
            SetVisible(_countdownPanel, false);
            SetVisible(_introPanel, false);
            SetVisible(_tutorialPanel, false);
            SetVisible(_injuryPanel, false);
        }
        public void ShowTutorial(string text)
        {
            Debug.Log(text + "aha");
            HideAll();

            _tutorialLabel.text = text;

            SetVisible(_tutorialPanel, true);
            AnimatePanelIn(_tutorialPanel);
        }
        public void ShowTimer()
        {
        }

        public void UpdateTimer(float time)
        {
            _timerLabel.text = $"{Mathf.CeilToInt(time)}s";
        }

        private void AnimatePanelIn(VisualElement panel)
        {
            panel.RemoveFromClassList("panel-hidden");
            panel.AddToClassList("panel-visible");
        }


        private static void SetVisible(VisualElement el, bool visible)
        {
            if (el == null) return;
            el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }


        private IEnumerator AnimateCountdownPop(Label label)
        {
            const float riseTime = 0.12f;
            const float holdTime = 0.15f;
            const float fallTime = 0.18f;

            float t = 0f;
            while (t < riseTime)
            {
                t += Time.deltaTime;
                float s = Mathf.Lerp(0.4f, 1.25f, t / riseTime);
                label.style.scale = new StyleScale(new Scale(new Vector2(s, s)));
                label.style.opacity = Mathf.Lerp(0f, 1f, t / riseTime);
                yield return null;
            }

            label.style.scale = new StyleScale(new Scale(Vector2.one * 1.25f));
            label.style.opacity = 1f;
            yield return new WaitForSeconds(holdTime);

            t = 0f;
            while (t < fallTime)
            {
                t += Time.deltaTime;
                float s = Mathf.Lerp(1.25f, 1.0f, t / fallTime);
                label.style.scale = new StyleScale(new Scale(new Vector2(s, s)));
                yield return null;
            }

            label.style.scale = new StyleScale(new Scale(Vector2.one));
        }

        public void ShowInjuryPanel()
        {
            SetVisible(_injuryPanel, true);
            AnimatePanelIn(_injuryPanel);
        }

        public void HideInjuryPanel()
        {
            SetVisible(_injuryPanel, false);
        }
    }
}
