using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16 {
    class Program {
        static void Main() {
            List<char> raw = new List<char>();
            List<char> bin = new List<char>();
            Packet top = new Packet();
            Int64 Sum = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                while(!file.EndOfStream)
                    raw.Add((char)file.Read());
            }
            if(raw[raw.Count - 1] == '\n')
                raw.RemoveAt(raw.Count - 1); //If for some reason there's a trailing character(s) in the input file, remove that.

            //this was initially a single outer while loop before I recognized it was a single outer packet and x inner packets
            
            subpacket_parse(ref raw, ref bin, ref top, raw.Count * 4);
            top = top.subpackets[0]; //Correct an oddity in the way subpackets are assigned at the top level
            
            //part 1: find all version numbers and print them
            
            Sum += sum_versions(top);
            Console.WriteLine("Version totals: " + Sum);

            //part 2: evaluate every packet
            Sum = 0;
            Sum += evaluate(top);
            Console.WriteLine("Evaluation: " + Sum);

        }
        #region PARSE
        //TODO: find a way to condense both of these into one universal function
        static int subpacket_parse(ref List<char> raw, ref List<char> bin, ref Packet top, int left) {
            if(left < 11) //base
                return 0;
            //get version number
            string current;
            string temp = "";
            int bits_used;
            int binRep;
            Packet tmp = new Packet();
            tmp.version = build_string(ref raw, ref bin, 3);
            tmp.typeID = build_string(ref raw, ref bin, 3);
            bits_used = 6;
            binRep = Convert.ToInt32(tmp.typeID, 2);
            if(binRep == 4) { //literal value
                while(true) {
                    current = build_string(ref raw, ref bin, 5);
                    bits_used += 5;
                    temp += current.Substring(1, 4);
                    if(current[0] == '1') {//more to do
                        continue;
                    }
                    else
                        break;
                }
                tmp.literal = temp;
            }
            else { //operator value
                char type = build_string(ref raw, ref bin, 1)[0];
                bits_used += 1;
                if(type == '0') { //15 bits describing length of subpacket(s)
                    int len = Convert.ToInt32(build_string(ref raw, ref bin, 15), 2);
                    bits_used += 15;
                    bits_used += subpacket_parse(ref raw, ref bin, ref tmp, len);
                }
                else { //11 bits describing number of subpackets
                    int len = Convert.ToInt32(build_string(ref raw, ref bin, 11), 2);
                    bits_used += 11;
                    for(int i = 0; i < len; i++) {
                        bits_used += subpacket_parse_const(ref raw, ref bin, ref tmp);
                    }
                }
            }
            top.subpackets.Add(tmp);
            bits_used += subpacket_parse(ref raw, ref bin, ref top, left - bits_used);
            return bits_used;
        }

        static int subpacket_parse_const(ref List<char> raw, ref List<char> bin, ref Packet top) {
            //get version number
            string current;
            string temp = "";
            int bits_used;
            int binRep;
            Packet tmp = new Packet();
            tmp.version = build_string(ref raw, ref bin, 3);
            tmp.typeID = build_string(ref raw, ref bin, 3);
            bits_used = 6;
            binRep = Convert.ToInt32(tmp.typeID, 2);
            if(binRep == 4) { //literal value
                while(true) {
                    current = build_string(ref raw, ref bin, 5);
                    bits_used += 5;
                    temp += current.Substring(1, 4);
                    if(current[0] == '1') {//more to do
                        continue;
                    }
                    else
                        break;
                }
                tmp.literal = temp;
            }
            else { //operator value
                char type = build_string(ref raw, ref bin, 1)[0];
                bits_used += 1;
                if(type == '0') { //15 bits describing length of subpacket(s)
                    int len = Convert.ToInt32(build_string(ref raw, ref bin, 15), 2);
                    bits_used += 15;
                    bits_used += subpacket_parse(ref raw, ref bin, ref tmp, len);
                }
                else { //11 bits describing number of subpackets
                    int len = Convert.ToInt32(build_string(ref raw, ref bin, 11), 2);
                    bits_used += 11;
                    for(int i = 0; i < len; i++) {
                        bits_used += subpacket_parse_const(ref raw, ref bin, ref tmp);
                    }
                }
            }
            top.subpackets.Add(tmp);
            return bits_used;
        }


        //build binary string from the numbers available in bin
        static string build_string(ref List<char> raw, ref List<char> bin, int num) {
            string toReturn = "";
            while(bin.Count < num)
                next(ref raw, ref bin);
            for(int i = 0; i < num; i++) {
                toReturn += bin[0];
                bin.RemoveAt(0);
            }
            return toReturn;
        }
        //build function needs more characters to process
        static void next(ref List<char> raw, ref List<char> bin) {
            string get = hexCharacters[raw[0]];
            raw.RemoveAt(0);
            for(int i = 0; i < get.Length; i++)
                bin.Add(get[i]);
        }
        #endregion

        static int sum_versions(Packet pack) {
            int sum = 0;
            sum += Convert.ToInt32(pack.version, 2);
            for(int i = 0; i < pack.subpackets.Count; i++)
                sum += sum_versions(pack.subpackets[i]);
            return sum;
        }

        static Int64 evaluate(Packet pack) {
            string id = pack.typeID;
            Int64 num = id == "001" ? 1 : 0;
            List<Int64> work = new List<Int64>();
            for(int i = 0; i < pack.subpackets.Count; i++) {
                switch(pack.typeID) {
                    case "000":
                        num += evaluate(pack.subpackets[i]);
                        break;
                    case "001":
                        num *= evaluate(pack.subpackets[i]);
                        break;
                    default:
                        work.Add(evaluate(pack.subpackets[i]));
                        break;
                }
            }

            switch(id) {
                case "010":
                    return work.Min();
                case "011":
                    return work.Max();
                case "100":
                    return Convert.ToInt64(pack.literal, 2);
                case "101":
                    return work[0] > work[1] ? 1 : 0;
                case "110":
                    return work[0] < work[1] ? 1 : 0;
                case "111":
                    return work[0] == work[1] ? 1 : 0;
                default:
                    break;
            }
            return num;
        }

        #region HEX MAP
        private static readonly Dictionary<char, string> hexCharacters = new Dictionary<char, string>() {
                { '0', "0000" },
                { '1', "0001" },
                { '2', "0010" },
                { '3', "0011" },
                { '4', "0100" },
                { '5', "0101" },
                { '6', "0110" },
                { '7', "0111" },
                { '8', "1000" },
                { '9', "1001" },
                { 'A', "1010" },
                { 'B', "1011" },
                { 'C', "1100" },
                { 'D', "1101" },
                { 'E', "1110" },
                { 'F', "1111" } };
        #endregion
    }

    class Packet {
        public Packet() {
            subpackets = new List<Packet>();
        }

        public string version;
        public string typeID;
        public string literal;
        public List<Packet> subpackets;
    }
}
