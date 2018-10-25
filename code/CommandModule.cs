/* COMMAND MODULE FOR DOG-E Node VII
 * AgentAileron 2018
 * LM: 22-10-2018
*/

using System;
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
        [Command("hello"), Description("Returns a hello to the calling user")]
        public async Task Hello(CommandContext ctx){
            await ctx.RespondAsync($"Hi there {ctx.User.Mention}");
        }

        // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        [Command("greet"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
        public async Task Greet(CommandContext ctx, [Description("The user to say hi to.")] DiscordMember member){
            // Trigger typing indicator for bot
            await ctx.TriggerTypingAsync();
            
            // Get emoji
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            // Print out - NB: '{}' are escaped, insides are parsed as strings and concat'd
            await ctx.RespondAsync($"{emoji} Hello, {member.Mention}!");
        }


        [Command("uptime"), Description("Returns how long the bot has been contiguously online for")]
        public async Task repeat(CommandContext ctx){

            //TimeSpan timeelapsed = DateTime.Now - BotStats.starttime;       // Gets actual time since bot launch
            TimeSpan timeelapsed = new TimeSpan(0, 1, 30, 0, 0);            // Sets debug timespan

            // Creates array that hold values for each metric, and a bool for if they're greater than 0 (IE: whether to include them)
            double[] timeinfo = {
                Math.Floor((double)(timeelapsed.Seconds)),
                Math.Floor((double)(timeelapsed.Minutes)),
                Math.Floor((double)(timeelapsed.Hours  )),
                Math.Floor((double)(timeelapsed.Days   ))
            };

            string[] strtimenames = {" seconds", " minutes", " hours", " days"};

            // <= 2 mins
            if (timeelapsed.TotalMinutes <= 2){
                timeinfo[0] += (timeinfo[1] * 60);
                timeinfo[1] = 0;
            }
            // <= 90 mins
            else if (timeelapsed.TotalMinutes <= 90){
                timeinfo[1] += (timeinfo[2] * 60);
                timeinfo[2] = 0;
            }
            // <= 48 hrs
            else if (timeelapsed.TotalHours <= 48){
                timeinfo[2] += (timeinfo[3] * 60);
                timeinfo[3] = 0;
            }

            string timeOut = "";
            int i = 0;
            bool andused = false;

            while (i < 3){
                if (timeinfo[i] > 0){
                    if (andused){
                        timeOut = timeinfo[i].ToString() + strtimenames[i] + ", " + timeOut;
                    }else if ((i == 3) || (i == 2 && timeinfo[3] == 0)){
                        timeOut = timeinfo[i].ToString() + strtimenames[i] + timeOut;
                    }else{
                        timeOut = "and " + timeinfo[i].ToString() + strtimenames[i];
                        andused = true;
                    }
                }
                i++;
            } 

            await ctx.RespondAsync($"I have been running for `{timeOut}`");

        }

    } // Class boundary
} // NameSpace boundary