# Unity Gameplay Programming Portfolio

A collection of Unity and C# gameplay programming systems developed as part of my personal game development projects.

This repository showcases my approach to gameplay programming, including player systems, camera and input management, combat mechanics, boss behaviour, AI navigation, and gameplay interactions.

The main goal of this repository is to demonstrate the code and gameplay systems I have developed, rather than provide a complete playable build of each project.

---

## Technologies

* Unity
* C#
* Unity Input System
* Unity Cinemachine
* Unity NavMesh
* Git
* Blender

---

## Featured Systems

### 🎮 Player & Camera Systems

The projects include player-focused systems developed using Unity's Input System and Cinemachine.

* Player input management
* Camera follow and LookAt systems
* Manual camera control
* Mouse and gamepad camera input
* Split-screen camera configuration
* Player index-based camera setup

The camera input system supports both mouse and gamepad controls, with different sensitivity handling depending on the input device.

The split-screen setup assigns each player a dedicated camera viewport based on their `PlayerInput` player index.

---

### ⚔️ Combat Systems

The project includes a modular combat system developed in C#.

* Interface-based damage system using `IDamageable`
* Player attack input using Unity's Input System
* Physics-based attack detection using `Physics.OverlapSphere`
* Layer-based enemy filtering
* Configurable attack radius and damage
* Randomized attack animations
* Randomized attack visual effects
* Editor Gizmos for attack range debugging

The `IDamageable` interface allows the attack system to interact with different damageable entities without directly depending on a specific enemy implementation.

This keeps the attack logic decoupled from the individual entities receiving damage.

---

### 👹 Boss AI & Gameplay

The project includes a component-based boss gameplay system built around a state-driven behaviour loop.

The boss architecture separates behaviour, movement, attacks, and fight sequences into dedicated components.

Key features include:

* State-driven boss behaviour using `Moving`, `Waiting`, `Attacking`, and `Dead` states
* Coroutine-based gameplay sequencing
* Randomized movement within a configurable gameplay area
* Unity NavMesh-based navigation
* Destination completion detection
* Abstract attack system allowing different attacks to share a common structure
* Extensible attack behaviours using inheritance
* Telegraphed area-of-effect attacks
* Visual attack indicators giving players time to react
* Configurable attack warning durations and damage areas
* Physics-based area detection using `Physics.OverlapSphere`
* Dedicated boss introduction and end-of-fight sequences
* Dynamic activation and deactivation of boss renderers and colliders

The `BossBrain` acts as the central behaviour controller while delegating movement and attack responsibilities to dedicated components.

The abstract `BossAttack` class provides a common structure for implementing different boss attack behaviours independently.

The `ExplosionAttack` system demonstrates a complete telegraphed attack sequence: a random target position is selected, a warning indicator is displayed for a configurable duration, and an area-of-effect explosion is then triggered.

This separation of responsibilities helps keep individual gameplay systems focused and allows the boss behaviour to be extended with new movement and attack implementations.

---

### 🕹️ Gameplay Systems

The project also includes reusable gameplay systems for interactions between players and the game world.

* Trigger-based interactions
* Scene transitions
* Configurable scene transition delays
* Optional visual effects
* Inspector-based configuration
* Editor Gizmos for gameplay trigger visualization

The `SceneChangeTrigger` system provides a reusable way to trigger scene transitions when a player enters a designated trigger zone.

---

## Project Structure

```text
Assets/
|- Scripts/
|   |- Fight/
|   |   |- IDamageable.cs
|   |   |- PlayerAttack.cs
|   |
|   |- Boss/
|   |   |- BossBrain.cs
|   |   |- BossMovement.cs
|   |   |- BossAttack.cs
|   |   |- Evenement/
|   |   |   |-BossEvent.cs
|   |   |   |-BossIntro.cs
|   |   |   |-ArenaController.cs
|   |
|   |- Player/
|   |   |- PlayerIndexAssigner.cs
|   |   |- PlayerMove.cs
|   |- Vehicule/
|   |   |- VehiculeControler.cs
|   |   |- VehiculeRespawn.cs
|   |
|   |- Camera/
|   |   |- CinemachineManualInput.cs
|   |   |- PlayerSetup.cs
|   |
|   |- Gameplay/
|       |- SceneChangeTrigger.cs
|
|- Scenes/
|- Prefabs/
|- Animations/
|- ...
```

> The repository structure may evolve as new gameplay systems and projects are added.

---

## Gameplay Showcase

A gameplay video demonstrating the project and its gameplay systems is available below.

🎥 **Gameplay Video:**
[Watch the gameplay on YouTube]()

---

## About Me

I am currently a student at 42, focusing on software development and looking to pursue a career in game programming.

I have been developing games and gameplay systems with Unity and C# through personal projects and my studies.

I am particularly interested in gameplay programming, game systems, player interactions, AI, and software architecture.

---

## Contact

**Morgan Bogey**

* GitHub: https://github.com/m-bogey
* Email: bogeymorgan@outlook.com

