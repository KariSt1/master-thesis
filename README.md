# The Perception of Weight in Virtual Reality When Combining Velocity Limiting and Control/Display Ratio Modification

# Abstract

This study explored the perception of weight in virtual reality using the combination of limiting the velocity
with which an object can be lifted and control/display ratio modification and what affordances and challenges the combination might bring. Both interaction methods have thus far been explored independently, but not together. A virtual reality environment was designed that allowed each interaction method to be tested independently and together. A between-subjects user study was conducted where participants did multiple weight discrimination tasks along with a task involving absolute weight estimation. Results indicated that the combination of the two interaction methods increased the accuracy of weight discrimination considerably while not allowing for absolute weight estimation. It also increased the likelihood of noticing the interaction method with some participants that tested only one interaction method struggling to distinguish between the weights. Future work is required to reduce the frustrations that dropping objects due to the velocity limit caused among participants.

## Interaction methods

Two interaction methods were tested in this project, the first is C/D ratio modification which involves creating an offset between your real and virtual hand. So depending on the weight of the object you are holding the C/D ratio is different. The second interaction method is called velocity limiting which involves limiting the velocity with which you can lift objects. Each object has a specific velocity limit and if you lift the object avove that limit you drop the object. 

## Scenes

The scenes can be found in the Scenes folder. Three scenes were created where the CD scene tests the C/D ratio modification interaction method, VL tests the velocity limiting interaction method, and Both_combined tests the combination of both interaction methods.

## Scripts

In the Scripts folder there are two main files that implemented the interaction methods. these were the HandMovement script which implemented the C/D ratio modification, and the DropOnFastLift script which implemented the velocity limiting. The other scripts were either for data collection during the study or various things like respawning the objects if they fell to the ground or move on to the next part of the study.

## Requirements

The project was created using Unity version 2021.3.18f1 and Virtual Reality Toolkit (VRTK) 4.0. During the user study the project was built onto the Meta Quest 2 headset and works with at least software update 54.0.
