Software Engineering - Blackjack
================================

If we're going to learn about being in the real world, let's put ourselves in the real world.

------

Just a git for the Blackjack project.

# Setup
(actual project stuff down at the bottom)

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

# Blackjack

## No Dice!
 * Josh Kaminsky
 * Molly Domino
 * Sean Allred
 * Mat Lee

## <a id="Questions"></a>Questions for Class

1. What if both the Player and Dealer bust on the same hand (or during a Hit)?
2. Does the Dealer win the round if the Dealer draws a hand totaling 21?
3. What if the Player does not have the minimum bet requirement?
	* Do they bet all their money?
	* Does the game end as if they had reached $0?

## Requirements Document

This game shall be a GUI-driven program to emulate the interactive play of Blackjack between the user ('the Player') and the computer ('the Dealer').

###Constraints

 * Must run successfully on the Windows 7 computers in SH-165
 * Played with one deck only
 * No reshuffling

###Example Execution

1. The Dealer will ask for the Player's first name upon startup.
2. The Player is given $500 of initial in-game betting money ('Cash') to start with.
3. Four cards from the deck are randomly distributed to both the Player and the Dealer in alternating order.
At this point, both the Player and the Dealer have two cards each.
The faces of both cards of the Player are visible, while only one card of the Dealer is visible to the Player.
6. The user places their bet (constrained by a minimum bet of $20 and a maximum of their current available Cash).
7. The Player chooses one of three actions for the current hand:
	* Hit
	* Stand
	* Split
8. The Player's bet is then deducted from their Cash and placed in the betting box, displayed on-screen.
9. Depending on the Player's previous action, one of three things will happen:
	* Hit  
		The Dealer deals both the Player and himself a single card from the deck.
		* If the Player's hand then totals over 21, he 'busts'. A message appears on the screen with the caption "BUST!".
		* If the Player's hand totals exactly 21, the Player wins the round.
		* If the Dealer busts (with a hand totaling greater than 21), the Player is decalred the winner.

		In each case, the value of the betting box is reset to zero.
	* Stand  
		The Dealer deals himself cards one-by-one until his hand totals 17 or greater.
		If the Dealer busts, the Player is determined the winner of the round.
	* Split  
		*This action should only be available if the Player has exactly two cards of equal rank.*
		The Player's hand is split into two hands, each with their own totals.
		The Player effectively becomes two Players, whose 'Hit' and 'Stand' actions will affect both hands.
0. A dialogue window appears asking the Player if he wants to 'Play Again?' with the options 'Yes' and 'No.'
	* Yes: the statistics are recorded and a new hand is dealt.
	* No: the program exits.

The game ends when the Player runs out of Cash.
		

###Special Considerations

 * If upon dealing the Dealer's hand initially totals 21, then both cards are face-up. (See [question 2](#Questions))
 * If the deal does not bust on a given hand, then the higher total wins. The Dealer invariable wins ties.
	* If both the Player's and the Dealer's hands total 21, then this tie goes to the Player (at double the payoff)

###Interface Design

 * The game is played on a felt-green background.
 * There is betting box in the upper-right-hand corner of the screen.
 * At all times, the name of the Player and the Player's Cash are displayed at the top of the screen.
 * The 'About...' menu item brings up a window denoting the version and team name.
 * The 'Statistics...' menu item brings up a window with the following information:
	* Number of hands won
	* Number of hands lost
	* Total number of hands played
	* Largest payoff
	* Largest loss
 * The 'Restart...' menu item, after confirmation, resets the statistics of the game and starts a new hand.