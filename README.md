# Automnal-SDP2
A Collection of Work by Alyssa Diaz for Meggies Studios' SDP2 Submission _Automnal_

My work on _Automnal_ mainly consisted of two areas: the implementation and use of Yarn Spinner itegration in our Unity project and setting up the scenes so that they transition into each other the proper way and load in the correct quest lines. Yarn Spinner is a Unity plugin that lets you easily set up dialogue converstations with NPCs. 

## Scene Transitions 
The creation of the scene transition system itself was done by team member Weston. My job was just to drag scriptable objects into the correct places within the Unity Editor. This may not seem like a lot, but I had to enter each scene and set which scene will load upon using each possible exit available. For example, the Mall Entrance scenes each have two possible exits: MallLeftExit and MallEntranceExit.

![image](https://user-images.githubusercontent.com/55610318/167977757-fb43b81e-8be9-49b8-8502-55dcbbba1edd.png)

The exits are signified by locations with pop-ups that indicate what type of scene will load and an up arrow (in the image, the locations are "To Street" and "Enter"). Each of these pop-ups are a prefab that can be edited to say the proper words and have the correct scene load cases. These are the load cases for MallLeftExit, and generally what each exit looks like in the Unity Inspector. I've highlighted the parts that change between each exit and are used to load a certain scene: 

![image](https://user-images.githubusercontent.com/55610318/167978759-21c6b547-be22-49d9-a0b3-aad527a62cc3.png)

Whatever is in Default Scene Case SO is the default scene that gets loaded when a player exits through that pop-up. Before I get more into this, I am first going to talk about the scriptable objects that go inside of these options. If the pop-ups we're editing here are called exits, then the scriptable objects inside of them are called entrances. The entrances are basically where the player spawns in upon loading into a new scene. It's one thing to make sure the proper scene is loaded in, but you also have to make sure the player is entering that scene from the correct entrance. MallLeftExit exits to the street scene, but we want our player to spawn on the right side of street since that's where they're coming from, so we use DefaultStreetRight (as seen in our Default Scene Case SO). 

Each entrance scriptable object can have a set of "decisions" inside of them. Decisions are basically booleans. These decisions are set to true or false within the Yarn scripts (which we'll get to later). These decisions tell the exits which entrances should be used as the player progresses though the game. The combination of these decisions inside the entrances are called "cases". Entrances in Default Scene Case SO will always be the default entrance regardless if it has a case or not. 

![image](https://user-images.githubusercontent.com/55610318/168011313-2d90806b-6cd2-4908-90fb-f5e4e196e1c5.png)

This is what the entrance scriptable objects look like (in particular, DefaultStreetRight). In the true list, you can see a decision placed. If that decision is set to true, the case for this entrance is complete. However, as noted before, DefaultStreetRight is in Default Scene Case SO, so that case is ignored. Let's take a look MallEntranceExit. 

![image](https://user-images.githubusercontent.com/55610318/168012205-dfb280be-1aec-4ef6-8529-e1f63870c098.png)

Notice how this exit has a scene case in addition to Default Scene Case SO. Let's take a look at the DefaultFountainCenter entrance.

![image](https://user-images.githubusercontent.com/55610318/168012763-17c5d0be-8a0b-4fc6-88db-eb82733d6a41.png)

This entrance's case is defined by one decision, FriendQ1. This decision gets triggered upon accepting the first friend quest. Once this decision is set to true, the case is activated, and MallEntranceExit now exits to DefaultFountainCenter instead of FountainCenter1 in the Default Scene Case SO. An entrance with an activated case will always override the Deafault Scene Case SO. An exit can have multiple scene cases as well. 

As a reminder, Weston created all of these systems, so props to him! I just utilized them to make sure scenes flowed into each other correctly. Here's an example of how MallEntranceExit chooses an entrance before and after a true case is set.

![documentation1](https://user-images.githubusercontent.com/55610318/168025973-422d6fd6-83aa-4032-9e54-3ef31088a94a.gif)

## Yarn Spinner
Thankfully, Yarn Spinner comes being able to work pretty much out of the box, so I don't really have to do any additional set up mind a few objects I have to create in each scene. Let's start with the dialogue script (named "GameDialogue" in the files). This script is pretty straight forward, yet is the main script that carries the gameplay. Yarn uses its own language and I actually had to download Visual Studio Code so I could use the Yarn Extenstion that highlights the syntax. This is the basic formula for each NPC's dialogue:

![image](https://user-images.githubusercontent.com/55610318/168027600-09c9bc5a-70df-43f2-98bb-5eb43b381ad6.png)

Every interactable NPC has a normal Unity script called YarnInteractable on it (included in files). This script is a default Yarn Spinner script included with the package that I added onto. The YarnInteractable script has a string variable that tells it which converstation to start upon the player interacting with a NPC. The string is the title of the converstation within the GameDialogue script. So, to start the first bully quest converstation, the string should be BullyQuest1 as it is in the screenshot above. Each line represents a new line of dialogue the player has to click through. Names of who's speaking can be set if needed and are in orange. Booleans can be created, such as the very first line where choosing is set to false. This lets the player be able to end the dialogue, explore other quest options, and come back and start the dialogue again. It isn't until the player chooses to accpect or deny a quest that the choosing boolean gets set to true. At the end of the dialogue, there is an if statement that disables the players ability to talk to an NPC if choosing is true. This keeps the player from starting and accepting/denying the quest again. Indentations with -> signify a part in the dialogue where the player can choose between options. Lines of dialogue under these indentations (while also indented) are spoken only after that options is choosen. Finally, throughout the script are lines that look like <<Bully1 jessica>>. This is how functions within the YarnInteractable script are called:

![image](https://user-images.githubusercontent.com/55610318/168037600-dee13314-13ff-4ea9-aae3-8ae3fd6a87c4.png)

[YarnCommand]s are commands that I can trigger from within the Yarn Dialogue script upon being triggered, the funciton below it will activate. This [YarnCommand] is named "Bully1" so that is the name used to trigger it in <<Bully1 jessica>>. "jessica" is the name of the game object that the YarnInteractable script is on. Within the function in this image, you can see me setting the BullyQ1 decision to true. So, <<Bully1 jessica>> is called at the end of the dialogue upon selecting to start the quest. This triggers the function in the YarnInteractable script, setting the BullyQ1 decision to true. The activation of this decison changes which entrances are called from certain exits as scene cases become true. 
