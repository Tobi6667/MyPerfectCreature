using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class FinalGameUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private VisualTreeAsset _optionPrefab;

    private Label _resultLabel;

    private Label _question;
    private VisualElement _container;
    private VisualElement _root;

    private VisualElement _main;
    private VisualElement _resultPanel;

    private VisualElement _scorePanel;
    private Label _scoreLabel;
    private Label _amountLabel;
    public Action<string> OnOptionSelected;

    // -----------------------------------
    // INIT
    // -----------------------------------
    private void Awake()
    {
        var root = _doc.rootVisualElement;
        _root = root;

        _question = _root.Q<Label>("question-label");
        _container = _root.Q<VisualElement>("answers-container");
        _resultLabel = _root.Q<Label>("result-label");
        _resultPanel = _root.Q<VisualElement>("result-panel");
        _main = _root.Q<VisualElement>("main-panel");
        _scorePanel = _root.Q<VisualElement>("final-score-panel");
        _scoreLabel = _root.Q<Label>("score-value");
        _amountLabel = _root.Q<Label>("score-total");
        _scorePanel.style.display = DisplayStyle.None;
        _main.style.display = DisplayStyle.None;
        _resultPanel.style.display = DisplayStyle.None;

    }

    public void ShowScore(int correct, int total)
    {
        _scoreLabel.text = correct.ToString();
        _amountLabel.text = total.ToString();
        _scorePanel.style.display = DisplayStyle.Flex;
    }

    // -----------------------------------
    // SHOW QUESTION
    // -----------------------------------
    public void Show(QuizState state, Action<string> onSelected)
    {

        OnOptionSelected = onSelected;
        _main.style.display = DisplayStyle.Flex;

        _question.text = state.Question;

        _container.Clear();

        foreach (var option in state.Options)
        {
            CreateOption(option);
        }
    }

    public void ShowResult(bool result, Action onShown)
    {
        _main.style.display = DisplayStyle.None;
        
        _resultLabel.text = result ? "correct" : "wrong";
        _resultPanel.style.display = DisplayStyle.Flex;
        DOVirtual.DelayedCall(2f, () =>
        {
            onShown?.Invoke();
            _resultPanel.style.display = DisplayStyle.None;
        });


    }
    // -----------------------------------
    // CREATE OPTION
    // -----------------------------------
    private void CreateOption(string option)
    {
        var element = _optionPrefab.Instantiate();

        var button = element.Q<Button>("select-btn");

        button.text = option;

        button.clicked += () =>
        {
            Debug.Log("answered");
            OnOptionSelected?.Invoke(option);
        };

        _container.Add(element);
    }
}