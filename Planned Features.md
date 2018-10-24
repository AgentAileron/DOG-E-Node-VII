# Attributes
	> sudo auth mode (allow server admins, require passkey pasting)
	> lockdown mode (modifyable remotely, only by MolarFox)
	> Message timeouts
	> Bot usage stat tracking
	> Permissions checking
	> System resource limiting (?)
	> Per user stat tracking
	> Per server permissino tracking
		- EG: allowed to play music in voicechat
	> SFW Level (for searches, etc.)

# Planned Commands
	**Regular:**
		> Basic
			- about (Include own contact info)
			- Ascii art fox
			- Contact for suggestions
			- Contact for bugs
			- Credit to D#+
		
		> Math
			- Basic expression evaluation (+ toggles for wolfram lang, latex, etc.)
			- Taylor Series Expansion (+ number of expansion to perform)
			
		> Search
			- General Wolfram Alpha query ability
			- Stack overflow
			- Google search (I'm feeling lucky + top 3 results)
			- Wikipedia
			- Wikihow
			- Furaffinity
			- Deviantart
			- Dictionary
			- Thesaurus
			- Urban Dictionary
			- Youtube
			- Soundcloud
			- Bandcamp
			- DuckDuckGo (I'm feeling lucky + top 3 results)
			- SCP wiki
			- Bugmenot
			- Wayback Machine
			- Know your meme
			- Reddit
			- Twitter
		
		> Cyberchef
			- See about direct calls to the JS interface
		
		> PRNG
			- OTP (between two users +factor user stats, isbot etc)
			- Debate (between two users)
			- Random XKCD
			- Random number (+ allow range definitions)
		
		> Music
			- Youtube, Soundcloud, Spotify, bandcamp support
			- Queue management
			- Play, pause, dump queue
			- SUDO: writelocks
			- Voice channel selection (based on calling user / specified seperately)
		
		> Novelty
			- User message stats
			- Rock paper scissors
			- Rock paper scissors lizard spock
			- Chess game and tracking
			- Unicode toys text converter
			- Greeter (respond to hello, etc.)
			- Ascii art copypasta
			
		> Evie / Cleverbot redirector
			- Redirect to cleverbot / evie, with a session per chatroom
			
		> Voting system
			- Make poll
			- Add vote
			- modify vote
			- Poll maker defines params (time limit, end poll, verbosity on vote additions, etc.)
			- Output stats on poll, server participation etc. on vote call
		
		> Per chatroom secretary
			- Note taking (user can delete own notes only, unless sudo)
			- Reminder setting (+ pings)
			- Get time in other timezone
			
		> Global chatfile
			- static across all servers and rooms
			- Stores last x messages (EG: last 20)
			- All users can input and view
		
		> Image Processing
			- make ascii art from image
			- ML Recognition of pictures, python lib call
			
		> Anonymous whispers
			- Only allowed when users share a server, and bot can DM target
			- Presents only message text without user sending's details
		
	**Elevated:**
		> Server Admin
			- Kick user
			- Make txt log of chat (in specified daterange, + limit daterange)
			- movein / moveout user (chatrooms, + check permission of user)
		
		> Linux like modifications to server
	
	**Bot Owner Only:**
		> Shutdown bot
		> Toggle lockdown mode
		> Indirectly message a server
		> 
		
# Misc Features
	> Ignore other bots