using System.Collections.Generic;
using UnityEngine;

namespace Game.Body
{

    public class LegInjuryInjector : MonoBehaviour
    {
        [SerializeField] private LegController _controller;

        private LegInjuryInstance _currentInjury;




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