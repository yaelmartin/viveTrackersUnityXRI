using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tracker
{
    public struct TrackerParameters
    {
        public float Degree;
        public Vector3 TrackerOriginOffset;
        public Vector3 IrlObjectWithTrackerPosition;
        public Quaternion IrlObjectWithTrackerRotation;

        public TrackerParameters(int dummyInt)
        {
            Degree = 0;
            TrackerOriginOffset = Vector3.zero;
            IrlObjectWithTrackerPosition = Vector3.zero;
            IrlObjectWithTrackerRotation = new Quaternion();
        }
    }
    public class TrackerOperations : MonoBehaviour
    {
        [SerializeField] private Transform trackerOrigin;
        [SerializeField] private Transform tracker;
        [SerializeField] private Transform spawn;
        [SerializeField] private Transform irlObjectWithTracker;
        [SerializeField] private ActionBasedController trackerActionBasedController;

        public string filePath = "trackerParameters.json";
        private TrackerParameters _trackerParameters;

        private Vector3 _initialIrlObjectWithTrackerPosition;
        private Quaternion _initialIrlObjectWithTrackerRotation;
        
        public void RecenterTrackerToSpawn()
        {
            Vector3 offset = spawn.transform.position;
            offset = offset - (tracker.position - trackerOrigin.position);
            trackerOrigin.position = offset;

            _trackerParameters.TrackerOriginOffset = offset;

            UnParentIrlObjectAndReparent();
        }

        //must be called when the Tracker is Recentered first and hasn't moved since
        public void UnParentIrlObjectAndReparent()
        {
            irlObjectWithTracker.parent = null;
            irlObjectWithTracker.position = _initialIrlObjectWithTrackerPosition;
            irlObjectWithTracker.rotation = _initialIrlObjectWithTrackerRotation;
            irlObjectWithTracker.SetParent(tracker,true);
            
            //save relative local transform of irlObjectWithTracker
            _trackerParameters.IrlObjectWithTrackerPosition = irlObjectWithTracker.localPosition;
            _trackerParameters.IrlObjectWithTrackerRotation = irlObjectWithTracker.localRotation;
        }
        
        public void AlignSpace(float degree)
        {
            trackerOrigin.rotation=Quaternion.Euler(new Vector3(0,degree,0));
            _trackerParameters.Degree = degree;
        }
        
        public void AlignSpaceAndRecenter(float degree)
        {
            AlignSpace(degree);
            RecenterTrackerToSpawn();
        }

        public void ToggleTracker(bool state)
        {
            trackerActionBasedController.enabled = state;
        }


        public void Awake()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, filePath);
            spawn.parent = null;
            _initialIrlObjectWithTrackerPosition = irlObjectWithTracker.position;
            _initialIrlObjectWithTrackerRotation = irlObjectWithTracker.rotation;
            
            LoadParameters();
        }
        
        public void LoadParameters()
        {
            if (File.Exists(filePath))
            {
                Debug.Log("File " + filePath + " found!");
                string dataAsJson = File.ReadAllText(filePath);
                _trackerParameters = JsonUtility.FromJson<TrackerParameters>(dataAsJson);
            }
            else
            {
                Debug.Log("File " + filePath + " not found. Using default Parameters.");
                _trackerParameters = new TrackerParameters();
            }
            
            ApplyParameters();
        }

        public void ApplyParameters()
        {
            trackerOrigin.position = _trackerParameters.TrackerOriginOffset;
            trackerOrigin.rotation=Quaternion.Euler(new Vector3(0,_trackerParameters.Degree,0));
            
            //Parent irlObjectWithTracker and use Transform from saved .json
            irlObjectWithTracker.SetParent(tracker);
            irlObjectWithTracker.localPosition = _trackerParameters.IrlObjectWithTrackerPosition;
            irlObjectWithTracker.localRotation = _trackerParameters.IrlObjectWithTrackerRotation;
        }
        
        public void SaveParameters()
        {
            string dataAsJson = JsonUtility.ToJson(_trackerParameters);
            File.WriteAllText(filePath, dataAsJson);
        }

        public void DeleteParameters()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("File deleted.");
            }
            else
            {
                Debug.Log("No file to delete.");
            }
        }

    }
}
