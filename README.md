# StillFrame

StillFrame is a 2D action platformer developed using the Unity 6.2.0 engine. The project focuses on the implementation of fluid character movement, responsive combat mechanics, enemy artificial intelligence, and integrated animation systems. This project serves as a practical application of game development principles and C# programming.



## Project Overview

StillFrame is a combat-oriented platformer where the player must navigate through various levels and defeat different types of enemies. The primary objective was to create a seamless user experience by balancing fast-paced movement with precise combat mechanics.

Furthermore, this project functioned as a comprehensive learning exercise in software engineering, specifically regarding debugging procedures, animation state management, and the architectural design of gameplay systems.


## Core Features

### Character Movement
The player control system includes several technical components:
* Horizontal movement and jumping mechanics.
* Character orientation and sprite flipping.
* Physics-based movement utilizing the Rigidbody2D component.
* Optimized input response to ensure smooth control during gameplay.

### Combat System
The combat architecture allows the player to engage enemies through melee interactions. Key features include:
* An input detection system for attacking.
* Proximity detection to determine attack range.
* A health and damage pipeline for both players and enemies.
* Implementation of cooldown periods to balance attack frequency.
* A parry mechanic that requires precise timing for defensive maneuvers.

### Enemy Artificial Intelligence
To provide a challenging experience, different AI behaviors were developed:

#### Melee Entities
* Autonomous detection of the player's position.
* Pathfinding logic to move toward the target.
* Stopping distance logic to prevent overlapping with the player.
* Close-range attack triggers with internal cooldown timers.

#### Ranged Entities (Archers)
* Long-distance player detection.
* Static orientation to face the player's current coordinates.
* Projectile instantiation logic (shooting arrows).
* Ranged pressure tactics to force player movement.

### Projectile Architecture
The arrow system operates through the following logic:
* Spawning at a specific fire point relative to the enemy.
* Directional velocity applied upon instantiation.
* Collision logic that applies damage to the player.
* Memory management through the destruction of objects upon impact or after a specific duration.


## Animation Framework
The project utilizes Unity Animator Controllers to manage the visual states of characters. This involves the use of Finite State Machines (FSM) to transition between different clips:
* Idle and running states.
* Jumping and falling transitions.
* Combat-related animations (Attack, Hurt, Death).

Transitions were carefully calibrated to reduce latency and improve the tactile feel of the gameplay.



## Technical Specifications
* **Engine:** Unity 6.2.0
* **Programming Language:** C#
* **Physics Components:** Rigidbody2D, BoxCollider2D, and CircleCollider2D.
* **Systems:** Unity Mecanim Animator and Prefab workflow.



## Project Structure

```text
Assets/
├── Scripts/
│   ├── PlayerMovement.cs
│   ├── PlayerCombat.cs
│   ├── Health.cs
│   ├── ArcherAI.cs
│   ├── MeleeEnemy.cs
│   └── Arrow.cs
├── Animations/
├── Prefabs/
├── Sprites/
└── Scenes/

## Development Challenges and Solutions
Animation Logic Conflicts
Enemy attack animations occasionally failed to trigger or were interrupted by movement states. This was resolved by refining the transition parameters and using animation layers to prioritize combat actions over basic locomotion.

Rendering and Visibility
Instances occurred where enemies were not visible during runtime. This was identified as a conflict between transform scales and sprite sorting layers. The solution involved standardizing the Sorting Layer hierarchy and ensuring that animation keyframes did not inadvertently alter the Z-axis position.

Projectile Directionality
Ranged enemies initially faced difficulties with projectile accuracy. This required a revision of the fire point logic and ensuring that the instantiated arrows inherited the correct directional vectors based on the enemy's orientation.

Detection Accuracy
Inconsistent hit registration was addressed by reviewing Layer Masks and the physics matrix. By adjusting the radius of the overlap circle and verifying object tags, the precision of the damage system was significantly increased.

## Future Development
The following updates are planned to expand the scope of the project:

Implementation of a Graphical User Interface (GUI) for health bars and menus.

Integration of spatial sound effects.

Development of advanced Boss AI with complex patterns.

Expansion of the combat system to include combo sequences.

Persistent data management through a save system.

## Execution Instructions
Clone the repository using the following command:
git clone https://github.com/HassanEldeeb8/StillFrame.git

Open the directory within Unity Hub.

Ensure the project is set to run on Unity version 6.2.0.

Load the primary scene located in the Scenes folder and enter Play Mode.

Developer: Created by Hassan Ayman.
License: This project is intended for educational and portfolio demonstration purposes.
