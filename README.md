# Slingshot Shinobi

Slingshot Shinobi is a top-down 2D roguelike RPG built in Unity. Players control a
shinobi who fights through progressively harder levels using slingshot-based
projectiles, movement, dashes, and power-ups.

## Core Features

- Slingshot-inspired ranged combat with automatic targeting and projectile firing
- Fast dash movement with area-of-effect damage
- Enemy types with different movement, attack, and audio behaviors
- Boss encounters with dedicated health bars and attacks
- Wave-based levels with additional random enemy spawns
- Progressive difficulty from Level 1 through Level 5
- Collectible shurikens that activate randomized power-up choices
- Health, dash cooldown, collection, and boss health UI
- Tutorial sequence for movement, combat, dashing, and power-ups
- Pause, restart, game-over, and victory flows
- Level selector with sequential level unlocking
- Background music, combat sounds, enemy voices, and victory audio
- Android build support for mobile play

## Level Progression

Each level combines planned enemy waves with a finite number of random enemies.
The difficulty increases gradually:

| Level | Waves | Enemies per wave | Random enemies | Special challenge |
| --- | ---: | ---: | ---: | --- |
| 1 | 1 | 10 | 20 maximum | Introduction to the main loop |
| 2 | 2 | 12 | 30 maximum | More enemies and tighter pacing |
| 3 | 2 | 16 | 40 maximum | Heavier encounters |
| 4 | 3 | 20 | 50 maximum | High-density combat |
| 5 | 4 | 24 | 60 maximum | Final boss encounter |

A level is completed only after all of its spawned enemies have been defeated.
Clearing a level unlocks the next level in the selector.

## Gameplay Systems

### Combat

The player automatically targets nearby enemies and fires pebbles from the
slingshot. Enemies pursue the player and attack when within range. The dash gives
the player a fast repositioning tool and can damage multiple enemies at once.

### Power-Ups

Collecting shurikens fills the collection meter and opens a power-up selection.
Available upgrades include healing, enemy slowing, dash cooldown reduction,
increased dash area and distance, faster pebble attacks, increased movement speed,
and temporary infinite dashes.

### Mobile Experience

The game is designed for touch controls and includes a virtual joystick, swipe
dashing, responsive camera following, and Android build support.

## Development

- Engine: Unity 2022.3.62f3
- Genre: Top-down 2D roguelike RPG
- Target platform: Android and desktop development builds
- Main technologies: Unity 2D, Tilemaps, TextMeshPro, Unity UI, physics, audio,
	animation, and scene-based progression

## Contributions

Slingshot Shinobi was developed collaboratively by both contributors. Gameplay,
creative direction, implementation, testing, and development decisions were shared
across the project.

### Meerab Munir - `fastcel`

- Level and map design across the playable areas
- Environment layout, visual tile arrangements, and map presentation
- Music and broader audio integration
- Level progression, level selection, and unlock flow
- Sprite and visual asset integration
- Power-up design and implementation
- Tutorial flow and player onboarding
- Tutorial instructions for movement, combat, dashing, and power-ups
- UI presentation and progression feedback
- Gameplay balancing and overall development

### Shehryar Hassan - `ShehryarHassan789`

- Core slingshot combat mechanics and projectile implementation
- Player targeting and ranged attack behavior
- Dash mechanics, movement, and dash-based area damage
- Enemy and boss gameplay interactions
- Combat systems and gameplay programming
- Gameplay balancing and overall development

Both contributors participated in development, iteration, debugging, testing, and
bringing the complete game experience together.

## Project Structure

- `Assets/Scenes/` - Title screen, tutorial, level selector, and playable levels
- `Assets/Scripts/` - Player, enemy, boss, combat, UI, audio, progression, and
	game-state systems
- `Assets/Sprites/` and `Assets/graphics/` - Characters, environments, tiles, UI,
	and other visual assets
- `Assets/Audios/` - Music and sound effects

## Building for Android

1. Open the project in Unity 2022.3.62f3.
2. Switch the build platform to Android in `File > Build Settings`.
3. Confirm the scenes are enabled in the build list.
4. Keep `Build App Bundle (Google Play)` unchecked for an APK.
5. Select at least one Android target architecture, preferably ARM64.
6. Click `Build` and save the output with an `.apk` extension.

## Credits

**Meerab Munir (`fastcel`)** and **Shehryar Hassan (`ShehryarHassan789`)**

Collaborative development, design, implementation, and testing.
