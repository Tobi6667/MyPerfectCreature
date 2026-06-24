using System.Collections.Generic;

public enum EQuizType
{
    GuessInjuryFromSymptoms,      // shown: symptoms → answer: injury name
    GuessBodyPartFromInjury,      // shown: injury name → answer: body part
    GuessInjuryFromDescription,   // shown: description → answer: injury name
    GuessInjuryFromFunFact,       // shown: fun fact → answer: injury name
    GuessInjuryFromVisual,        // shown: body part visually → answer: injury name
    GuessRealNameFromFunnyName,   // shown: funny name → answer: real medical name
}

public class QuizState
{

        public EQuizType Type;
        public string Question;
        public List<string> Options;
        //public EBodyInjuryType? VisualBodyPartType; // non-null = show body part visually
}