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

        // Returns the 'OTP' percentage match of two input users
        [Command("otp"), Description("**Finds the OTP strength of two users - can change over time**")]
        public async Task OTPAsync(CommandContext ctx, [Description("First user")] DiscordMember member1, [Description("Second user")] DiscordMember member2){
            await ctx.RespondAsync("success");
        }

        // Generates a random number and returns it
        [Command("num"), Aliases("generate", "gen")]
        [Description(@"**Returns a random integer within the specified range**
        
        `-b` - output a boolean *(0 or 1)*

        `-l <number>` - define lower bound *(default is 0)*
        `-u <number>` - define upper bound *(default is 10)*")]
        public async Task RandNumAsync(CommandContext ctx){
            Random random = new Random();   // Create a random generator

             // Initialise expected arguments to pass
            Dictionary<string,bool> expectedArgs = new Dictionary<string, bool>();
                expectedArgs["l"] = true;  // Lower number bound (default = 0)
                expectedArgs["u"] = true;  // Upper number bound (default = 10)

                expectedArgs["b"] = false;  // Only outputs 0 or 1

            // Interpret args, set defaults if none given
            Dictionary<string,string> args = Reg.Util.GetArgs(ctx.Message.ToString(), expectedArgs);

            // Reply with boolean and exit
            if (args.ContainsKey("b")){
                var result = random.Next(0,2);
                if (result == 1){
                    await ctx.RespondAsync(Reg.StrUtil.CSSBlock("True"));
                }else{
                    await ctx.RespondAsync(Reg.StrUtil.FixBlock("False"));
                }
                return;
            }



        }

    } // Class boundary
} // NameSpace boundary