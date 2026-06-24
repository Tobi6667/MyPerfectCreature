using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Minigames
{
    public class UIMinigameManager : MonoBehaviour
    {

        public static UIMinigameManager Instance { get; private set; }

        [SerializeField] private UIDocument _document;

        private VisualElement _feedbackPanel;
        private Label _feedbackLabel;
        private Coroutine _feedbackRoutine;
        private VisualElement _tutorialPanel;
        private VisualElement _introPanel;
        private VisualElement _countdownPanel;
        private VisualElement _hudPanel;
        private VisualElement _readyPanel;
        private VisualElement _instructionPanel;

        private VisualElement _injuryPanel;
        private Label _countdownLabel;
        private Label _introLabel;
        private Label _tutorialLabel;
        private Label _timerLabel;
        private Label _instructionLabel;


        private Label _injuryTitel;
        private Label _injuryName;
        private Label _injurySymptoms;
        private Label _injuryDescription;
        private Label _injuryFunfact;

        private ProgressBar _fatProgress;
        private ProgressBar _vatProgress;


        void OnEnable()
        {
            var root = _document.rootVisualElement;

            _tutorialPanel = root.Q("tutorial");
            _introPanel = root.Q("intro");
            _countdownPanel = root.Q("countdown");
            _hudPanel = root.Q("HUD");
            _readyPanel = root.Q("ready");
            _injuryPanel = root.Q("Injury");
            _instructionPanel = root.Q("anweisung");
            _countdownLabel = root.Q<Label>("countdown-label");
            _introLabel = _introPanel.Q<Label>("intro-label");
            _tutorialLabel = _tutorialPanel.Q<Label>("tutorial-label");
            _timerLabel = _hudPanel.Q<Label>("timer-label");

            _instructionLabel = _instructionPanel.Q<Label>("anweisung-label");
            _feedbackPanel = root.Q("feedback");
            _feedbackLabel = _feedbackPanel.Q<Label>("feedback-label");

            _injuryTitel = _injuryPanel.Q<Label>("injury-titel");
            _injuryName = _injuryPanel.Q<Label>("injury-name");
            _injurySymptoms = _injuryPanel.Q<Label>("injury-symptoms");
            _injuryDescription = _injuryPanel.Q<Label>("injury-description");
            _injuryFunfact = _injuryPanel.Q<Label>("injury-funfact");

            _fatProgress = _hudPanel.Q<ProgressBar>("progress-fat");
            _vatProgress = _hudPanel.Q<ProgressBar>("progress-vas");
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
                   AudioMinigameManager.Instance?.PlayCountdownTick();
                yield return AnimateCountdownPop(_countdownLabel);
                yield return new WaitForSeconds(0.3f);  // gap between numbers
            }

            _countdownLabel.text = "GO!";
            AudioMinigameManager.Instance?.PlayCountdownGo();
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
            SetVisible(_instructionPanel, false);
            SetVisible(_feedbackPanel, false);
        }
        public void ShowTutorial(string text)
        {
            Debug.Log(text + "aha");
            HideAll();

           _introLabel.text  = text;

            SetVisible(_introPanel, true);
            AnimatePanelIn(_introPanel);
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

        public void UpdateFatVas(int vat, int fat)
        {
            _fatProgress.value = fat;
            _vatProgress.value = vat;
        }


        private static void SetVisible(VisualElement el, bool visible)
        {
            if (el == null) return;
            el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }


        public void ShowInstruction(string instruction)
        {
            Debug.Log("inst: " + instruction);
            SetVisible(_instructionPanel, true);
            _instructionLabel.text = instruction;
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

        public void ShowInjuryPanel(IInjuryData injury)
        {
            _injuryTitel.text = injury.InjuryName;
            _injuryName.text = injury.InjuryRealName;
            _injurySymptoms.text = injury.Symptoms;
            _injuryDescription.text = injury.Description;
            _injuryFunfact.text = injury.FunFact;

            SetVisible(_injuryPanel, true);
            AnimatePanelIn(_injuryPanel);
        }

        public void HideInjuryPanel()
        {
            SetVisible(_injuryPanel, false);
        }


        public void ShowWrongGestureFeedback(bool tooSlow)
        {
            ShowFeedback(tooSlow ? "TOO SLOW!" : "WRONG!", "feedback-wrong");
        }

        public void ShowCorrectGestureFeedback()
        {
            ShowFeedback("NICE!", "feedback-correct");
        }

        private void ShowFeedback(string text, string styleClass)
        {
            if (_feedbackRoutine != null)
                StopCoroutine(_feedbackRoutine);

            _feedbackRoutine = StartCoroutine(CoShowFeedback(text, styleClass));
        }

        private IEnumerator CoShowFeedback(string text, string styleClass)
        {
            _feedbackLabel.text = text;

            _feedbackPanel.RemoveFromClassList("feedback-wrong");
            _feedbackPanel.RemoveFromClassList("feedback-correct");
            _feedbackPanel.AddToClassList(styleClass);

            _feedbackPanel.style.display = DisplayStyle.Flex;

            const float popTime = 0.12f;
            const float holdTime = 0.4f;
            const float fadeTime = 0.25f;
            const float shakeAmount = 8f;

            // pop in + shake
            float t = 0f;
            while (t < popTime)
            {
                t += Time.deltaTime;
                float s = Mathf.Lerp(0.6f, 1.1f, t / popTime);
                float shakeX = Mathf.Sin(t * 60f) * shakeAmount * (1f - t / popTime);
                _feedbackPanel.style.scale = new StyleScale(new Scale(new Vector2(s, s)));
                _feedbackPanel.style.translate = new StyleTranslate(new Translate(shakeX, 0));
                _feedbackPanel.style.opacity = Mathf.Lerp(0f, 1f, t / popTime);
                yield return null;
            }

            _feedbackPanel.style.scale = new StyleScale(new Scale(Vector2.one));
            _feedbackPanel.style.translate = new StyleTranslate(new Translate(0, 0));
            _feedbackPanel.style.opacity = 1f;

            yield return new WaitForSeconds(holdTime);

            // fade out
            t = 0f;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                _feedbackPanel.style.opacity = Mathf.Lerp(1f, 0f, t / fadeTime);
                yield return null;
            }

            _feedbackPanel.style.display = DisplayStyle.None;
            _feedbackRoutine = null;
        }
    }
}
