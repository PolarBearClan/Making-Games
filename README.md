# Making-Games-Project
This is a project for the course 'Making Games (Autumn 2024)' at the IT-University of Copenhagen.

The game will be accsesable on Itch.io and steam in the foreseerable future...

# Running the Game

To play the game on you local PC, you need a few things:
- A computer with Windows x64
- Download the game from the most recent action in the branch: *main*
- Once the above steps are done, you can enjoy the game!


# Group Members

Producer: Kristóf Lénárd - klen@itu.dk  \
Design Lead: Simas Alaunė - simal@itu.dk \
UX Lead: Asger Holmehøj Bach - asgba@itu.dk \
Tech Lead: Nicholas Hansen - nicha@itu.dk \
Programmer: Petr Simek - peci@itu.dk \
Programmer: Eik Boelt Bagge-Petersen - eikb@itu.dk

# Issue formatting
All issues should follow this format:

**Overview** \
The overview should summarize the objectives of the task.

**User story** \
The user story should provide human centric context to the task.

**Acceptance critera** \
The acceptance criteria should be a list of subtasks that collectively complete the task.

**Full task description** \
The full task description should contain any revelant infomation in any form e.g. diagrams and pictures.

# Guideline
- Commits should be short and concise like e.g *"Add fix for dark mode toggle state"* to show intent of commit code.

- Pull requests into *main* branch should always contain the *Major* tag into the title for version control reasons. For *dev* branch it applies with the tags *Minor* and *Patch*. 

- Task estimation every monday after class to estimate size of features/tasks from backlog.

- *Main* is for major changes and build the game for release, dev is for minor and patches and branches are for issues.

- Pull requests are required to be reviewed and approved by another developer, for pull requests into *main* the Tech Lead should be involved in the process. 

- Releases are of the format v<x.y.z> where:
  - x: MAJOR version when you make incompatible API changes
  - y: MINOR version when you add functionality in a backward compatible manner
  - z: PATCH version when you make backward compatible bug fixes

- Scenes: create a scene for your own issue if neccesary. There should be a main scene for the acutal gameplay/level designer.

- Scenes should be created as a collection for a specific feature.

- Prefabs are awesome! Should be a folder for its own.
