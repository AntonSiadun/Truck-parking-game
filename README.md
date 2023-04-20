# Truck parking game

![Page](TitlePage.png?raw=true "Title")

# <a name="Description"></a>Description

Parking trucks is the prototype of a two-dimensional game simulator for trailer truck parking. This is one prototype project that I want to replicate while making it more user-friendly. The game, which was the basis( title hidden), had less attractive and responsive control. Here I tried to make as lively and involved in the game system truck control.

# <a name="Main goals"></a>Main goals

When developing the prototype, I followed the following objectives:
- easy-to-control immersive gameplay;
- minimum code base;
- use of Addressables system;
- provide an acceptable level of physical simulation;
- implement dependencies with zenject for easy maintenance.

# <a name="Pleasant things"></a>Pleasant things

There are a number of points in the project that can be used in other projects:
- method of finding one collider in another;
- new MyButton class with additional events;
- full wheel;
- the method of identifying touches in a certain part of the screen (it is planned to separate a class TouchFilter);
- system of management of the same level through the label using addressables (possibility to implement the simplest content delivery strategy without rewriting scripts).
