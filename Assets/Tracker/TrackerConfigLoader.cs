using System.IO;
using UnityEngine;

namespace Tracker
{
    public class TrackerConfigLoader : MonoBehaviour
    {
        [SerializeField] private Transform tracker;
        [SerializeField] private Transform irlObjectWithTracker;
        public string filePath = "trackerParameters.json";
        private TrackerParameters _trackerParameters;
        
        public void Awake()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, filePath);
            _trackerParameters=TrackerOperations.LoadParameters(filePath);
            
            //Parent irlObjectWithTracker and use Transform from saved .json
            irlObjectWithTracker.SetParent(tracker);
            irlObjectWithTracker.localPosition = _trackerParameters.IrlObjectWithTrackerPosition;
            irlObjectWithTracker.localRotation = _trackerParameters.IrlObjectWithTrackerRotation;
        }
    }  
}

