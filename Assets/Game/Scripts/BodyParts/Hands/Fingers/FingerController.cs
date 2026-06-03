using System;
using UnityEngine;


namespace Game.Body
{
    public class FingerController : MonoBehaviour
    {
        [Header("Debug EMG Driver")]
        public bool useDebugDriver = true;

        private bool _fingerControls = false;

        [Range(0f, 4f)] public float strength = 1f;
        [Range(0.1f, 10f)] public float speed = 2f;
        [Range(-1f, 1f)] public float offset = 0f;
        [Range(0f, 1f)] public float noise = 0f;


        [Header("Idle Animation")]
        [SerializeField] private bool useIdle = true;

        [SerializeField] private float idleSpeed = 1.2f;
        [SerializeField] private float idleCurlAmount = 0.03f;
        [SerializeField] private float idleTwistAmount = 0.03f;
        [SerializeField] private float idleSpreadAmount = 0.02f;
        private float _idleTime;
        public SingleFingerData[] fingers;
        public PalmData palm;
        private HandInjuryTypes _activeHandInjury;

        internal void Initialize(Action onInit)
        {
            palm.restRotation = palm.palmRoot.localRotation;

            foreach (var finger in fingers)
            {
                int count = finger.BoneCount;
                finger.restRotations = new Quaternion[count];

                finger.restLocalPos = finger.root.localPosition; // IMPORTANT FIX

                for (int i = 0; i < count; i++)
                {
                    finger.restRotations[i] = finger.GetBone(i).localRotation;
                }
            }



            _fingerControls = true;

            onInit?.Invoke();
        }

        void Update()
        {
            if (!_fingerControls) return;
            _idleTime += Time.deltaTime;
            for (int i = 0; i < fingers.Length; i++)
            {
                ApplyFinger(fingers[i]);
            }

            ApplyPalm();
        }
        private void ApplyFinger(SingleFingerData finger)
        {
            finger.currentCurl = Mathf.Lerp(
                finger.currentCurl,
                finger.targetCurl,
                Time.deltaTime * 10f
            );

            float curl = finger.currentCurl;

            if (useIdle)
            {
                float phase = _idleTime * idleSpeed + finger.spreadIndex * 0.35f;

                curl += Mathf.Sin(phase) * idleCurlAmount;

                curl += Mathf.PerlinNoise(
        finger.spreadIndex * 10f,
        _idleTime * 0.5f
    ) * 0.01f;
            }

            // =========================
            // FIXED NATURAL SPREAD
            // =========================
            float spreadFactor = Mathf.SmoothStep(0f, 1f, palm.currentSpread);

            finger.root.localPosition =
                finger.restLocalPos +
                finger.spreadDirection *
                (finger.spreadIndex * spreadFactor * finger.spreadAmount * finger.spreadWeight);

            // =========================
            // CURL BONES (ANATOMICAL)
            // =========================
            for (int i = 0; i < finger.BoneCount; i++)
            {
                float t = (i + 1f) / finger.BoneCount;

                float shape = Mathf.SmoothStep(0f, 1f, t);
                shape = Mathf.Pow(shape, 1.2f);
                float jointBias;

                // MCP (base) → strongest
                if (i == 0)
                {
                    jointBias = 1.0f;
                }
                // PIP (middle) → main bending joint
                else if (i == 1)
                {
                    jointBias = 0.85f;
                }
                // DIP (tip) → subtle follow-through
                else
                {
                    jointBias = 0.6f;
                }


                SingleFingerInjuryData injury = null;

                if (_activeHandInjury != null)
                {
                    injury = GetFingerInjury(_activeHandInjury, finger.Type);
                }

                if (injury != null)
                {
                    // strength loss
                    curl *= (1f - injury.strengthLoss);

                    // stable nerve noise
                    curl += (
                        Mathf.PerlinNoise(
                            finger.spreadIndex * 50f,
                            Time.time * 2f
                        ) * 2f - 1f
                    ) * injury.noise;

                    // tremor
                    curl += Mathf.Sin(Time.time * 25f) * injury.tremor;

                    // range limit
                    float maxCurl = 1f - injury.rangeLoss;
                    curl = Mathf.Clamp(curl, 0f, maxCurl);
                }

                float angle = curl * shape * 170f * jointBias;

                finger.GetBone(i).localRotation =
                    finger.GetRestRotation(i) *
                    Quaternion.AngleAxis(angle, finger.curlAxis);
            }
        }


