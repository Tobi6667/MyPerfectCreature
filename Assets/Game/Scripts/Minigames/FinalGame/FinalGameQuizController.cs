using DG.Tweening;
using Game.Body;
using Game.Main;
using Game.Minigames;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FinalGameQuizController: MonoBehaviour
{
    [SerializeField] private List<BodyPartBase> _bodyParts;
    [SerializeField] private Transform _bodyTarget;

    [SerializeField] private FinalGameUIController _uicontroller;
    private BodyPartBase _activeBodypart;

    private BodyPartBase _lastBody;

    private int _correct = 0;
    private int _totalQuestions = 0;

    private List<IInjuryData> _injuries;

    private List<IInjuryData> _allInjuries;

    private IInjuryData _currentInjury;
    private EQuizType _currentType;

    private Action<String> _onSelected;

    [SerializeField]
    private List<EQuizType> _allowedTypes = new List<EQuizType>
    {
        EQuizType.GuessInjuryFromSymptoms,
        EQuizType.GuessInjuryFromVisual,
        EQuizType.GuessBodyPartFromInjury,
        EQuizType.GuessInjuryFromDescription,
        EQuizType.GuessInjuryFromFunFact,
        EQuizType.GuessRealNameFromFunnyName,
    };

    private static readonly string[] BodyParts =  { "Head", "Torso", "LeftArm", "RightArm", "LeftLeg", "RightLeg" };

    public void Initialize()
    {
        _bodyParts = new List<BodyPartBase>();
        _allInjuries = new List<IInjuryData>();
        _bodyParts = GameManager.Instance.BodypartsList;
        _injuries = new List<IInjuryData>();
        _injuries.AddRange(GodInjuryDatabase.GetAllShown());
        _allInjuries.AddRange(GodInjuryDatabase.GetAll());
        _totalQuestions = _injuries.Count;


    }

    public bool QuestionsLeft()
    {
        if(_injuries.Count == 0) return false;
        else return true;
    }

    public void SetupQuestion(Action<string> onSelected)
    {

        _onSelected = onSelected;
        _currentInjury = _injuries[UnityEngine.Random.Range(0, _injuries.Count)];

        foreach (var part in _bodyParts)
        {
            if(_currentInjury.Region == part.Region)
            {
                _activeBodypart = part;
                break;
            }
        }

        // UIMinigameManager.Instance.ShowInjuryPanel(_currentInjury);

        _uicontroller.Show(NextQuestion(), _onSelected);
        _injuries.Remove(_currentInjury);

    }


    public void ShowScore()
    {
        _uicontroller.ShowScore(_correct, _totalQuestions);
    }


    public void EnableVisualQuestions(bool on)
    {
        if (on && !_allowedTypes.Contains(EQuizType.GuessInjuryFromVisual))
            _allowedTypes.Add(EQuizType.GuessInjuryFromVisual);
        else
            _allowedTypes.Remove(EQuizType.GuessInjuryFromVisual);
    }

    public QuizState NextQuestion()
    {

        _currentType = _allowedTypes[UnityEngine.Random.Range(0, _allowedTypes.Count)];

        if(_activeBodypart as FrankensteinController)
        {
            var fr = _activeBodypart as FrankensteinController;
            fr.OnQuiz();
        }
        Debug.Log("active part: " + _activeBodypart);
        _activeBodypart.transform.DOMove(_bodyTarget.position + _activeBodypart.OffsetPositionAtFinal, 2f).OnComplete(()=>
        {

            _activeBodypart.transform.DOLocalRotate(_activeBodypart.OffsetAtFinal, 1f);



            _activeBodypart.OnInject(_currentInjury);

                _lastBody = _activeBodypart;
            
        });

        if(_lastBody && _lastBody != _activeBodypart)
        {
            _lastBody.transform.DOMove(_bodyTarget.position - new Vector3(10f, 0f, 0f),1.4f);
        }



        return new QuizState
        {
            Type = _currentType,
            Question = BuildQuestion(),
            Options = BuildAnswers(),
        };


    }

    private string BuildQuestion() => _currentType switch
    {
        EQuizType.GuessInjuryFromSymptoms => $"Symptoms:\n{_currentInjury.Symptoms}",
        EQuizType.GuessBodyPartFromInjury => $"Which body part does '{_currentInjury.InjuryName}' affect?",
        EQuizType.GuessInjuryFromDescription => $"{_currentInjury.Description}",
        EQuizType.GuessInjuryFromFunFact => $"Fun fact:\n{_currentInjury.FunFact}",
        EQuizType.GuessInjuryFromVisual => "What injury is being shown?",
        EQuizType.GuessRealNameFromFunnyName => $"What is the real name for '{_currentInjury.InjuryName}'?",
        _ => "?"
    };

    private List<string> BuildAnswers()
    {
        string correct = _currentType switch
        {
            EQuizType.GuessBodyPartFromInjury => _currentInjury.Region.ToString(),
            EQuizType.GuessRealNameFromFunnyName => _currentInjury.InjuryRealName,
            _ => _currentInjury.InjuryName
        };

        List<string> pool = _currentType switch
        {
            EQuizType.GuessBodyPartFromInjury => new List<string>(BodyParts),
            EQuizType.GuessRealNameFromFunnyName => _allInjuries.ConvertAll(i => i.InjuryRealName),
            _ => _allInjuries.ConvertAll(i => i.InjuryName)
        };

        // Remove correct from pool to avoid duplicates
        pool.Remove(correct);

        var answers = new List<string> { correct };

        int targetCount = Mathf.Min(4, 1 + pool.Count); // up to 4 options

        while (answers.Count < targetCount && pool.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, pool.Count);
            answers.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        Shuffle(answers);
        return answers;
    }

    public bool Submit(string answer) => _currentType switch
    {
        EQuizType.GuessBodyPartFromInjury => answer == _currentInjury.Region.ToString(),
        EQuizType.GuessRealNameFromFunnyName => answer == _currentInjury.InjuryRealName,
        _ => answer == _currentInjury.InjuryName
    };

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void ShowResult(bool result, Action onShown)
    {
        _uicontroller.ShowResult(result,onShown);

        if (result)
        {
            _correct++;
        }


    }

}
