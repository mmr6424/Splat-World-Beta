![Spray can surrounded by splashes of paint](/SplatWorld/Assets/Images/Icons/icon.png)
# Splat World (Beta 1.7.0)
__Repository for storing the Unity files relating to the Beta design of Splat World, the hit new AR graffiti game!__
> [!IMPORTANT]
> This README.md file is being updated currently and is unfinished. Advice may be incomplete or out of date.
## How It Works! (Right Now)
> [!NOTE]
> #### I see a script called DDOL. What is DDOL?
> DDOL is shorthand for "Don't Destroy On Load", and its script is used to ensure that any object it is attached to is maintained between Unity scenes.
### AR Line Renderer
* ARLineRenderer script creates a new LineRenderer object and assigns it a position within AR Space each time the user *begins* touching the screen. Should the user move their touch point, ARLineRenderer will *update* the LineRenderer object with its new point, at which the LineRenderer will draw a line between the two points using the color, width, and material selected.
* The ARLineRenderer script uses Lightship's example HitTest script as a basis, expanding upon it to create new LineRenderers at the chosen position and updating them, rather than simply placing a 3D object at the location selected.
* ARLineRender serializable variables are as follows: Camera, HitTestType (which determines the types of surfaces it allows users to select), Default Color (which determines the color of the line), Corner Vertice and End Cap Vertices (which determine the quality of the line), StartLineWidth and EndLineWidth (which determine the respective widths of the line), MBrush, MCan, and MChalk (which determine the different material types), and IfSimplify (which will attempt to simplify the line into 2 points).
* ARLineRenderer non-serializable variables are as follows: prevLR (the previous line), lineRender (the current LineRenderer object), lines (a list of all lines), posCount (the number of positions the current line has), distanceToPoint (the distance between the previous point and the current point), and currentTool (enum that selects the material for the tool type).
### Tool Toggle
> [!WARNING]
> This section is a work in progress. Information contained within may or may not be accurate.
* ToolToggle script maintains the status of the current tool selected in the U.I., hiding and unhiding the current active tool from the menu.
## What is Lightship AR? How do I work it?
> [!NOTE]
> Developers who wish to become even more intimately familiar with this project can read the Lightship ARDK 2.5 documentation [here](https://lightship.dev/docs/archive/ardk/).

Lightship AR is an AR Developer Kit created for Unity with the express purpose of precisely overlaying AR content on the real world. We are using Lightship's hit detection format and plane detection to create 2D planes on real-world surfaces which the user can then draw on using our various tools. Specifically, we are uing AR Sessions, touchCounts, and the currentFrame.HitTest function to grab the users touched position. Lightship works through the AR Session object/script, which utilizes a series of helper scripts such as the Plane Manager, Capability Checker, Rendering Manager, and Depth Manager.
## Mapbox and How We are Using It
## Setting Up Your Environment (For New Developers)
New Developers will need [Unity version 2020.3.34f1](https://unity.com/releases/editor/whats-new/2020.3.34) with a seperately downloaded [Gradle version 7.5.1](https://gradle.org/releases/). This section is purely meant to walk new developers through setting up their unity environment for development of this project.
> [!WARNING]
> If gradle is not set correctly, project will __not__ build on your computer, regardless of other settings!

## App UI
### Home Screen Scrolling
* Panels exist in a "grid" where each space is the size of the screen. A swipe between pages moves the entire grid across the camera space so that a new panel becomes visible.
### Tag Gallery
* Dynamically creates tag thumbnails in a flowing grid (similar to something like an instagram profile). Tag information is loaded from a filler dataset right now. Awaiting server implementation

## Current Known Issues
* Stuttering during gameplay, causing art to glitch in and out of view
* Creation of new LineRenderer object upon each change to a line and each individual touch of the screen, resulting in empty objects
* No connection between ARCanvas object and ARLineRenderer object
