/* Utility methods for DN7
 * DOG-E Node VII
 * AgentAileron 2018
*/

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Reg{

    public class Utility{
        
        // Returns any args passed in a command, based on those expected by the command
        public static Dictionary<string,string> GetArgs(string message, Dictionary<string,bool> expectedArgs){
            // string[] splitString = message.Split(' ', '\n');   // Split input message by space and nl (old but stable - now using method to also match quotes below)

            // Parsing 101 - Discord bot programming puzzle edition (split by whitespace / nl, ignore things within quotes)
            string[] splitString = message.Replace("\\\"","|^|p*2|53(|").Split('\"');  // Replace escaped quotes with (hopefully) unique sequence, split by remaining quotes

            if (splitString.Length % 2 != 1){
                // Ignore last quote if mismatched
                splitString[splitString.Length-2] = splitString[splitString.Length-2] + "\"" + splitString[splitString.Length-1];
                splitString = splitString.Take(splitString.Count() - 1).ToArray();
            }

            // Restore escaped quotes
            for (int i=0; i < splitString.Length; i++){
                splitString[i] = splitString[i].Replace("|^|p*2|53(|","\"");
            }

            foreach (string item in splitString){Console.WriteLine(item);}Console.WriteLine("----\n"); //TEMPTEMP pass

            if (splitString.Length > 1){
                int chunkCounter = 0;
                List<string> tempList = new List<string>();
                // Split the non-quote enclosed portions of the input string by whitespace, and append this into a list
                while (chunkCounter*2 < splitString.Length-1){
                    string[] tempArray = splitString[chunkCounter*2].Split(new char[]{' ', '\n'}); 
                    tempList.AddRange(tempArray);
                    tempList.Add(splitString[chunkCounter*2 + 1]);
                    chunkCounter++;
                }
                splitString = tempList.ToArray(); // Put the final list back into the array
            }else{
                splitString = splitString[0].Split(new char[]{' ', '\n'}); 
            }

            foreach (string item in splitString){Console.WriteLine(item);}Console.WriteLine("----\n"); //TEMPTEMP

            // Remove any whitespace / blank chunks
            splitString = splitString.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            int argsFoundCount = 0;
            List<string> argsFound = new List<string>();     // Holds any args found
            Dictionary<string, string> potentialParams = new Dictionary<string, string>();   // Holds potential input params to args

            // Check all text for input args, and take any potential params to args
            for (int i=1; i < splitString.Length; i++){
               if (splitString[i].Length > 1 && splitString[i].Substring(0,1) == "-"){
                   // If current chunk starts with '-', iterate through to get any args within
                   for (int j=1; j < splitString[i].Length; j++){
                       argsFound.Add(splitString[i].Substring(j,1));
                       argsFoundCount++;
                   }

                   var lastInList = argsFound.LastOrDefault();  // Holds the last arg added to list, or null if empty
                   if (lastInList != null){
                       if (!potentialParams.ContainsKey(lastInList)){
                            potentialParams.Add(lastInList, null);   // If last element in list exists, add to dict if does not already added

                            if (i < splitString.Length - 1){
                                potentialParams[lastInList] = splitString[i+1]; // Set next text chunk as potential param to last arg, if valid and not another arg block
                            }
                        }
                    }
                }
            }
            Dictionary<string,string> strDictOut = new Dictionary<string, string>(); // Final output (arg, value/null)

            // Iterate through found args and determine which are expected
            foreach (string arg in argsFound){
                if (expectedArgs.TryGetValue(arg, out var value) && !strDictOut.ContainsKey(arg)){
                    strDictOut.Add(arg, null);  // a valid arg, not in the dict, was found

                    // Check if this arg expects a param, and add it the output array if so
                    if (value && potentialParams.TryGetValue(arg, out var paramFound)) {
                        strDictOut[arg] = paramFound;
                    }
                }
            }
            return strDictOut;
        }

        
        // Gets all contents of the specified file, and returns its contents as a string array of each line in file
        public static string[] GetFileContents(string filePath, int maxLines=250 ){
            try{
                const Int32 BufferSize = 1024;
                using (var fileStream = File.OpenRead(@filePath)){
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)){
                        string line;
                        List<string> bufferList = new List<string>();
                        while ((line = streamReader.ReadLine()) != null){
                            bufferList.Add(line);
                        }
                        string[] outArray = bufferList.ToArray();
                        return outArray;
                    }
                }


            }catch (Exception e){
                if (e is FileNotFoundException){    // FileNotFound Exception
                    Console.WriteLine($"Error reading file at {@filePath} - does it exist?");
                    return null;
                }
                else throw;     // Unexpected exception type
            }
        }

    } // Class boundary
} // NameSpace boundary