/* PRNG COMMAND MODULE FOR DOG-E Node VII
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

using DogeNode7;


namespace CommandModules{
    [Group("prng"), Aliases("rng")]
    [Description("Commands that ~~use a pseudo random number generator~~ **are expertly decided with advanced AI!**")]
    public class CommandListPseudoRNG{

        // Returns the 'OTP' percentage match of two input users
        [Command("otp")]
        [Description("**Finds the OTP strength of two users - can change over time**")]
        public async Task OTPAsync(CommandContext ctx, [Description("First user")] DiscordMember member1, [Description("Second user")] DiscordMember member2){
            await ctx.TriggerTypingAsync();

            string botSelfId = BotStats.selfId;
            
            // Create the embed to eventually output
            DiscordEmbedBuilder embedOut = new DiscordEmbedBuilder{

            };
            embedOut.WithAuthor($"{member1.Username} x {member2.Username}");

            // Is either input member this bot?
            if ((member1.Id.ToString() == botSelfId) || (member2.Id.ToString() == botSelfId)){

                // Are both members this bot?
                if (member1 == member2){
                    embedOut.Description = $"**Compatibility: ??%**\n*Err... you want to pair me with myself?*";
                    embedOut.Color = new DiscordColor(255,255,255);
                }else{
                    embedOut.Description = $"**Compatibility: 100%**\n";
                    embedOut.Color = new DiscordColor(0,255,0);
                    
                    // Find which member is not this bot
                    DiscordMember otherMember;
                    if (member1.Id.ToString() != botSelfId){
                        otherMember = member1;
                    }else{
                        otherMember = member2;
                    }
                    
                    // Is other member a bot?
                    if (otherMember.IsBot){
                        embedOut.Description += "*Heck yea mechanical frien! ^-^*"; // Other member is a bot
                    }else{
                        embedOut.Description += "*Heck yea hooman! :D*"; // Other member is a human
                    }
                }

            }else{      // Neither member is this bot 

                // Get the seeded generator for the otp based on usernames
                double nameVal1 = 0;
                double nameVal2 = 0;
                foreach (var letter in member1.Username){
                    nameVal1 += Convert.ToUInt32(letter);
                }
                foreach (var letter in member2.Username){
                    nameVal2 += Convert.ToUInt32(letter);
                }

                // Constant calculations (initial compatibility score)
                Random seededrng = new Random((int)Math.Floor(Math.Max(nameVal1, nameVal2) % Math.Min(nameVal1, nameVal2)));    // PRNG (Usernames)
                int Compatibility = seededrng.Next() % 100;     // Val based on names only (constant)

                int offsetRange = seededrng.Next(0,25); // Used as constant range for offset variation

                // Variable calculations (offset)
                Random rng = new Random();   // unseeded PRNG
                int offset = rng.Next(-1*offsetRange,offsetRange);

                // Halve value til compatibility is within valid range (since int, floor division)
                while (Compatibility + offset > 100 || Compatibility + offset < 0){
                    offset = offset / 2;
                }

                Compatibility += offset;
                embedOut.Description = $"**Compatibility: {Compatibility}%**\n ";    // Set compatibility score, first half of description output
                
                // ----------------------------

                // Determine what kind of matching has been made
                string wordListPath;
                if (member1 != member2){    // Unique members
                    if (member1.IsBot || member2.IsBot){    // At least 1 member is a bot
                        if (member2.IsBot && member2.IsBot){    // Both members are bots
                            wordListPath = @"./Phrase_Lists/otp-2bots.txt";
                        }else{  // 1 member is bot, 1 is human
                            wordListPath = @"./Phrase_Lists/otp-bothuman.txt";
                        }
                    }else{  // Neither members are bots
                        wordListPath = @"./Phrase_Lists/otp-2humans.txt";
                    }
                }else{  // Self OTP
                    if (member1.IsBot){ // self OTP of bot
                        wordListPath = @"./Phrase_Lists/otp-selfbot.txt";    
                    }else{  // self OTP of human
                        wordListPath = @"./Phrase_Lists/otp-selfhuman.txt";
                    }
                }

                string[] wordList = Reg.Util.GetFileContents(wordListPath); // Get appropriate wordList for OTP found

                // Iterate to correct line in file, and add to description (autoadjusts to number of lines in file)
                string CompatDesc = "";
                for (int i=0; i < wordList.Length; i++){
                    if ((100*i) / wordList.Length >= Compatibility){
                        CompatDesc = wordList[i];
                        break;
                    }
                }

                if (CompatDesc != ""){ embedOut.Description += Reg.StrUtil.Italic(CompatDesc); }    // Append the found compatibility description to the embed (if not blank)

                // ----------------------------

                // Based on compatibility percent, determine colour to use
                int[] minCol = {230,0,0};
                int[] maxCol = {64,255,0};
                int[] outColour = {0,0,0};

                for (int j=0; j <= 2; j++){
                    outColour[j] = minCol[j] + (int)((maxCol[j] - minCol[j]) * Compatibility / 100);
                }

                embedOut.Color = new DiscordColor((byte)outColour[0],(byte)outColour[1],(byte)outColour[2]);  // Set embed colour

            }

            embedOut.WithFooter("<Generated dialogue goes here>");  // Temp
            embedOut.AddField("Ship Names", "<Generated ship names go here>");   // Temp

            // Finally, construct embed and output
            await ctx.RespondAsync(null,false,embedOut.Build());
        }

        // Generates a random number and returns it
        [Command("num"), Aliases("generate", "gen")]
        [Description(@"**Returns a random integer within the specified range**
        
        `-b` - output a boolean *(True or False)*

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

            int lowerBound = 0;
            int upperBound = 10;

            // Check if a lower bound was given
            if (args.ContainsKey("l")){
                Int32.TryParse(args["l"], out lowerBound);
            }

            // Check if an upper bound was given
            if (args.ContainsKey("u")){
                Int32.TryParse(args["u"], out upperBound);
            }

            await ctx.RespondAsync(Reg.StrUtil.InlineCode(random.Next(lowerBound, upperBound + 1).ToString())); // Output random num
        }

    } // Class boundary
} // NameSpace boundary