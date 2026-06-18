using System.Collections.Generic;
namespace Game.Body

{
    [System.Serializable]
    public class HandPositions
    {
        public EHandGestures HandGesture;
        public List<FingerPositions> FingerPositions;
        public string GestureKey;
        public float PalmCurl;
        public float PalmSpread;
        public float PalmTwist;
        public string KeyName;
    }
}