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


        [Command("repeat"), Description("repeats whatever text is given")]
        public async Task repeat(CommandContext ctx, [Description("Message to repeat")] string message){
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync(message);

        }

    } // Class boundary
} // NameSpace boundary