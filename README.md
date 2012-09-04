Software Engineering - Blackjack
================================

Just a git for the Blackjack Project
------------------------------------

------

## No Dice!
 * Josh Kaminsky
 * Molly Domino
 * Sean Allred
 * Mat Lee

**Typical Software Engineering Roles**

Adapted from a [Developer.com](http://www.developer.com/mgmt/article.php/3490871/Cracking-the-Code-Breaking-Down-the-Software-Development-Roles.htm) article by Robert Bogue.

 * Subject Matter Expert (SME)  
	The subject matter expert is the person(s) from which requirements are captured.
	These are the people who know what the software needs to do and how the process works.
	They are often the ones who will benefit from the use of the system.
 * Functional Analyst (FA)  
	Transforms the requirements of the SME into clear, concise, non-conflicting, and unambiguous requirements.
 * Solutions Architect (SA)  
	The Solutions Architect is responsible for creating the design documents from the requirements of the Functional Analysts.
	These design documents are used by the rest of the team (mostly Developers and Development Lead).
	The Solutions Architect is typically responsible for matching technologies to the problem being solved.
 * Development Lead (DL)  
	The Development Lead's role is focused around providing more detail to the Solution Architect's architecture (detailed program specifications creation).
	The Development Lead is also the first line of support for a Developer who needs help understanding a concept or working through a particularly thorny issue.
 * Developer (Dev)  
	The Developer (more often a team of them) writes the code according to the specification.
 * Quality Assurance (QA)  
	Using a variety of techniques ranging from keying in data and playing with the system to formalized, automated testing scripts, the Quality Assurance team is responsible for ensuring the quality of the solution and it's fit to the requirements gathered by the Functional Analyst.
 * Deployment (Deploy)  
	Deployment packages up all of the compiled code and configuration files and deploys it through the appropriate environments or on the appropriate systems (getting the solution *used*).
	To that end, the role may include automated software installation procedures or may be as simple as copying the files to the appropriate place and running them.
 * Training  
	The training role is responsible for documentation for the system as well as any instructor or computer based training solutions, which are designed to help the users better understand how the system works and what they can do with it.
 * Project Manager (PM)  
	The Project Manager is responsible for ensuring consistent reporting, risk mitigation, timeline, and cost control.
	They try to resolve problems while they are small so that they can be handled more quickly and with less cost.
 * Development Manager (DM)  
	The Development Manager is responsible for managing multiple priorities of conflicting projects.
	The Development Manager role is also an escalation for issues from the team, which it is unable to resolve internally.

As you surely know by now, we don't need that many roles.
This is a game of Blackjack for a class.
Irregardless, there are several roles that we can fill out to make this a successful venture:

 * Subject Matter Expert
 * Functional Analyst
 * Solutions Architect
 * Developer
 * Quality Assurance

For intangible reasons that seem innately obvious, we should all be taking on the roles of *Developer* and *Quality Assurance*.

Since we all know our skills and talents, let's fill in the roles we would be comfortable fulfilling. (Note that we will all be taking on multiple roles, and not all roles we say we are okay with doing will be our actual roles.)
I've checked off roles that I think fit the bill, but please, please, *please* correct me if you want to do something else or if you don't want to do something listed.

| Name  |  SME   |   FA   |   SA   |  DEV   |   QA   |  
|------:|:------:|:------:|:------:|:------:|:------:|  
| Josh  |&#x2713;|&#x2713;|&#x2713;|        |&#x2713;|  
| Molly |&#x2713;|&#x2713;|        |&#x2713;|&#x2713;|  
| Mat   |&#x2713;|&#x2713;|        |&#x2713;|&#x2713;|  
| Sean  |        |&#x2713;|&#x2713;|&#x2713;|&#x2713;|

## <a id="Questions"></a>Questions for Class

1. What if both the Player and Dealer bust on the same hand (or during a Hit)?
2. Does the Dealer win the round if the Dealer draws a hand totaling 21?
3. What if the Player does not have the minimum bet requirement?
	* Do they bet all their money?
	* Does the game end as if they had reached $0?

## Requirements Document

This game shall be a GUI-driven program to emulate the interactive play of Blackjack between the user ('the Player') and the computer ('the Dealer').

### Constraints

 * Must run successfully on the Windows 7 computers in SH-165
 * Played with one deck only
 * No reshuffling

### Example Execution

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
		

### Special Considerations

 * If upon dealing the Dealer's hand initially totals 21, then both cards are face-up. (See [question 2](#Questions))
 * If the deal does not bust on a given hand, then the higher total wins. The Dealer invariable wins ties.
	* If both the Player's and the Dealer's hands total 21, then this tie goes to the Player (at double the payoff)

### Interface Design

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
 * A good basis would be this picture: http://takegame.com/gamblings/pictures/blackjack.jpg
 * 	On the left are the users cards, on the right the dealer's face up card. With labels explaining things thusly 
 * 	Below the users cards are the "hit" "split" and "stand" buttons
 * 	Above in the banner are the user's funds and their current bet
 * 	Above that banner are the menu things:  Statistics, reset button, other things
 * 		The announcement of a win/loss/ "are you sure you want to restart" should be new windows that open up
 * 
