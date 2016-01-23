# Genetic Startups
> C# .NET implementation (with Windows Forms) of a Desktop application, based on Genetic Algorithms, representing possible lives of startups. The algorithm improves startup choices over generations, to achieve the most successful outcome possible; in a map where investors, product launches, team members, sad news and sales, among other options, appear.

## Introduction: Genetic Algorithms

In the field of artificial intelligence, a genetic algorithm (GA) is a search heuristic that mimics the process of natural selection. This heuristic is routinely used to generate useful solutions to optimization and search problems.

Genetic algorithms belong to the larger class of evolutionary algorithms (EA), which generate solutions to optimization problems using techniques inspired by natural evolution, such as inheritance, mutation, selection, and crossover.

## The problem: Startup life evolution

Startups are surrounded with huge uncertainty and have very limited resources and time. Besides, the life of a startup is full of obstacles and tough choices. As founders, we must be very careful when choosing one path or another.

In this application, we generate random "maps" that represent the life of a startup (by showing some choices that startup founders usually encounter).
Since time and resources are limited, finding the optimal path is key to success. Pursuing that goal, we have developed a genetic algorithm that tries to pick the best possible outcome for the startup, learning with each new generation.

### Population: chromosomes (start cell & movements)

In this application, we are representing every startup as a binary array. This means that each element of the population would be defined as a set of chromosomes (which can be each a "0" or a "1"). 

As an example, given a map of 3 columns and 3 rows in which we allow each startup to take two steps, one possible element of the population could be: "010001".

In the example above, since we have 3 rows, there are 3 possible cells for the start position. For that reason, the first two chromosomes represent the start cell. The rest of the chromosomes, in pairs, represent the movements. Both "00" and "10" would be "move right", "01" would be "move down" and "11" would be "move up".

### Operators: selection, crossover and mutation

For each generation, we will display the best candidate within the population. The score is calculated based on the values of the types of squares as described in that section of the menu.
Every new generation is calculated by performing 3 operations:
- Selection: The algorithm picks the top 1/3 candidates and pass them directly to the next generation.
- Crossover: The algorithm randomly picks pairs of elements, representing 1/3 of the population, divides them by half and creates two new elements by crossing.
- Mutation: The remaining 1/3 of new elements will be generated by picking random elements of the previous generation and changing a random chromosome to each one.

### Implementations
There will be implementations of this application using 3 different combinations of technologies / framewoks:
- *Native Windows app:* C# + .Net + Windows Forms ([repository](https://github.com/romenrg/genetic-startups-desktop-csharp-dotnet) | [installer](https://github.com/romenrg/genetic-startups-desktop-csharp-dotnet/releases))
- *NodeJS + REST API + ReactJS*
- *Java / Spring + REST API + AngularJS*

### Copyright and License

Copyright 2015 Romen Rodríguez-Gil

Licensed under The MIT License (MIT), as described in the file LICENSE.md
