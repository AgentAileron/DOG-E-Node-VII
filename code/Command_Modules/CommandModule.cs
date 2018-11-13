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


namespace CommandModules{
    [RequirePermissions(Permissions.ReadMessageHistory)]
    public class CommandListTopLevel{

        
        // Returns hello to the calling user (TODO: beef it up a tad)
        [Command("hello"), Description("Returns a hello to the calling user"), Aliases("hey", "hi", "g'day")]
        public async Task HelloAsync(CommandContext ctx){
            await ctx.RespondAsync($"G'day {ctx.User.Mention}!");
        }


        // Greets a specified user
        [Command("greet"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
        public async Task GreetAsync(CommandContext ctx, [Description("The user to say hi to.")] DiscordMember member){
            // Trigger typing indicator for bot
            await ctx.TriggerTypingAsync();
            
            // Get emoji
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            // Print out - NB: '{}' are escaped, insides are parsed as strings and concat'd
            await ctx.RespondAsync($"{emoji} Hello there, {member.Mention}!");
        }


        // Responds with a nicely formatted description of uptime elapsed
        [Command("uptime"), Description("Returns how long the bot has been online for / time since last reboot")]
        public async Task UpTimeAsync(CommandContext ctx){

            await ctx.TriggerTypingAsync();     // Trigger typing indicator for bot

            TimeSpan time_elapsed = DateTime.Now - DogeNode7.BotStats.starttime;       // Gets actual time since bot launch
            //TimeSpan time_elapsed = new TimeSpan(0, 0, 13, 0, 0);            // Sets debug timespan

            string timeOut = Reg.StatMethod.FormatTime(time_elapsed);

            await ctx.RespondAsync($"I have been running for `{timeOut}`");     // Output the constructed response message

        }


        // Returns a list of online users
        [Command("online_users"), Description("Returns list of online users (useful for Discord over irc)"), Aliases("w")]
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
            Dictionary<string,string> args = Reg.Utility.GetArgs(ctx.Message.ToString(), expectedArgs);

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
            string onlineMatches    = "```css \n**Online**";
            string noDisturbMatches = "```diff \n**Do Not Disturb**";
            string idleMatches      = "```fix \n**Idle**";
            string offlineMatches   = "``` \n**Offline**";

            // Iterate array until requried number of users gotten, or end of array reached
            int counter = 0;
            int matchesFound = 0;
            while (counter < memberList.Length && matchesFound < dispCount+1){
                var currentMember = memberList[counter];                    // Current member in server list
                var currentMemberStatus = currentMember.Presence.Status;    // Status of current member
                bool statusCheck = false;                                   // Flag that determines if user passes params

                // Check status matches required, and mark
                if      (args.ContainsKey("o") && currentMemberStatus == UserStatus.Online)         {statusCheck = true;}
                else if (args.ContainsKey("d") && currentMemberStatus == UserStatus.DoNotDisturb)   {statusCheck = true;}
                else if (args.ContainsKey("i") && currentMemberStatus == UserStatus.Idle)           {statusCheck = true;}
                else if (args.ContainsKey("O") && currentMemberStatus == UserStatus.Offline)        {statusCheck = true;}

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
                    string formattedLine = String.Format("{1,15} | {2}#{3}", currentMember.Nickname, currentMember.Username, currentMember.Discriminator);
                    if (currentMemberStatus == UserStatus.Online){
                        onlineMatches = String.Concat(onlineMatches, "\n ", formattedLine);
                    }else if (currentMemberStatus == UserStatus.DoNotDisturb){
                        onlineMatches = String.Concat(onlineMatches, "\n-", formattedLine);
                    }else if (currentMemberStatus == UserStatus.Idle){
                        onlineMatches = String.Concat(onlineMatches, "\n ", formattedLine);
                    }else{
                        onlineMatches = String.Concat(onlineMatches, "\n ", formattedLine);
                    }
                }
                counter++;
            }

            // Finally, print user lists to chat
            if (args.ContainsKey("o") && onlineMatches.Length > 18){
                await ctx.RespondAsync(onlineMatches + "\n```");
            }
            if (args.ContainsKey("d") && onlineMatches.Length > 27){
                await ctx.RespondAsync(noDisturbMatches + "\n```");
            }
            if (args.ContainsKey("i") && onlineMatches.Length > 16){
                await ctx.RespondAsync(idleMatches + "\n```");
            }
            if (args.ContainsKey("d") && onlineMatches.Length > 16){
                await ctx.RespondAsync(offlineMatches + "\n```");
            }
            await ctx.RespondAsync($"`matches in range: {matchesFound}, page {pageNum}`");
        }


        // Responds with an embed containing bot info (and a randomly chosen fact thingo)
        [Command("about"), Description("Returns bot info"), Aliases("info")]
        public async Task AboutAsync(CommandContext ctx){
            
            // -- TODO: populate these --
            int activeServers = 2;
            string dSharpVersion = "3.4.002";
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

            DiscordEmbed output = embedOut.Build();
            await ctx.RespondAsync("",false,output);    // Output embed (NB: 3rd arg in respondasync)
        }


        // This module is restricted access - use it for testing things
        [Command("test"), Description("A temp function - very unstable"), Hidden, RequireOwner]
        public async Task tempTestAsync(CommandContext ctx){
            await DogeNode7.ProgramCode.bot.UpdateStatusAsync(new DiscordGame("Wofe wofe, am DOG-E"), UserStatus.DoNotDisturb);
            await ctx.RespondAsync("done.");
        }


    } // Class boundary
} // NameSpace boundary