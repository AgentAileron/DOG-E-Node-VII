/* MAIN PROGRAM FOR DOG-E Node VII
 * AgentAileron 2018
 * LM: 22-10-2018
 */

using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using DSharpPlus;


namespace DOGE_E{
    class Program{

        static DiscordClient bot;
        static string auth_token;

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

            // Begin asynchronous instance
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        // == ASYNC EXECUTION == //
        static async Task MainAsync(string[] args){
            // Initialise bot
            bot = new DiscordClient(new DiscordConfiguration{
                Token = auth_token,
                TokenType = TokenType.Bot
            });

            bot.MessageCreated += async e =>{
                if (e.Message.Content.ToLower().StartsWith("!ping"))
                    await e.Message.RespondAsync("pong!");
            };

            await bot.ConnectAsync();
            await Task.Delay(-1);

        }

    }
}
