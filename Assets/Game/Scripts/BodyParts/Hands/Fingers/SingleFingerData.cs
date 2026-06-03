using UnityEngine;

namespace Game.Body
{

    [System.Serializable]
    public class SingleFingerData
    {
        public string FingerName;
        [SerializeField] private EFingerTypes type;

        [SerializeField] private Transform[] bones;

        [Header("Spacing")]
        public Transform root;
        public Vector3 spreadDirection = Vector3.right;
        public float spreadAmount = 0.01f;

        [Tooltip("Thumb -2 ... Pinky +2")]
        public int spreadIndex = 0;
        public float spreadWeight = 1f;

        public Transform[] Bones => bones;
        public EFingerTypes Type => type;

        [HideInInspector] public Quaternion[] restRotations;

        [HideInInspector] public float signalCurl;
        [HideInInspector] public float targetCurl;
        [HideInInspector] public float currentCurl;
        [HideInInspector] public Vector3 restLocalPos;

        public Vector3 curlAxis = Vector3.forward;

        public int BoneCount => bones.Length;

        public Transform GetBone(int i) => bones[i];
        public Quaternion GetRestRotation(int i) => restRotations[i];
    }
}
