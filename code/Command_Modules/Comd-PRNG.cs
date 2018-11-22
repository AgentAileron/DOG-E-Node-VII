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
    [Group("prng"), Aliases("rng")]
    [Description("Commands that ~~use a pseudo random number generator~~ **are expertly decided with advanced AI!**")]
    public class CommandListPseudoRNG{

        [Command("otp"), Description("Finds the OTP strength of two users - can change over time")]
        public async Task stuff(CommandContext ctx, [Description("First user")] DiscordMember member1, [Description("Second user")] DiscordMember member2){
            await ctx.RespondAsync("success");
        }
    } // Class boundary
} // NameSpace boundary