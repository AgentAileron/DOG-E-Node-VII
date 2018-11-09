/* MAIN PROGRAM FOR DOG-E Node VII
 * AgentAileron 2018
 * LM: 22-10-2018
*/

using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;


namespace DogeNode7{
    // Class to hold bot stats
    public class BotStats{
        public static DateTime starttime = DateTime.Now;
    }


    // Main Program Class
    class ProgramCode{

        static string auth_token;
        static DiscordClient bot;
        static CommandsNextModule cmd_module;

        // == INITIALISATION == //
        static void Main(string[] args){

            // -- Attempt to load in the auth token from file, halt execution otherwise --
            try{
                System.IO.StreamReader f = new System.IO.StreamReader(@"./auth_token.txt");
                auth_token = f.ReadLine();
            }catch (Exception e){
                // FileNotFound Exception
                if (e is FileNotFoundException){
                    Console.WriteLine("Error loading auth token: auth_token.txt does not exist!");
                    Environment.Exit(2);
                }
                // Unexpected exception type
                else throw;
            }

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
                if (e.Message.Content.ToLower().StartsWith("!ping"))
                    await e.Message.RespondAsync("pong!");
            };*/

            // Initialise Command Interpreter
            cmd_module = bot.UseCommandsNext(new CommandsNextConfiguration{
                StringPrefix = "$"
            });

            cmd_module.RegisterCommands<CommandListTopLevel>();     // Register and check all regular defined commands

            // Async triggers
            await bot.ConnectAsync();
            await Task.Delay(-1);
        }

    } // Class boundary
} // NameSpace boundary
