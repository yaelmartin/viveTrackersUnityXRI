# viveTrackersUnityXRI v1.4

## Introduction
viveTrackersUnityXRI is a Unity package that enables the use of SteamVR trackers in Unity XRI 3 and simplifies the process of matching real objects with virtual ones through easy calibration.

[![Video preview](https://github.com/user-attachments/assets/21f6b937-8379-4259-87e1-3eee6da9da37)](https://youtu.be/8gWOsPiG3_I?si=tNO_YpnL_Y1LkK_v "Use real objects with trackers in Unity XR Interaction Toolkit 3")


## Features

- SteamVR tracker integration with Unity XRI 3
- Calibration of offset between real objects and trackers
- Room-scale area alignment with screens
- Support for multiple trackers and real objects (with or without HMD)
- Compatible with 12 SteamVR tracker roles

## Use Cases

- Virtual Production (see [https://github.com/yaelmartin/screenPerspective](https://github.com/yaelmartin/screenPerspective))
- Training simulation, games (with or without HMD)

## Supported SteamVR Tracker Roles

|  |  |  |
|---|---|---|
| Left Foot | Right Foot | Waist |
| Left Shoulder | Right Shoulder | Chest |
| Left Elbow | Right Elbow | Camera |
| Left Knee | Right Knee | Keyboard |

Note: For single tracker use, the VTSingle preset is available. However, when multiple trackers are active, use dedicated presets to avoid mixed positional data.

## Installation and Setup

### For VR Projects (e.g., VRCore template):
Assuming you only need to use trackers inside Unity without the other features of this project,
1. Import the following from the package:
   - ViveTrackers.inputactions
   - HTCViveTrackerHapticProfile.cs
   - Preset folder
   - Resources folder

2. Add the trackers interaction profile:
   - Go to Edit → Project Settings → XR Plug-in Management → OpenXR
   - Under Enabled Interaction Profiles, click '+' and select "HTC Vive Tracker Profile"

3. In your scene hierarchy:
   - Find your InputActionManager
   - Add ViveTrackers.inputactions to its Action Assets

4. Place a tracker prefab with a role (e.g., TrackerKeyboard) as a child of XROrigin → Camera Offset

### For New Projects:

1. Ensure you are using Unity Editor 2021.3 or higher
2. Install "XR Interaction Toolkit" version 3 from the Package Manager
3. Import the latest viveTrackersUnityXRI.unitypackage
4. Add the trackers interaction profile (as described above)
5. Use prefabs from Assets → Tracker → Demos in your scene and unpack them

## Calibration

Proper calibration ensures accurate representation of real objects in the virtual space.

1. Ideally use a 1:1 CAD model or 3D scan of your real object as the virtual irlObjectWithTracker
2. Place your model inside MovableArea
3. Select TrackerOperations and link your model to the irlObjectWithTracker field in the inspector
4. Position the "Spawn" (pink T-shaped object) where the tracker would be on your real object
5. Click Play and place your real object in front of your screen
6. Use the sliders to align the yaw axis
7. Recenter the tracker

Tips:
- Make forward and backward motions while viewing the bottom orthographic view
- Use a VR headset with passthrough to verify offset accuracy

After calibration:
- Save the offset (creates a .json file in the StreamingAssets folder)
- The configuration will load automatically on Play if the file exists

## Applying Saved Offset in VR

1. Add the component TrackerConfigLoader.cs to a tracker prefab (e.g., VTSingle, TrackerCamera)
2. Change the filePath field to use your configured file

## Using Lighthouse Trackers Without HMD

1. Edit C:\Program Files (x86)\Steam\config\steamvr.vrsettings
2. Add the following to the "steamvr" section:
   ```json
   "requireHmd": false,
   "forcedDriver": "null",
   "activateMultipleDrivers": true
   ```
3. Add the following to the "dashboard" section:
   ```json
   "enableDashboard": false
   ```
4. Use nullDriverEnable.ps1 or nullDriverDisable.ps1 by right-clicking and selecting "Run with PowerShell"

Note: To fix a Unity Editor and SteamVR null driver [bug that freezes Windows](https://github.com/ValveSoftware/steamvr_unity_plugin/issues/990), edit the following file:
C:\Program Files (x86)\Steam\steamapps\common\SteamVR\drivers\null\resources\settings\default.vrsettings

Remove these lines:
```
"windowX": 0,
"windowY": 0,
"windowWidth": 2160,
"windowHeight": 1200,
```

## Credits

- HTCViveTrackerProfile.cs from [Vive's forums](https://forum.htc.com/topic/14370-tutorial-openxr-pc-vr-how-to-use-vive-tracker/?do=findComment&comment=55772) and haptics from [https://github.com/mbennett12/ViveTrackerHapticOpenXR](https://github.com/mbennett12/ViveTrackerHapticOpenXR)

- FreeFlyCamera.cs from [Sergey Stafeev](https://assetstore.unity.com/packages/tools/camera/free-fly-camera-140739)
