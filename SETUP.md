# Introduction
If we're going to learn about being in the real world, let's put ourselves in the real world.
You have probably worked on team projects in the past.
You know how hard it is to keep everyone updated with the latest changes of a document (whether that is a presentation or a paper).
These changes are often nearly impossible to track, and nothing ever goes completely as planned.

Developers and Project Managers have this problem too, but often on a much grander scale.
Projects are often composed of hundreds (sometimes thousands) of seperate documents, each with several hundred to several thousand lines of content.
Historically, managing content on this scale is a daunting task, and people were hired specifically to keep track of everything.

This is the modern world.
We have technology.
It's fun stuff.
While all of the hassle of yesteryear is not completely gone, developers in particular have (not surprisingly) developed a way to automatically, consistently, and reliably manage changes in a project over its entire lifecycle.
In the real world, in today's world, we as developers (or perhaps more arrogantly, software engineers) use *source control management systems* (SCM, a.k.a *source configuration management*, *software configuration management*).

It is exactly what it sounds like.
In fact, there are several flavors of this SCM concept, including Subversion (SVN), Mercurial (hg), Concurrent Versions System (CVS), and you guessed it, *git*.
*Git* is a source control management system.
It is a tool to be used among collaborators of a project to synchronize their efforts and to provide a way to track changes (and influence) over time.
This is ***not*** a replacement for communication.
In fact, communication is now more important than ever.

Working on large projects means we have to know *ahead of time* what we are going to do.
How are we going to solve this problem?
How are individual contributions going to work with each other?
What are my responsibilities as a team member?
These are all questions that need answering before we commit even one line of functional code.
Documentation (external and internal) are paramount to our smooth success.
Trust me, you'll feel good about yourself when we get this going.
It's like growing up or some shit.
It's like being professional or some shit.
This is some crazy shit.

We don't need a book to tell us that we need to figure out what we are doing before we do it. With sufficient communication and planning, we be certain of our actions and consistently constructive.
There will be bugs; there will be miscommunication, but could anything be worse than emailing these things back and forth between four people? Let's make this work.

And yes, over-dramatization is one of my unsung talents. ;)

# Setup

## Cloning the Repository
1. Create an account on [Github](http://www.github.com) and [email me](mailto:seallred@smcm.edu) your username (so I can give you push permission)
2. Download a simple git client and set it up with your Github info
	* [Github for Windows](http://windows.github.com)
	* [Github for Mac](http://mac.github.com)
	* [TortoiseGit](http://code.google.com/p/tortoisegit/) is good too, but I haven't really used it
	* Even [Github for Eclipse](http://eclipse.github.com)
3. Open [this repository on Github](https://github.com/vermiculus/se-blackjack).
4. Select 'Clone in Windows' or 'Clone in Mac'.
	* The Github client should open up. It will download all source files into the root Github folder on your computer. This folder can be changed in settings, otherwise it just defaults to your documents folder.

## Contributing to the Repository
1. Make any changes you'd like in the repository folder and save them.
2. Open up your github client and commit your changes. Keep the commit message concise.
3. Committing your changes will update your own local copy of the repository, but will not update the server's copy.
Select 'Sync' to update the server. The next time anyone pulls from the repository, they will get your changes.

## Conflicts
I know that git itself can handle conflicting copies, but what I don't know is how it does so.
It'd be easiest for now to just commit any changes you made when you're ready to stop working on it, and to pull any new changes when you're ready to start again.