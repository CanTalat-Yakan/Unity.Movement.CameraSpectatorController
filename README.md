# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# Camera Spectator Controller

> Quick overview: Free‑fly spectator camera controller with mouse look, scroll‑wheel dolly, WASD/QE movement, boost, and optional acceleration.

A spectator‑style camera controller intended for scene navigation and debug viewing. Mouse movement is used for pitch/yaw, the scroll wheel translates along the forward axis, and movement uses WASD with Q/E for vertical motion. A boost key may increase speed, an optional acceleration curve can be applied, and a reset key returns the camera to its initial transform.

![screenshot](Documentation/Screenshot.png)

## Features
- Toggleable subsystems
  - Master Active toggle to enable/disable the controller
  - Rotation (mouse X/Y) with configurable sensitivity
  - Forward/back translation via mouse scroll with configurable speed
  - Movement (W/A/S/D + Q/E) with base speed and boost speed
  - Optional speed acceleration with configurable factor
- Input and keys
  - Default keys: WASD for planar, E up, Q down, Left Shift for boost, R to reset
  - Customizable key bindings for boost/up/down/reset
- Quality of life
  - Reset to initial position/rotation via a key
  - Boost speed is validated to be >= base speed in the inspector
- Lightweight
  - Single runtime component; no physics or character controller required

## Requirements
- Unity 6000.0+ (per package manifest)
- Legacy Input Manager axes present: "Mouse X", "Mouse Y", "Mouse ScrollWheel"
  - If using the new Input System only, these axes or equivalents must be provided/adapted

## Usage
1) Add to a Camera
   - Select your Camera and add `CameraSpectatorController`
2) Configure behavior
   - Enable/disable Rotation, Translation (scroll), Movement, and Acceleration as needed
   - Tune `mouseSense`, `translationSpeed`, `movementSpeed`, and `boostedSpeed`
   - Optionally rebind `boost`, `moveUp` (default E), `moveDown` (default Q), and `reset` (default R)
3) Play
   - Move: W/A/S/D; Up: E; Down: Q; Boost: hold Left Shift
   - Look: move mouse; Dolly: mouse scroll
   - Reset: press R to return to the initial transform

## How It Works
- Update loop
  - Scroll wheel applies forward translation scaled by `translationSpeed` and `Time.deltaTime`
  - WASD/QE movement is applied in the camera’s local axes; boost temporarily raises the speed
  - Optional acceleration increases per‑frame distance while the camera is moving
- Rotation
  - Mouse movement adjusts pitch (X) and yaw (Y) with `mouseSense`; roll remains unchanged
- Initialization and validation
  - Initial position/rotation are cached on Start for Reset
  - Inspector validation ensures `boostedSpeed >= movementSpeed`

## Notes and Limitations
- Transform‑based motion without collisions; not a character controller
- Cursor locking/hiding is not managed by default; handle it externally if required
- Rotation is always active when enabled; no mouse‑button gating is included by default
- Requires the legacy Input Manager axes unless adapted for the new Input System

## Files in This Package
- `Runtime/CameraSpectatorController.cs` – Free‑fly spectator camera controller
- `Runtime/UnityEssentials.CameraSpectatorController.asmdef` – Runtime assembly definition

## Tags
unity, camera, spectator, flycam, free‑fly, noclip, movement, input, wasd, runtime, debug
