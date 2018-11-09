/* COMMAND MODULE FOR DOG-E Node VII
 * AgentAileron 2018
 * LM: 08-11-2018
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


namespace DogeNode7{
    public class CommandListTopLevel{


        
        // Returns hello to the calling user (TODO: beef it up a tad)
        [Command("hello"), Description("Returns a hello to the calling user"), Aliases("hey", "hi", "g'day")]
        public async Task Hello(CommandContext ctx){
            await ctx.RespondAsync($"G'day {ctx.User.Mention}!");
        }



        // Greets a specified user
        [Command("greet"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
        public async Task Greet(CommandContext ctx, [Description("The user to say hi to.")] DiscordMember member){
            // Trigger typing indicator for bot
            await ctx.TriggerTypingAsync();
            
            // Get emoji
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            // Print out - NB: '{}' are escaped, insides are parsed as strings and concat'd
            await ctx.RespondAsync($"{emoji} Hello there, {member.Mention}!");
        }



        // Responds with a nicely formatted description of uptime elapsed
        [Command("uptime"), Description("Returns how long the bot has been online for / time since last reboot")]
        public async Task UpTime(CommandContext ctx){

            await ctx.TriggerTypingAsync();     // Trigger typing indicator for bot

            TimeSpan time_elapsed = DateTime.Now - BotStats.starttime;       // Gets actual time since bot launch
            //TimeSpan time_elapsed = new TimeSpan(0, 0, 13, 0, 0);            // Sets debug timespan

            string timeOut = Reg.StatMethod.FormatTime(time_elapsed);

            await ctx.RespondAsync($"I have been running for `{timeOut}`");     // Output the constructed response message

        }


        [Command("about"), Description("Returns bot info"), Aliases("info")]
        public async Task About(CommandContext ctx){
            DiscordEmbedBuilder embedOut = new DiscordEmbedBuilder();   // Create embed
            
            // -- TODO: populate these --
            int activeServers = 2;
            string dSharpVersion = "3.4.002";
            string botVersion = "0.2";
            string randomFact = "Such obedience, many corporeal form - *wowe!*";


            // Populate embed with info
            embedOut.Color = new DiscordColor(221, 102, 42);
            embedOut.WithAuthor("Dog-like Obedience: GNU - Experimental Node mk7");
            embedOut.WithUrl("https://github.com/AgentAileron/DOG-E-Node-VII");
            embedOut.Description = $"**Maintained by <@!211776725875556352> **\n{randomFact}\n";
            embedOut.ThumbnailUrl = "https://cdn.discordapp.com/app-icons/494447566428307469/98fb09740e2645f657b1cd6c7e05c957.png?size=256";
            embedOut.WithFooter($"Active on {activeServers} servers | D#+ v{dSharpVersion} | DN7 v{botVersion}", 
                        ""); // -- TODO: Add own dev logo here --

            embedOut.AddField("Want a feature added?","http://bit.ly/DN7_FeatReq");

            DiscordEmbed output = embedOut.Build();
            await ctx.RespondAsync("",false,output);    // Output embed (NB: 3rd arg in respondasync)
        }

    } // Class boundary
} // NameSpace boundary