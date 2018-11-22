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

        [Command("otp")]
        public async Task stuff(CommandContext ctx){
            await ctx.RespondAsync("success");
        }
    } // Class boundary
} // NameSpace boundary