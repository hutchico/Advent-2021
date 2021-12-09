using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day08 {
    class Program {
        static void Main() {
            List<digit> digits = new List<digit>();
            int pt1_count = 0;
            int pt2_sum = 0;

            using(StreamReader file = new StreamReader("input.txt")) {
                string[] raw;
                string[] tk;
                digit line;
                while(!file.EndOfStream) {
                    raw = file.ReadLine().Split(" | ");
                    tk = raw[0].Split(' ');
                    line = new digit();
                    for(int i = 0; i < tk.Length; i++) //should be 10
                        line.uniques.Add(tk[i]);
                    tk = raw[1].Split(' ');
                    for(int i = 0; i < tk.Length; i++) // should be 4
                        line.output.Add(tk[i]);
                    digits.Add(new digit(line));
                    line = null;
                }
            }
            //part 1: check each output for occurences of lengths 2, 4, 3, and 7?
            for(int i = 0; i < digits.Count; i++) {
                digits[i].publish(); //needed for part 2
                for(int j = 0; j < 4; j++) {
                    int len = digits[i].output[j].Length;
                    if(len == 2 || len == 3 || len == 4 || len == 7)
                        pt1_count++;
                }
            }
            Console.WriteLine("total count: " + pt1_count);

            //part 2:
            /* further explanation: each string in Uniques corresponds to a possible digit
             *  each string in Output also corresponds to a digit but not necessarily in the same order
             *  strategy: 
             *      length 2 MUST be 1
             *      length 4 MUST be 4
             *      length 3 MUST be 7
             *      length 7 MUST be 8
             *      length 5 can be 2, 3, 5
             *      length 6 can be 0, 6, 9
             *      
             *      going by default mapping-> 0-a 1-b 2-c 3-d 4-e 5-f 6-g
             *      this mapping will be different for each Digit considered
             *      Note that we're not concerned with mapping each segment A->B, just enough so we have all digits known
             */

            for(int i = 0; i < digits.Count; i++) {
                //the gimmes are already assigned per calling Publish in part 1, 1 4 7 8 already known
                string intermediate = "";
                //assign mapping a: use lengths 3 and 2, compare
                digits[i].segment_Map['a'] = find_dif(digits[i].seven,digits[i].one)[0];
                //assign mapping c: find differences between (0 6 9) and 7
                intermediate += find_dif(digits[i].possible_zerosixnine[0],digits[i].possible_zerosixnine[1]);
                intermediate += find_dif(digits[i].possible_zerosixnine[0],digits[i].possible_zerosixnine[2]);
                intermediate += find_dif(digits[i].possible_zerosixnine[1],digits[i].possible_zerosixnine[2]);
                intermediate = strip_dupes(intermediate);
                intermediate = find_common(digits[i].seven,intermediate);
                digits[i].segment_Map['c'] = intermediate[0];
                //assign mapping f: concat a and c, compare to 7
                intermediate = digits[i].segment_Map['c'].ToString() + digits[i].segment_Map['a'].ToString();
                digits[i].segment_Map['f'] = find_dif(intermediate,digits[i].seven)[0];

                //find digit 5: look at 2 3 5 for whatever doesn't have segment map for c in it
                for(int j = 0; j < digits[i].possible_twothreefive.Count; j++) {
                    intermediate = find_common(digits[i].possible_twothreefive[j],digits[i].segment_Map['c'].ToString());
                    if(intermediate == "") { //find_common found no commons, ie digit does not contain segment c
                        digits[i].five = digits[i].possible_twothreefive[j];
                        digits[i].possible_twothreefive.RemoveAt(j);
                        break;
                    }
                }
                //find digit 2: look at 2 3 5 for whatever doesn't have segment map for f in it
                for(int j = 0; j < digits[i].possible_twothreefive.Count; j++) {
                    intermediate = find_common(digits[i].possible_twothreefive[j],digits[i].segment_Map['f'].ToString());
                    if(intermediate == "") { //find_common found no commons, ie digit does not contain segment f
                        digits[i].two = digits[i].possible_twothreefive[j];
                        digits[i].possible_twothreefive.RemoveAt(j);
                        break;
                    }
                }
                //digit 3: whatever the other two didn't find
                digits[i].three = digits[i].possible_twothreefive[0];

                //find digit 6: look at 0 6 9 for whatever doesn't have segment map c
                for(int j = 0; j < digits[i].possible_zerosixnine.Count; j++) {
                    intermediate = find_common(digits[i].possible_zerosixnine[j],digits[i].segment_Map['c'].ToString());
                    if(intermediate == "") {
                        digits[i].six = digits[i].possible_zerosixnine[j];
                        digits[i].possible_zerosixnine.RemoveAt(j);
                        break;
                    }
                }

                //assign mapping g: dif 5 and 7 strip c, dif 4 strip cf
                intermediate = find_dif(digits[i].five,digits[i].seven);
                intermediate = strip_char(intermediate,digits[i].segment_Map['c']);
                intermediate = find_dif(intermediate,digits[i].four);
                intermediate = strip_char(intermediate,digits[i].segment_Map['c']);
                intermediate = strip_char(intermediate,digits[i].segment_Map['f']);
                digits[i].segment_Map['g'] = intermediate[0];

                //assign mapping d: dif 3 and 7 strip g
                intermediate = find_dif(digits[i].three,digits[i].seven);
                intermediate = strip_char(intermediate,digits[i].segment_Map['g']);
                digits[i].segment_Map['d'] = intermediate[0];

                //find digit 0: look at 0 6 for whatever doesn't have segment map d
                for(int j = 0; j < digits[i].possible_zerosixnine.Count; j++) {
                    intermediate = find_common(digits[i].possible_zerosixnine[j],digits[i].segment_Map['d'].ToString());
                    if(intermediate == "") {
                        digits[i].zero = digits[i].possible_zerosixnine[j];
                        digits[i].possible_zerosixnine.RemoveAt(j);
                        break;
                    }
                }
                //all that remains now is digit 9 which is whatever is left in possible_zerosixnine
                digits[i].nine = digits[i].possible_zerosixnine[0];

                //compute output conversion
                digits[i].convert();
                pt2_sum += digits[i].converted_output;
            }
            Console.WriteLine("Digit sum: " + pt2_sum);
        }

        //return a string containing a ^ b
        public static string find_dif(string a, string b) {
            string result = "";
            for(int i = 0; i < a.Length; i++) {
                if(b.IndexOf(a[i]) == -1)
                    result += a[i].ToString();
            }
            for(int i = 0; i < b.Length; i++) {
                if(a.IndexOf(b[i]) == -1)
                    result += b[i].ToString();
            }
            strip_dupes(result); //catch for duplicates because I'm sure that's gonna happen somehow
            return result;
        }
        
        //return a string containing a && b
        public static string find_common(string a, string b) {
            string result = "";
            for(int i = 0; i < a.Length; i++) {
                if(b.IndexOf(a[i]) != -1)
                    result += a[i].ToString();
            }
            for(int i = 0; i < b.Length; i++) {
                if(a.IndexOf(b[i]) != -1)
                    result += b[i].ToString();
            }
            result = strip_dupes(result);
            return result;
        }

        //remove duplicate values from a string
        public static string strip_dupes(string input) {
            string result = "";
            for(int i = 0; i < input.Length; i++) {
                if(result.IndexOf(input[i]) != -1) //already exists in string
                    continue;
                else
                    result += input[i];
            }
            return result;
        }

        public static string strip_char(string input, char toRemove) {
            string result;
            List<char> temp = new List<char>(); //hacky workaround because string.remove is too finnicky
            temp = input.ToList();
            int ind = temp.IndexOf(toRemove);
            temp.RemoveAt(ind);
            result = new string(temp.ToArray());
            return result;
        }

        public class digit {
            public digit() {
                uniques = new List<string>();
                output = new List<string>();
                possible_twothreefive = new List<string>();
                possible_zerosixnine = new List<string>();
                segment_Map = new Dictionary<char,char>();
                raws = new List<string>();
            }

            public digit(digit other) {
                uniques = other.uniques;
                output = other.output;
                segment_Map = other.segment_Map;
                possible_twothreefive = other.possible_twothreefive;
                possible_zerosixnine = other.possible_zerosixnine;
                raws = other.raws;
            }

            public void publish() { //populate dictionary with 1 4 7 8 values
                for(int i = 0; i < uniques.Count; i++) {
                    switch(uniques[i].Length) {
                        case 2:
                            one = uniques[i];
                            break;
                        case 4:
                            four = uniques[i];
                            break;
                        case 3:
                            seven = uniques[i];
                            break;
                        case 5:
                            possible_twothreefive.Add(uniques[i]);
                            break;
                        case 6:
                            possible_zerosixnine.Add(uniques[i]);
                            break;
                        case 7:
                            eight = uniques[i];
                            break;
                    }
                }
            }
            public void convert() {
                string raw = "";
                make_nice();
                for(int i = 0; i < 4; i++) {
                    for(int j = 0; j < 10; j++) {
                        List<char> str_a = new List<char>();
                        List<char> str_b = new List<char>();
                        List<char> aresult;
                        List<char> bresult;
                        char[] temp = output[i].ToCharArray();
                        str_a = temp.ToList();
                        temp = raws[j].ToCharArray();
                        str_b = temp.ToList();
                        str_a.Sort();
                        str_b.Sort();
                        aresult = str_a.Except(str_b).ToList();
                        bresult = str_b.Except(str_a).ToList();
                        if(aresult.Count != 0 || bresult.Count != 0) {
                            continue; 
                        }
                        else {
                            raw += j.ToString();
                            break;
                        }
                    }
                }
                converted_output = Int32.Parse(raw);
            }

            //compile digits 0-9 into a list for convert() to use more easily
            private void make_nice() {
                raws.Add(zero);
                raws.Add(one);
                raws.Add(two);
                raws.Add(three);
                raws.Add(four);
                raws.Add(five);
                raws.Add(six);
                raws.Add(seven);
                raws.Add(eight);
                raws.Add(nine);
            }


            //good coding practice is get/set methods for each class member but screw that,
            //  I just want this to function as a glorified struct

            public Dictionary<char,char> segment_Map;
            public List<string> uniques;
            public List<string> output;
            public List<string> possible_twothreefive;
            public List<string> possible_zerosixnine;
            public int converted_output;

            private List<string> raws;
            
            public string zero; //Each of these fields will be mapped to one string from Uniques
            public string one;  
            public string two;  //but if that solution doesn't work for some reason I've saved myself some typing
            public string three;
            public string four;
            public string five;
            public string six;
            public string seven;
            public string eight;
            public string nine;
            
        }
    }
}
