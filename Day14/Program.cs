using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14 {
    class Program {
        static void Main() {
            List<char> start = new List<char>();
            Dictionary<string,char> chemicals = new Dictionary<string,char>();
            Dictionary<string,Int64> count = new Dictionary<string,long>();

            using(StreamReader file = new StreamReader("input.txt")) {
                while(true) {
                    char inp = (char)file.Read();
                    if(inp == '\n') //read to end of first line
                        break;
                    start.Add(inp);
                }
                file.Read(); //ignore whitespace line
                while(!file.EndOfStream) {
                    string[] tk = file.ReadLine().Split(" -> ");
                    chemicals[tk[0]] = tk[1][0];
                }
            }

            for(int i = 1; i < start.Count; i++) {
                string each = "";
                each += start[i - 1].ToString() + start[i].ToString();
                count[each] = count.ContainsKey(each) ? count[each] + 1 : 1;
            }

            //part 1: i < 10
            //part 2: i < 40
            for(int i = 0; i < 40; i++) {
                count = smartStep(count,chemicals);
            }
            Console.WriteLine("Difference between most common and least common after 40 steps: " + smart_count(count,start[start.Count - 1]));
        }

        static Dictionary<string,Int64> smartStep(Dictionary<string,Int64> tracker,Dictionary<string,char> chemicals) {
            Dictionary<string,Int64> temp = new Dictionary<string,Int64>(tracker);
            //idea: insert C into NH -> increment NC, CH by 1
            foreach(KeyValuePair<string,Int64> entry in tracker) {
                if(chemicals.ContainsKey(entry.Key)) {
                    string part1, part2;
                    part1 = entry.Key.Insert(1,chemicals[entry.Key].ToString());
                    part2 = part1.Substring(1,2);
                    part1 = part1.Remove(2);
                    //we now have two strings, ie NC and CH
                    temp[part1] = temp.ContainsKey(part1) ? temp[part1] + entry.Value : entry.Value;
                    temp[part2] = temp.ContainsKey(part2) ? temp[part2] + entry.Value : entry.Value;
                    temp[entry.Key] -= entry.Value;
                }
            }
            return temp;
        }

        static Int64 smart_count(Dictionary<string,Int64> count,char last) {
            Dictionary<char,Int64> specifics = new Dictionary<char,long>();
            foreach(KeyValuePair<string,Int64> entry in count) {
                char c1;
                c1 = entry.Key[0]; //count first letter of each pair
                specifics[c1] = specifics.ContainsKey(c1) ? specifics[c1] + entry.Value : entry.Value;
            }
            specifics[last] += 1; //account for last character in the input string since it will never ever be modified
            return specifics.Values.Max() - specifics.Values.Min(); //don't even need to know specifically what it is lmao
        }
    }
}
