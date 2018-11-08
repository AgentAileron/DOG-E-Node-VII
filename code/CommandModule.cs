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
    public class CommandListRegular{


        
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

            string timeOut = Reg.BotStats.FormatTime(time_elapsed);

            await ctx.RespondAsync($"I have been running for `{timeOut}`");     // Output the constructed response message

        }



        [Command("about"), Description("Returns bot info")]
        public async Task About(CommandContext ctx){
            DiscordEmbedBuilder embedOut = new DiscordEmbedBuilder();
            //embedOut.Color = new DiscordColor();
            embedOut.Title = "Title";
            embedOut.WithAuthor("LOLOL My name here I think?", "https://Google.com");
            embedOut.Url = "https://youtube.com.au"; // Of title
            embedOut.Description = "Description";
            embedOut.ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Social_Network_Analysis_Visualization.png/220px-Social_Network_Analysis_Visualization.png";
            embedOut.ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/90/Petersen_graph_3-coloring.svg/1200px-Petersen_graph_3-coloring.svg.png";
            //embedOut.Timestamp = new DateTime();

            embedOut.AddField("Name1","Value1", true);
            embedOut.AddField("Name2","Value2", true);
            embedOut.AddField("Name3","Value3");

            DiscordEmbed output = embedOut.Build();

            await ctx.RespondAsync("",false,output);
        }

    } // Class boundary
} // NameSpace boundary