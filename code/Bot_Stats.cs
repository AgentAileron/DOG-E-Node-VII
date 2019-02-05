/* Methods pertaining to bot statistics
 * DOG-E Node VII
 * AgentAileron 2018
*/

using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;


namespace Reg{
    public class StatMethod{
        
        // Formats a given TimeSpan into human readable format
        public static string FormatTime(TimeSpan time_elapsed){
            // Creates array that hold values for each metric, and a bool for if they're greater than 0 (IE: whether to include them)
            double[] timeinfo = {
                Math.Floor((double)(time_elapsed.Seconds)),
                Math.Floor((double)(time_elapsed.Minutes)),
                Math.Floor((double)(time_elapsed.Hours  )),
                Math.Floor((double)(time_elapsed.Days   ))
            };

            string[] strtimenames = {" seconds", " minutes", " hours", " days"};

            // -- Change values to suit output rules -- //

            // <= 2 mins
            if (time_elapsed.TotalMinutes <= 2){
                timeinfo[0] += (timeinfo[1] * 60);
                timeinfo[1] = 0;
            }
            // <= 90 mins
            else if (time_elapsed.TotalMinutes <= 90){
                timeinfo[1] += (timeinfo[2] * 60);
                timeinfo[2] = 0;
            }
            // <= 48 hrs
            else if (time_elapsed.TotalHours <= 48){
                timeinfo[2] += (timeinfo[3] * 24);
                timeinfo[3] = 0;
            }
            
            // -- Construct the string representing uptime -- //

            // Find out how many non-zero values exist
            int count = 0;
            int i = 0;
            while (i < 4){
                if (timeinfo[i] > 0){count ++ ;}
                i ++;
            }

            string timeOut = "";    // Time string will be constructed in this
            bool andused = false;   // True when one element is added and there are more to add

            i = 0;                  // Reset counter for for loop

            // Construct time elapsed string
            while (i < 4){
                // Zero vals won't be outputted
                if (timeinfo[i] > 0){
                    // Some text is in the string
                    if (andused){
                        timeOut = timeinfo[i].ToString() + strtimenames[i] + ", " + timeOut;
                    // No text in the string yet, and there will be more than one
                    }else if (count > 1){
                        timeOut = "and " + timeinfo[i].ToString() + strtimenames[i];
                        andused = true;
                    // There is only one element in the string to place
                    }else{
                        timeOut = timeinfo[i].ToString() + strtimenames[i];
                        break;
                    }
                }
                i++;
            }

            return timeOut;
        }
        
        // Async handler for incoming messages //
        public static async Task messageLogAsync(DiscordMessage msg){
            // TODO
        }

        // Handler for user state changes //
        public static void logUserState(DiscordMember member, DiscordPresence oldPresence, string currStatus){
            string filepath = @"./Data_Cache/User_Tracking/" + member.Id;   // Path to user's statfile
            FileInfo userFile = new FileInfo(filepath);     // FileStream object for user statfile
            
            if (!userFile.Exists){  // Create a new statfile if non-existent for user
                /*  -- STRUCTURE OF STATFILE --
                *   ITEM            |   EXAMPLE
                *   _________________________________________
                *   <Id>            |   000000000000000000
                *   <username>#---- |   Wumpus#0001
                *   <last_state>    |   Online
                *   <last_seen>     |   25/12/2031 12:00:00 AM
                */
                using (FileStream fs = userFile.Create()){  // Create a new statfile for user
                    Byte[] Id = new UTF8Encoding(true).GetBytes($"{member.Id}\n");
                        fs.Write(Id, 0, Id.Length);
                    Byte[] username = new UTF8Encoding(true).GetBytes($"{member.Username}#{member.Discriminator}\n");
                        fs.Write(username, 0, username.Length);
                    Byte[] state = new UTF8Encoding(true).GetBytes($"{currStatus}\n");
                        fs.Write(state, 0, state.Length);
                    Byte[] lastSeen = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + '\n');
                        fs.Write(username, 0, username.Length);
                    
                }
            }

            string[] statFileContents = Reg.Util.GetFileContents(filepath); // Get the current statfile for the user



            
        }

    } // Class boundary
} // NameSpace boundary