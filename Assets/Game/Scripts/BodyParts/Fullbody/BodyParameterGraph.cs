using System.Collections.Generic;
using UnityEngine;


namespace Game.Body
{
    public class BodyParameterGraph : MonoBehaviour
    {
        private class RuntimeImpulse
        {
            public string target;
            public float strength;
            public float duration;

            public float decayDelay;
            public float priority;
            public bool persistent;

            public float time;
            public AnimationCurve shape;
        }

        private readonly List<RuntimeImpulse> impulses = new();

        public float baseStepLength = 1f;
        public float baseStepHeight = 0.2f;
        public float baseRightStride = 1f;
        public float baseRightHeight = 1f;
        public float baseHipShift = 0f;
        public float baseInstability = 0f;

        public void AddImpulse(BodyImpulse impulse)
        {
            impulses.Add(new RuntimeImpulse
            {
                target = impulse.target,
                strength = impulse.strength,
                duration = impulse.duration,
                shape = impulse.shape,
                decayDelay = impulse.decayDelay,
                priority = impulse.priority,
                persistent = impulse.persistent
            });
        }

        void Update()
        {
            for (int i = impulses.Count - 1; i >= 0; i--)
            {
                impulses[i].time += Time.deltaTime;

                if (!impulses[i].persistent && impulses[i].time > impulses[i].duration)
                {
                    impulses.RemoveAt(i);
                }
            }
        }

        public float Evaluate(string param)
        {
            float value = GetBase(param);

            float weightSum = 0f;

            for (int i = 0; i < impulses.Count; i++)
            {
                var imp = impulses[i];
                if (imp.target != param) continue;

                float t = imp.time;

                float influence = 1f;

                if (t > imp.decayDelay)
                {
                    float fadeT = Mathf.InverseLerp(
                        imp.decayDelay,
                        imp.duration,
                        t
                    );

                    influence = 1f - imp.shape.Evaluate(Mathf.Clamp01(fadeT));
                }

                float weight = imp.priority;
                float delta = imp.strength * influence * weight;

                value += delta;
                weightSum += weight;
            }

            // normalize so multiple impulses don't explode values
            if (weightSum > 0.0001f)
                value /= (1f + weightSum * 0.1f);

            return value;
        }

        float GetBase(string param)
        {
            return param switch
            {
                "stepLength" => baseStepLength,
                "stepHeight" => baseStepHeight,
                "rightStride" => baseRightStride,
                "rightHeight" => baseRightHeight,
                "hipSideShift" => baseHipShift,
                "instability" => baseInstability,
                _ => 1f
            };
        }
    }
}