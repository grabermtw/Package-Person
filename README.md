# Package Person
## Modernizing Arcades through Messaging

### Inspiration
Once upon a time, arcades were adored by the young people of the world. They were places for gaming, sure, but also places to hang out with friends and make new friends. Sadly, that time has long passed. With the advent of home video game consoles, the popularity of arcades evaporated, and most of them went out of business.

I once visited an arcade many years ago. I had a good time there, and even managed to make it onto the leaderboard for _Galaga_. However, after I left, I never ended up going back. I don't even know if I'm still on that _Galaga_ leaderboard, it just hadn't really been on my mind since then. The arcade in the 21st century feels like a novelty experience you'd partake in once just to say you've done it. There's really nothing to remind you of it once you leave.

For this project, I wanted to explore ways that arcades could be updated to better fit today's trends in gaming and socializing. Specifically, creating an incentive to keep coming back to an arcade by extending the reach of the games beyond the confines of the arcade itself. By enabling arcade games to notify their players about updates to their leaderboard status, it incentivizes players to return to the arcade to defend their honor.
### What it does
_Package Person_ is a proof-of-concept PC game with an arcade-style leaderboard that keeps players updated on changes to their placement on the leaderboard. In the game, you (the player) are a courier (or a "Package Person") with the job of delivering packages that you find on the ground to the houses in a neighborhood. The goal is to deliver as many packages as possible while avoiding the dogs that roam the neighborhood (we all know how much dogs hate the mailman). The game also features a title screen, from which (in addition to starting the game) the player can access the "How to Play" information, view the current leaderboard, or change the game's settings (such as the name of the "arcade," graphics quality, etc.).

The game ends when a dog touches the player. After this happens, the leaderboard is displayed. If you scored high enough to make it onto the leaderboard, you will be presented with input fields to enter your name, email, phone number, and a custom message that will be sent to all players on the leaderboard who scored lower than you. The email and phone number are completely optional, but if you choose to enter either or both of those, then you will receive an email and/or text message congratulating you on making it onto the leaderboard. At the same time, all players who scored lower than you who provided their email and/or phone number will receive messages informing them that you knocked them down in the rankings. If you specified a custom message, that will be included in the email or text they receive as well.
### How we built it
The game itself was created using the Unity engine with all the logic and mechanics implemented using the C# programming language. [Courier](https://www.courier.com/) was used to manage sending all the emails and text messages. A notification was created in Courier's online notification creation/management portal specifically for _Package Person_ and was configured to send emails via Mailjet and text messages through Twilio.
Sections of text in the Courier notification were given conditions to make them visible or hidden depending on the information that the game sent to Courier. The conditions allow there to to be three main types of notifications that are sent out:
1. Notifying that you have just made it onto the leaderboard
2. Notifying that someone has beaten your score and you've dropped to a lower ranking on the leaderboard
3. Notifying that someone has beaten your score and you've been knocked off the leaderboard entirely (like many classic arcade cabinets, _Package Person_ only stores the top ten scorers on the leaderboard)

These messages also will inform the recipient of the name of the "arcade" that the leaderboard in question is located at (the name can be specified in _Package Person_'s Settings menu).
The necessary data is sent to Courier via a POST API request in C#.
### Challenges we ran into
While Courier has SDKs for languages such as Python, Java, Ruby, Go, etc., and can generate example code for sending notifications from these languages, unfortunately C# is not included. Instead, I had to manually construct a POST request through Unity's C# web request class that fit the format desired by the Courier API.
### Accomplishments that we're proud of
_Package Person_ has reached a completely playable state and is fully capable of keeping track of high scores as well as automatically sending updates to previous players on the leaderboard as more players play the game.
### What we learned
I learned how to use Courier to create, dynamically edit, and send notifications, as well as how to use Unity and C# to automatically create meaningful POST requests to an API server. 
### What's next for Package Person: Modernizing Arcades through Messaging
In the future, I hope to explore adding support for more messaging services via Courier into the game. For example, Discord is a very popular service used largely by gamers, so finding ways to use Courier to allow for sending _Package Person_ leaderboard updates via Discord would be a high-priority objective.

Some other ways to expand _Package Person_ that time has not allowed for during this hackathon include using a database to create a global leaderboard capable of being viewed from any installation of _Package Person_ on any device, improving the balance of play within the game itself, adding more mechanics to the game (for example, the ability to distract dogs by throwing bones), as well as vastly improving the aesthetics of the game's environment and UI.
