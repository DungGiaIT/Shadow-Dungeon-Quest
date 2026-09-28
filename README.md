# Shadow Dungeon

## 3D Programming Final Project

Shadow Dungeon is a cooperative 3D multiplayer dungeon game developed as a
final project for 3D Programming. The project is customized from Unity's
multiplayer sample and focuses on applying real-time networking concepts in a
playable game. This repository contains the complete Unity project used for
the DemoDungeon implementation.

The project demonstrates:

- Cooperative multiplayer gameplay with Unity Netcode for GameObjects.
- Server-authoritative character movement and combat.
- Multiplayer sessions and Relay connectivity through Unity Gaming Services.
- A customized dungeon-themed interface and branding.
- Keyboard movement with `W/A/S/D` and runtime network statistics with `X`.

## Requirements

- Unity Hub.
- Unity `6000.0.52f1`.
- Git and Git LFS.
- The Unity installation should include the target platform support module
  (Windows, macOS, or Linux Standalone).

## Run the project after cloning

### 1. Clone the repository

```bash
git clone https://github.com/Hoang271205/DemoDungeon.git
cd DemoDungeon
```

### 2. Download large assets

```bash
git lfs install
git lfs pull
```

### 3. Open the project in Unity

1. Open **Unity Hub**.
2. Select **Add** and choose the cloned `DemoDungeon` folder.
3. Open the project with Unity `6000.0.52f1`.
4. Wait for Unity to import the assets and finish compiling the project.

### 4. Run the game

Press the **Play** button in the Unity Editor. From the main menu:

- Select **START SESSION** to create or join an online multiplayer session.
- Select **START WITH DIRECT IP** for a local/direct-IP session.

The first launch may take longer while Unity imports assets and builds the
Library cache.

## Controls

- `W/A/S/D`: Move the character.
- Left mouse button: Select a target or interact with the game world.
- `1`, `2`, `3`: Use hero actions.
- `4`: Open the emote bar.
- `X`: Toggle Runtime Network Stats.

## Project information

- Unity version: `6000.0.52f1`
- Multiplayer framework: Unity Netcode for GameObjects
- Online connectivity: Unity Gaming Services Sessions and Relay
- Project type: Cooperative 3D multiplayer game
