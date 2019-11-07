# LastTry

### Introduction of the game:
This is a mobile RPG hack'n slash game with perma death system.

## Gameplay:
The main objective is to kill all the enemies in a stage. Once every enemies are dead the player can choose which stat to upgrade from damage, health and special. Player also get to choose a random weapon/item at each stage end. Player can also upgrade weapon and armour. Upgrade money is acquired through killing enemies. Boss enemies drop special items. Player can also upgrade passive skills by acquiring separate money for it through killing enemies. Player will lose all items, weapons, money, passive money and upgrades when player dies. Player will start from the start when dead.

## Table of Contents:
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Controls](#controls)
- [Versioning](#versioning)
- [Authors](#authors)
- [License](#license)

## Prerequisites:
#### Unity Game Engine
Latest stable Unity version is 2019.2.9f1. Install Unity game engine from version 2019.2.9f1 or after. There are two ways to install Unity, as follows:

1. [The latest version of Unity](https://store.unity.com/download-nuo): Download the latest version of Unity from the link and follow the installation instructions to install Unity. Once installation is done start Unity and open the project. If the downloaded Unity version is higher than the stable Unity version then a prompt will ask you to update the project automatically for being able to work with the latest Unity version. This process may break the project if the project's Unity version is too backdated.

2. [Unity Hub (recommended)](https://store.unity.com/download?ref=personal): Download the Unity Hub. Follow the installation instructions to install Unity Hub. Once installation is done open the Unity Hub. Select the "Installs" tab and then in the "Installs" tab click the "Add" button. A prompt will open with many Unity versions. Select the Unity version that matches the Unity version of the stable and click "Next". Follow the installation instruction. Once the Unity version is done installing then open the project in Unity. You can also use the hub to install the latest version of Unity. If you do install the latest version of Unity then please follow the instructions in point *1.* for opening the project in the latest version of Unity.

#### *(Optional)* Android Studio
Latest stable Android Studio version is 3.4 Build #AI-183.5429.30.34.5452501, built on April 10, 2019. Install Android Studio from 3.4 Build #AI-183.5429.30.34.5452501 or latest version of Android Studio. [Download Android Studio from here](https://developer.android.com/studio). After the download then install the Android Studio and follow the installation instructions.

## Installation
Download the latest stable version of the project and open it up in the Unity. It is that simple. If stable's version causes issue then please follow point *2.* in [Prerequisites](#prerequisites).

## Usage
Open the project in Unity. Navigate to Scene1.unity and open it which is in Assets/LastTry/\_Scenes/Scene1.unity

![Navigate to Scene1.Unity](https://i.imgur.com/7sBWzhg.png)
*Figure1: Navigating to Scene1.Unity*

Once Scene1.unity has been opened then set the flag "Is Joy Pad" to true. The find the flag navigate in the "Hierarchy" tab to find the PlayerHolder which is in Root/Characters/PlayerHolder. Select the PlayerHolder and then on the "Inpector" tab there will be the "PlayerCharacter (Script)". In the "PlayerCharacter (Script)" scroll down to "Player Character Properties" header and under that header is the flag "Is Joy Pad". Set it to true. Making it true will allow to use mouse and keyboard or joypad to play the game in editor.

![Navigate to IsJoyPad](https://i.imgur.com/6s0Scwt.png)

*Figure2: Navigation to "Is Joy Pad" flag*

Hit the play button on the Unity.

![Hit Play](https://i.imgur.com/XcsUXse.png)
*Figure3: Hit the play button*

#### Controls
1. Mouse and Keyboard: To move around use W,A,S and D. To attack press left mouse button or left ctrl button in keyboard. To dash press right mouse button or right alt button in keyboard.

2. PS4 Controller: To move around use the left thumbstick. To attack press the "Square" button. To dodge press the "Circle" button.

3. XBOX Controller: To move around use the left thumbstick. To attack press the "X" button. To dodge press the "B" button.

## Versioning
The project uses [Semantic Versioning](https://semver.org/). Available versions can be seen in [tags on this repository](https://github.com/deadlykam/LastTry/releases).

## Authors
- Syed Shaiyan Kamran Waliullah \- [DeadlyKam](https://github.com/deadlykam)

## License
This project is licensed under the GNU General Public License v3.0 - see the [LICENSE.md](LICENSE) file for details
