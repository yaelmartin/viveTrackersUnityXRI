using System.IO;
using UnityEngine;

namespace Tracker
{
    public struct TrackerParameters
    {
        public float Degree;
        public Vector3 TrackerOriginOffset;
        public Vector3 IrlObjectWithTrackerPosition;
        public Quaternion IrlObjectWithTrackerRotation;
    }
    
    /// <summary>
    /// Retrieves calibration to move a virtual object like the real one
    /// </summary>
    public class TrackerConfigLoader : MonoBehaviour
    {
        [SerializeField] private protected Transform tracker;
        [SerializeField] private protected Transform irlObjectWithTracker;
        public string filePath = "trackerParameters.json";
        private protected TrackerParameters _trackerParameters;
        
        private void Awake()
        {
            SetFilePath();
            
            _trackerParameters=GetParametersFromJSON(filePath);
            
            ParentObjectWithTrackerUsingParameters();
        }

        protected void SetFilePath()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, filePath);
        }

        protected static TrackerParameters GetParametersFromJSON(string file)
        {
            if (File.Exists(file))
            {
                Debug.Log("File " + file + " found!");
                string dataAsJson = File.ReadAllText(file);
                return JsonUtility.FromJson<TrackerParameters>(dataAsJson);
            }
            else
            {
                Debug.Log("File " + file + " not found. Using default Parameters.");
                return new TrackerParameters();
            }
        }

        protected void ParentObjectWithTrackerUsingParameters()
        {
            irlObjectWithTracker.SetParent(tracker);
            irlObjectWithTracker.localPosition = _trackerParameters.IrlObjectWithTrackerPosition;
            irlObjectWithTracker.localRotation = _trackerParameters.IrlObjectWithTrackerRotation;
        }
    }  
}