        internal void InjectInjury(HandInjuryTypes injury)
        {
            Debug.Log("injected fucker");
            _activeHandInjury = injury;
        }

        private SingleFingerInjuryData GetFingerInjury(
        HandInjuryTypes handInjury, EFingerTypes fingerType)
        {
            switch (fingerType)
            {
                case EFingerTypes.Thumb:
                    return handInjury.thumb;

                case EFingerTypes.Index:
                    return handInjury.index;

                case EFingerTypes.Middle:
                    return handInjury.middle;

                case EFingerTypes.Ring:
                    return handInjury.ring;

                case EFingerTypes.Pinky:
                    return handInjury.pinky;
            }

            return null;
        }

        private void ApplyPalm()
        {
            palm.currentCurl = Mathf.Lerp(palm.currentCurl, palm.curl, Time.deltaTime * 8f);
            palm.currentSpread = Mathf.Lerp(palm.currentSpread, palm.spread, Time.deltaTime * 8f);
            palm.currentTwist = Mathf.Lerp(palm.currentTwist, palm.twist, Time.deltaTime * 8f);



            float idleTwist = 0f;
            float idleSpread = 0f;

            if (useIdle)
            {
                idleTwist =
                    Mathf.Sin(_idleTime * idleSpeed * 0.7f)
                    * idleTwistAmount;

                idleSpread =
                    Mathf.Sin(_idleTime * idleSpeed * 0.5f + 1.4f)
                    * idleSpreadAmount;
            }

            Quaternion curlRot =
                Quaternion.AngleAxis(palm.currentCurl * 45f, palm.palmRoot.forward);

            Quaternion spreadRot =
                Quaternion.AngleAxis(
                    ((palm.currentSpread + idleSpread) - 0.5f) * 20f,
                    palm.palmRoot.right
                );

            Quaternion twistRot =
                Quaternion.AngleAxis(
                    ((palm.currentTwist + idleTwist) - 0.5f) * 30f,
                    palm.palmRoot.forward
                );

            palm.palmRoot.localRotation =
                palm.restRotation *
                curlRot *
                spreadRot *
                twistRot;
        }

        // ===============================
        // HIGH-LEVEL HAND API
        // ===============================

        internal void Grab(float strength = 1f)
        {
            palm.curl = strength;
        }

        public void Release()
        {
            palm.curl = 0f;
        }

        public void Spread(float value)
        {
            palm.spread = value;
        }

        public void Twist(float value)
        {
            palm.twist = value;
        }

        internal void SetFingerCurl(EFingerTypes type, float curl)
        {
            curl = Mathf.Clamp(curl, -180, 180f);
            curl = curl / 180f;
            foreach (var f in fingers)
            {
                if (f.Type == type)
                {
                    f.targetCurl = curl;
                }
            }
        }

        public void SetGesturePosition(HandPositions positions)
        {
            //Debug.Log("set hand");
            foreach (var gest in positions.FingerPositions)
            {
                SetFingerCurl(gest.FingerType, gest.FingerCurl);
            }

            SetPalm(
                positions.PalmCurl,
                positions.PalmSpread,
                positions.PalmTwist
            );
        }

        public void SetFingers(EFingerTypes[] types, float curl)
        {
            foreach (var t in types)
            {
                SetFingerCurl(t, curl);
            }
        }

        public void StretchFinger(EFingerTypes type, float stretch)
        {
            float curl = 1f - Mathf.Clamp01(stretch);

            foreach (var f in fingers)
            {
                if (f.Type == type)
                    f.signalCurl = curl;
            }
        }

        private void SetPalm(float curl, float spread, float twist)
        {
            palm.curl = Mathf.Clamp01(curl);
            palm.spread = Mathf.Clamp01(spread);
            palm.twist = Mathf.Clamp01(twist);
        }
    }

}