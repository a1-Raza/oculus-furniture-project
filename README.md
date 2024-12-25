# oculus-furniture-project

## Welcome to the VR Furniture Shopping Experience!
It's always been hard to shop for furniture since you can't simply "try on" furniture, but with this, that's no problem! This project explores how the furniture shopping experience can be enhanced using VR.

This is how the experience goes: You'll load into a template room, with furniture around you. You'll be able to check these models out up close, switch between different furniture pieces, and customize to your liking from a provided selection. More details in the section below.

Note: This project is in Unity and currently designed using the Meta SDK, so for now it'll only work on the Oculus/Meta Quest headsets.

### Features:
I've been developing this project in stages, with each "complete" proof-of-concept (POC) adding something new. Note: The server (link provided in About section) must be running to use some of the features of the app.
These are the features available in the latest "complete" POC:
- Templates - a room will be loaded, with furniture pieces arranged around you; requires server to load specific templates
- Viewing furniture - you'll be able to move around the room, allowing you to inspect individual furniture pieces or the entire set
- Switching pieces - you can replace each furniture piece with a another one
- Model downloading - the models of furniture pieces can be downloaded and loaded in front of you on the spot; requires server to run

### Building and Running + Bugs
This project can be built the usual way Meta/Oculus Quest games are built, but note that the performance won't be great (although that could be because I'm testing on a heavily used Quest 2). You may experience stutters when trying to view a model you don't have as the headset downloads it, or due to the graphics (I experimented with at least semi-realistic visuals in POC3). I hope to fix those at some point!

### Hiatus over soon
This project has been on hiatus for a while, but I'll be picking it up again soon now that I'm on break!
