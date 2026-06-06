using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Game.Body
{

    public class LegInjuryInjector : MonoBehaviour
    {
        [SerializeField] private LegController _controller;

        private LegInjuryInstance _currentInjury;
        private List<LegInjuryInstance> _shownInjuries = new List<LegInjuryInstance>();

        private List<LegInjuryInstance> AvailableInjuries => LegInjuryDatabase.GetAll();


        internal void InjectRandomInjury()
        {
            var injuries = AvailableInjuries;

            if (injuries == null || injuries.Count == 0)
            {
                Debug.LogError("No injuries found in LegInjuryDatabase!");
                return;
            }

            int randomIndex = Random.Range(0, injuries.Count);

            LegInjuryInstance injuryToInject = injuries[randomIndex];
            _currentInjury = injuryToInject;
            _shownInjuries.Add(_currentInjury);

        }

        internal List<IInjuryData> GetAll()
        {
            return new List<IInjuryData>(AvailableInjuries);

        }


        internal List<IInjuryData> GetShownInjuries()
        {
            return (List<IInjuryData>)(_shownInjuries as IInjuryData);
        }

        internal void InjectInjury(ELegInjury einjury)
        {
            LegInjuryInstance injury = LegInjuryDatabase.GetInjury(einjury);
            _currentInjury = injury;
        }



        internal LegInjuryInstance GetActiveInjury()
        {
            return _currentInjury;
        }




        internal void Apply()
        {


            //ApplyRigEffect(_currentInjury);
            //ApplyVisualEffect(_currentInjury);
        }
        /*
        private void ApplyRigEffect(LegInjuryInstance i)
        {
            _controller.InjectInjury(i);
        }

        private void ApplyVisualEffect(LegInjuryInstance i)
        {
            Debug.Log("STEP 1: Visual effect started");


            foreach (var muscleEnum in i.affectedMuscles)
            {
                MuscleController muscle = _controller.MusclesModule.GetMuscle(muscleEnum);

                if (muscle != null)
                {
                    muscle.ChangeVisual();
                }
            }

            if (i.affectedMuscles == null)
            {
                Debug.LogError("affectedMuscles is NULL");
                return;
            }

            Debug.Log("STEP 2: Applying glow");

            foreach (ELegMuscles muscleEnum in i.affectedMuscles)
            {
                if (_muscleMap.TryGetValue(muscleEnum, out MuscleController muscle))
                {
                    muscle.ChangeVisual();
                }
                else
                {
                    Debug.LogWarning($"No MuscleController found for {muscleEnum}");
                }
            }
        }
    */
    }
}