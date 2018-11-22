/* MAIN PROGRAM FOR DOG-E Node VII
 * AgentAileron 2018
*/

using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;


namespace DogeNode7{
    // Class to hold bot stats
    public class BotStats{
        public const string strPrefix = "$";

        public static DateTime starttime = DateTime.Now;
    }


    // Main Program Class
    class ProgramCode{

        static string auth_token;
        public static DiscordClient bot;
        static CommandsNextModule cmd_module;

        // == INITIALISATION == //
        static void Main(string[] args){

            // -- Attempt to load in the auth token from file, halt execution otherwise --
            auth_token = Reg.Utility.GetFileContents(@"./auth_token.txt")[0];

            // Print a message on successful initialisation
            Console.WriteLine("-//- DOG-E Initialised successfully! -//-");
            Console.WriteLine("Auth token: " + auth_token);
            Console.WriteLine(BotStats.starttime);

            // Begin asynchronous instance
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        // == ASYNC EXECUTION == //
        static async Task MainAsync(string[] args){

            // Re-Initialise bot on async call
            bot = new DiscordClient(new DiscordConfiguration{
                Token = auth_token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            /* Bot received a message notification (Now handled by command modules)
            bot.MessageCreated += async e =>{
                if (e.Message.Content.ToLower().StartsWith("$about"))
                    await e.Message.RespondAsync("pong!");
            };*/

            // Initialise Command Interpreter
            cmd_module = bot.UseCommandsNext(new CommandsNextConfiguration{
                StringPrefix = BotStats.strPrefix
            });

            cmd_module.RegisterCommands<CommandModules.CommandListTopLevel>();     // Register and check all top level commands
            cmd_module.RegisterCommands<CommandModules.CommandListPseudoRNG>();

            await bot.ConnectAsync();   // Connect the bot
            await Task.Delay(-1);       // Wait forever
        }

    } // Class boundary
} // NameSpace boundary