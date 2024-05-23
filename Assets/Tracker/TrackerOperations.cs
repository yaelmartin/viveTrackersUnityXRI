using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Tracker
{
    /// <summary>
    /// Used to calibrate a irl object and a tracker
    /// </summary>
    public class TrackerOperations : TrackerConfigLoader
    {
        [SerializeField] private Transform trackerOrigin;
        [SerializeField] private Transform spawn;
        [SerializeField] private TrackedPoseDriver trackerTrackedPoseDriver;
        
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
            trackerTrackedPoseDriver.enabled = state;
        }
        
        private void Awake()
        {
            SetFilePath();
            
            spawn.parent = null;
            
            _initialIrlObjectWithTrackerPosition = irlObjectWithTracker.position;
            _initialIrlObjectWithTrackerRotation = irlObjectWithTracker.rotation;
            
            LoadParameters();
        }

        public void LoadParameters()
        {
            _trackerParameters=GetParametersFromJSON(filePath);
            
            trackerOrigin.position = _trackerParameters.TrackerOriginOffset;
            trackerOrigin.rotation=Quaternion.Euler(new Vector3(0,_trackerParameters.Degree,0));
            
            ParentObjectWithTrackerUsingParameters();
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
