/* COMMAND MODULE FOR DOG-E Node VII
 * AgentAileron 2018
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using Google.Apis.Customsearch.v1;


namespace CommandModules{
    [RequirePermissions(Permissions.ReadMessageHistory)]
    public class CommandListTopLevel{

        
        // Returns hello to the calling user (TODO: beef it up a tad)
        [Command("hello"), Description("**Returns a hello to the calling user**"), Aliases("hey", "hi", "g'day")]
        public async Task HelloAsync(CommandContext ctx){
            await ctx.RespondAsync($"G'day {ctx.User.Mention}!");
        }


        // Greets a specified user
        [Command("greet"), Description("**Says hi to specified user**"), Aliases("sayhi", "say_hi")]
        public async Task GreetAsync(CommandContext ctx, [Description("The user to say hi to.")] DiscordMember member){
            // Trigger typing indicator for bot
            await ctx.TriggerTypingAsync();
            
            // Get emoji
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            // Print out - NB: '{}' are escaped, insides are parsed as strings and concat'd
            await ctx.RespondAsync($"{emoji} Hello there, {member.Mention}!");
        }


        // Responds with a nicely formatted description of uptime elapsed
        [Command("uptime"), Description("**Returns how long the bot has been online for / time since last reboot**")]
        public async Task UpTimeAsync(CommandContext ctx){

            await ctx.TriggerTypingAsync();     // Trigger typing indicator for bot

            TimeSpan time_elapsed = DateTime.Now - DogeNode7.BotStats.starttime;       // Gets actual time since bot launch
            //TimeSpan time_elapsed = new TimeSpan(0, 0, 13, 0, 0);            // Sets debug timespan

            string timeOut = Reg.StatMethod.FormatTime(time_elapsed);

            await ctx.RespondAsync($"I have been running for `{timeOut}`");     // Output the constructed response message

        }


        // Returns a list of online users (IN SERIOUS NEED OF REWRITE)
        [Command("online_users"), Description("**Returns list of online users (useful for Discord over irc) [UNSTABLE]**"), Aliases("w")]
        public async Task OnlineUsersAsync(CommandContext ctx){
            await ctx.TriggerTypingAsync();
            
            // Initialise expected arguments to pass
            Dictionary<string,bool> expectedArgs = new Dictionary<string, bool>();
                expectedArgs["A"] = false;  // Show All user statuses
                expectedArgs["o"] = false;  // Show online  (def)
                expectedArgs["d"] = false;  // Show do not Disturb
                expectedArgs["i"] = false;  // Show idle
                expectedArgs["O"] = false;  // Show Offline
                expectedArgs["b"] = false;  // Show bots
                expectedArgs["u"] = false;  // Show users   (def)

                expectedArgs["n"] = true;   // Number of people to show (def 5, max 40)
                expectedArgs["p"] = true;   // page number to show (where multiple pages exist)

            // Get list of users in server
            var memberList = ctx.Guild.GetAllMembersAsync().Result.ToArray();

            // Interpret args, set defaults if none given
            Dictionary<string,string> args = Reg.Util.GetArgs(ctx.Message.ToString(), expectedArgs);

            int dispCount = 5;
            int pageNum = 1;
            if (args.Count == 0){   // No args passed
                args["o"] = null;
                args["u"] = null;
            }else{
                if (args.ContainsKey("n")){ // dispcount defined
                    Int32.TryParse(args["n"], out dispCount);
                    if (dispCount > 40 || dispCount < 0){dispCount = 40;}    // Range check
                }
                if (args.ContainsKey("p")){ // pagenum defined
                    Int32.TryParse(args["p"], out pageNum);
                    if (pageNum < 0){pageNum = 1;}      // Range check
                }
            }

            // Output users if netiher bots nor users are specified
            if (!args.ContainsKey("u") && !args.ContainsKey("b")){
                args["u"] = null;
            }

            // set all status flags to true if "A" is true
            if (args.ContainsKey("A")){
                args["o"] = null;
                args["d"] = null;
                args["i"] = null;
                args["O"] = null;
            }
            
            // Create needed lists
            string onlineMatches    = "```css\n-- ONLINE --";
            string noDisturbMatches = "```diff\n-- DO NOT DISTURB --";
            string idleMatches      = "```fix\n-- IDLE --";
            string offlineMatches   = "```\n-- Offline --";

            // Iterate array until requried number of users gotten, or end of array reached
            int matchesFound = 0;
            UserStatus currentMemberStatus;
            foreach (var currentMember in memberList){
                if (currentMember.Presence == null){
                    currentMemberStatus = UserStatus.Offline;
                }else{
                    currentMemberStatus = currentMember.Presence.Status;    // Status of current member
                }
                bool statusCheck = false;                                   // Flag that determines if user passes params
                
                // Check status matches required, and mark
                if      (args.ContainsKey("o") && currentMemberStatus == UserStatus.Online)         {statusCheck = true;}
                else if (args.ContainsKey("d") && currentMemberStatus == UserStatus.DoNotDisturb)   {statusCheck = true;}
                else if (args.ContainsKey("i") && currentMemberStatus == UserStatus.Idle)           {statusCheck = true;}
                else if (args.ContainsKey("O") && currentMemberStatus == UserStatus.Offline)        {statusCheck = true;}   // Todo: Check why presence returns null

                // If status check passed, check for user type, pagenum and re-evaluate
                if (statusCheck){
                    if (args.ContainsKey("u") && !currentMember.IsBot){
                        matchesFound++;
                        if (matchesFound < dispCount*(pageNum-1)){  // Unmark for adding if below required pages
                            statusCheck = false;
                        }
                    }else if (args.ContainsKey("b")){
                        matchesFound++;
                        if (matchesFound < dispCount*(pageNum-1)){  // Unmark for adding if below required pages
                            statusCheck = false;
                        }
                    }
                }
                
                // If user still passed, add to relevant output list
                if (statusCheck){
                    string formattedLine = "";
                    if (currentMember.Nickname != null){
                        formattedLine = String.Format("{0,-18} | ({1})", currentMember.Username + "#" +currentMember.Discriminator, currentMember.Nickname);
                    }else{
                        formattedLine = currentMember.Username + "#" +currentMember.Discriminator;
                    }

                    if (currentMemberStatus == UserStatus.Online){
                        onlineMatches = onlineMatches + "\n " + formattedLine;
                    }else if (currentMemberStatus == UserStatus.DoNotDisturb){
                        noDisturbMatches = noDisturbMatches + "\n-" + formattedLine;
                    }else if (currentMemberStatus == UserStatus.Idle){
                        idleMatches = idleMatches + "\n " + formattedLine;
                    }else{
                        offlineMatches = offlineMatches + "\n " + formattedLine;
                    }
                }
                if (matchesFound >= dispCount*(pageNum)+1){
                    break;
                }
            }

            // Finally, print user lists to chat
            if (args.ContainsKey("o") && onlineMatches.Length > 19){
                await ctx.RespondAsync(onlineMatches + "\n```");
            }
            if (args.ContainsKey("d") && onlineMatches.Length > 28){
                await ctx.RespondAsync(noDisturbMatches + "\n```");
            }
            if (args.ContainsKey("i") && onlineMatches.Length > 17){
                await ctx.RespondAsync(idleMatches + "\n```");
            }
            if (args.ContainsKey("O") && onlineMatches.Length > 17){
                await ctx.RespondAsync(offlineMatches + "\n```");
            }
            await ctx.RespondAsync($"`matches in range: {matchesFound+1}, page {pageNum}`");
        }


        // Responds with an embed containing bot info (and a randomly chosen fact thingo)
        [Command("about"), Description("Returns bot info"), Aliases("info")]
        public async Task AboutAsync(CommandContext ctx){
            
            // -- TODO: populate these --
            int activeServers = 2;
            string dSharpVersion = "<TODO>";
            string botVersion = "0.2";
            string randomFact = "Such obedience, many corporeal form - *wowe!*";

            // Create an embed object
            DiscordEmbedBuilder embedOut = new DiscordEmbedBuilder{
                Url = "https://github.com/AgentAileron/DOG-E-Node-VII",
                ThumbnailUrl = "https://cdn.discordapp.com/avatars/494447566428307469/5b602e21cb80a186edbf2728f72ff40a.png?size=512",
                Description = $"**Maintained by <@!211776725875556352> **\n{randomFact}\n",
                Color = new DiscordColor(221, 102, 42)
            };

            // Populate embed with info
            embedOut.WithAuthor("Dog-like Obedience: GNU - Experimental Node mk7");
            embedOut.WithFooter($"Active on {activeServers} servers | D#+ v{dSharpVersion} | DN7 v{botVersion}", 
                        "https://i.imgur.com/qnvjk8C.png");
            embedOut.AddField("Want a feature added?", Formatter.MaskedUrl("Request it here!",new Uri("http://bit.ly/DN7_FeatReq"),"Flag{0man_4dd_f34tur3s}"));
            embedOut.AddField("Add me to your own server!", Formatter.MaskedUrl("Default Permissions", 
                                new Uri("https://discordapp.com/oauth2/authorize?client_id=494447566428307469&scope=bot&permissions=1341643968")));

            await ctx.RespondAsync("",false,embedOut.Build());    // Output embed (NB: 3rd arg in respondasync)
        }


        // Responds with the first n results of a google search (TODO: Add more metasearch providers)
        [Command("search"), Aliases("s")]
        [Description(@"**Runs a Google custom search on the target site and returns the results**

        `d`    - DuckDuckGo
        `m`    - Music: Soundcloud, Bandcamp
        `yt`   - Youtube

        `wiki` - Wikipedia
        `dev`  - Dev help: stackoverflow, MSDN, MDN
        `git`  - Github

        `red`  - Reddit
        `twt`  - Twitter
        `buy`  - Online shopping: eBay, Amazon

        `cad`  - CAD: Thingiverse, GrabCAD, etc.
        `art`  - Art: Deviantart, Weasyl, FA")]
        public async Task SearchAsync(CommandContext ctx, [Description("<Optional> custom search to use")] string argIn, 
                                                        [Description("EG: *$s -wiki Godwin's Law*")] [RemainingText] string searchInput){
            await ctx.TriggerTypingAsync();

            // Initialise custom search instance
            CustomsearchService gSearch = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer{
                ApplicationName = "DOGENode7",
                ApiKey = Reg.Util.GetFileContents(@"./auth_token.txt")[4]
            });

            string engineCX = "";   // Will define custom engine to use
            string engineName = ""; // Will define the name of the custom engine used
            bool isNSFW = false;    // Used to bar NSFW searches in SFW channel
            
            // Initialise embed
            var embedOut = new DiscordEmbedBuilder{
                Color = new DiscordColor(0,0,0)
            };

            // Check that an arg was passed
            if (argIn[0] == '-'){
                string engineArg = argIn.Substring(1);

                // Assign relevant engine based on arg
                switch (engineArg){
                    case ("d"):   engineName = "DuckDuckGo";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:a3sd42z3mdw"; break;
                    case ("dev"): engineName = "Developer";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:qkseh2q5d_0"; break;
                    case ("git"): engineName = "GitHub";
                                    embedOut.Color = new DiscordColor(  0,  0,  0); engineCX = "013372514763418131173:fjappre0o6y"; break;
                    case ("wiki"):engineName = "WikiPedia";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:ws5k1hksq_m"; break;
                    case ("yt"):  engineName = "YouTube";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:ggvh-vwmp4u"; break;
                    case ("red"): engineName = "Reddit";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:fakt0ssmgzq"; break;
                    case ("m"):   engineName = "Music";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:wpldyrlicbs"; break;
                    case ("twt"): engineName = "Twitter";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:eyrkat6dklq"; break;
                    case ("cad"): engineName = "CAD / 3D Printing";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:1ruakjrkgke"; break;
                    case ("buy"): engineName = "Shopping";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:ugbcijzelqu"; break;
                    case ("art"): engineName = "Art";
                                    embedOut.Color = new DiscordColor(000,000,000); engineCX = "013372514763418131173:c5pujwhu4ue"; break;
                    default:      engineName = "Google";
                                    embedOut.Color = new DiscordColor( 72,133,237); engineCX = "013372514763418131173:osz9az3ojby"; break;
                }
            }else{
                searchInput = argIn + " " + searchInput;
                engineName = "Google"; 
                embedOut.Color = new DiscordColor( 72,133,237); 
                engineCX = "013372514763418131173:osz9az3ojby";
            }


            // Exit when NSFW search requested in SFW channel
            if (isNSFW && !ctx.Channel.IsNSFW){
                await ctx.RespondAsync("`This is an NSFW search - it can only be done within NSFW channels`");
                return;     // Return if NSFW search called from SFW channel
            }

            // Set custom engine and search query
            CseResource.ListRequest listRequest = gSearch.Cse.List(searchInput);
            listRequest.Cx = engineCX; 

            // Execute the search
            var search = listRequest.Execute();
            int counter = 0;

            // Add top x results to embed
            foreach (var item in search.Items){
                embedOut.AddField(item.Title, item.Link);
                if (counter >= 5){          // Define number of resutls to output here
                    break;
                }
                counter++;
            }

            embedOut.WithAuthor($"{engineName} Search Results");
            embedOut.Description = $"*Query: {searchInput}*";

            await ctx.RespondAsync("",false,embedOut.Build());
        }


        // Temp command for testing
        [Command("test"), Description("Temp function"), Hidden, RequireOwner]
        public async Task tempTestAsync(CommandContext ctx){

            await DogeNode7.ProgramCode.bot.UpdateStatusAsync(new DiscordGame("Memes"), UserStatus.Online);
            await ctx.RespondAsync("*mmm*, delicious federal crimes");
        }


    } // Class boundary
} // NameSpace boundary