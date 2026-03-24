# Bimp.ly - Light in the Dark 
This project is a small 2D-platformer Unity prototype where the player controls a solardriven robot that seeks a way out of a dark cave. The objective is to navigate your way out by using the flashlight and the light projectiles of the robot, while managing it's battery state.

## Features
- Player Movement System
- Battery System
- Enemy Behaviour System
- Elevator System

- Grid-Based Movement System
- Lightning System

## Git Strategy 

- **Branching Model**
  - Master Branch: Holds the stable, production-ready version of the project.
  - Feature Branches: Used for developing new features, bug fixes, or experiments. Each developer works in their own branch.
  - Pull Requests: Changes are merged into the main branch via pull requests, ideally after code review and testing.

- **Commit Best Practices**
  - Commit early and often: Frequent commits help track progress and make it easier to revert changes if needed.
  - Clear commit messages: Use descriptive messages like "Add biofeedback interface" or "Fix prefab loading issue".

- **Process**
  1. Create a new branch off master in which to do feature or bug work.
  2. Commit often to this branch and push those changes to back them up.
  3. When ready, try to merge the feature branch to the main branch.
  4. When merge errors occure, contact me for help or create a pull request. Don't force push etc.!
  5. When no errors occure and everything in Unity works as expected, commit and push the merge.
 
## Note
-It was a gamejam project with very limited time. I therefore didn't comment my code as good as I would usually do.
