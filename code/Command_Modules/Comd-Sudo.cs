/* SUPERUSER COMMAND MODULE FOR DOG-E Node VII
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
    [Group("sudo"), Aliases("su"), Hidden]
    [Description("**Restricted, Administrative commands** \ntry `help <command>` for more info")]
    public class CommandListSudo{
        
        // Set or refresh bot presence
        [Command("set_status"), Aliases("update_status", "presence"), Description("Sets bot presence"), RequireOwner]
        public async Task tempTestAsync(CommandContext ctx){
            await ctx.TriggerTypingAsync();
            
            // Initialise expected arguments to pass
            Dictionary<string,bool> expectedArgs = new Dictionary<string, bool>();
                expectedArgs["O"] = false;  // Set status to 'online'
                expectedArgs["D"] = false;  // Set status to 'do not disturb'
                expectedArgs["I"] = false;  // Set status to 'idle'
                expectedArgs["F"] = false;  // Set status to 'offline'
                expectedArgs["m"] = true;   // New presence message

            // Get any passed args
            Dictionary<string,string> args = Reg.Util.GetArgs(ctx.Message.ToString(), expectedArgs);

            if (args.ContainsKey("O")){   // Set status online
                DogeNode7.BotStats.selfStatus = UserStatus.Online;
            }else if (args.ContainsKey("D")){   // Set status do not disturb
                DogeNode7.BotStats.selfStatus = UserStatus.DoNotDisturb;
            }else if (args.ContainsKey("I")){   // Set status idle
                DogeNode7.BotStats.selfStatus = UserStatus.Idle;
            }else if (args.ContainsKey("F")){   // Set status offline
                DogeNode7.BotStats.selfStatus = UserStatus.Offline;
            }

            if (args.TryGetValue("m", out string newPresence)){     // Set new bot presence
                DogeNode7.BotStats.selfPresence = newPresence;
            }

            // Finally, update actual bot presence with cached values and return success
            await ctx.RespondAsync(DogeNode7.BotStats.selfStatus.ToString() + ": " + DogeNode7.BotStats.selfPresence);
            await DogeNode7.ProgramCode.bot.UpdateStatusAsync(new DiscordGame(DogeNode7.BotStats.selfPresence), DogeNode7.BotStats.selfStatus);
        }

    } // Class boundary
} // NameSpace boundary