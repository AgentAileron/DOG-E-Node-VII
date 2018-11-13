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


        // Returns a list of online users (TODO: limit count)
        [Command("online_users"), Description("Returns list of online users (useful for Discord over irc)"), Aliases("w")]
        public async Task OnlineUsersAsync(CommandContext ctx){
            await ctx.TriggerTypingAsync();

            // get list of all members in guild
            var memberList = ctx.Guild.GetAllMembersAsync().Result.ToArray();

            foreach (var member in memberList){

            }
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
            await DogeNode7.ProgramCode.bot.UpdateStatusAsync(new DiscordGame(""), UserStatus.DoNotDisturb);
            await ctx.RespondAsync("done.");
        }


    } // Class boundary
} // NameSpace boundary