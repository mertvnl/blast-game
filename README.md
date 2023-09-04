# Blast Game

## Getting Started
In this project I have used my own mobile game base project that I created. I usually use this project to help me with the study cases by including reused systems.
I decided to use seeds for level generation in order to have same level generation and randomness for every device. This also prevents generating totally random items for X level, so if player fails or restarts the level, they will play the same generation again.


https://github.com/mertvnl/blast-game/assets/41839620/e51818dc-d223-46c0-b93f-7af1f2892598



### Assets used
1) DoTween, to make simple animations.
2) Odin Inspector, to stay organized with the Unity editor.
3) SerializableDictionary, to serialize and modify dictionary on Unity editor.

## Gameplay 

### Rules
1) Player needs to tap items that is grouped as two or more in order to blast them.
2) Player earns score from every item that is blasted. If player reaches target score before running out of moves, wins the level. Otherwise player fails.

## Level Settings and Level Generation

<img width="403" alt="image" src="https://github.com/mertvnl/blast-game/assets/41839620/8af384b6-156c-47d0-90b8-dfe146b935c9">

### Grid Data
This data contains grid settings such as width and height.

<b>Grid Sprite</b> is the background of the grid. We can change the background for any level we want, so we can have a variaty. Any 9 sliced sprite is works.

### Item Settings
<b>Allow Obstacles</b> is determines if there will be any obstacle on that level.

<b>Total Item Color Count</b> is determines how many different colored object will the level include.

<b>Obstacle Creation Count</b> is value to spawn obstacles by chosen amount.

<b>Blastable Group Thresholds</b> are the threshold to change item icons depending on their grouping.

<b>Blastable and Obstacle Item Database</b> item database for that level. Created items are chosen by this database.

<img width="385" alt="image" src="https://github.com/mertvnl/blast-game/assets/41839620/d687e308-4b0a-4953-a7ae-007880903162">

### Level Settings
<b>Required Score</b> is the goal for player to achieve in order to complete that level.

<b>Move Count</b> is given attempts to player to reach the required score.

<b>Level Seed</b> is the seed for that level in order to have same randomness for every device.
