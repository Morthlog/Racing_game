# Computer Graphics Assignment
This project is a 3D racing game developed in Unity for the "Computer Graphics" course at AUEB. The player controls a car on a pre-designed track. The goal is to set a record time by completing 3 full laps.
  
## Core Features
* **Environment:** Modular track synthesis using individual elements.
* **Physics:** Unity's physics engine handles the car's gravity and collision responses.
* **Lighting:** Includes an environment map, realtime lights, baked light probes for global illumination, and reflection probes.
* **VFX:** Particle systems are implemented for smoke and explosion effects.

## Setup & Build Instructions
1. **Requirements:** Download and install **Unity 6.3 LTS**.
2. **Project Load:** Open the project folder through the Unity Hub.
3. **Light Baking (Crucial):** Before building, you must bake the lighting to ensure all visual elements and shaders render correctly.
   * Go to `Window > Rendering > Lighting`.
   * Click **Generate Lighting** to bake the Lightmaps and the **Adaptive Probe Volumes**.
4. **Build:** Go to `File > Build Settings`.
    * Ensure all racing scenes are included in the "Scenes in Build" list.
    * Select your target platform (e.g., PC, Mac & Linux Standalone).
    * Click **Build** and choose a destination folder.

## How to Play
* **Controls:** Move and steer using the **WASD** keys.
* **Power-ups:**
    * Press **Num 1** for an Instant Speed boost.
    * Press **Num 2** to activate a Shield.
* **Objective:** Complete 3 laps as fast as possible.
* **Hazards:** Avoid traps and obstacles that can slow you down or trigger collision events.