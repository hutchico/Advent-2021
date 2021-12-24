using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Day18 {
    class Program {
        static void Main() {
            List<List<char>> pairs = new List<List<char>>();
            using(StreamReader file = new StreamReader("test_input.txt")) {
                while(!file.EndOfStream) {
                    List<char> line = new List<char>();
                    string line_raw = file.ReadLine();
                    for(int i = 0; i < line_raw.Length; i++)
                        line.Add(line_raw[i]);
                    pairs.Add(line);
                }
            }
            
            while(pairs.Count > 1) {
                List<char> left = pairs[0];
                List<char> right = pairs[1];
                reduce(ref left, ref right);
            }


        }

        static void reduce(ref List<char> left, ref List<char> right) {
            List<char> toSend = new List<char>();
            int ret = -1;
            Pair raw;
            toSend.Add('[');
            toSend.AddRange(left);
            toSend.Add(',');
            toSend.AddRange(right);
            toSend.Add(']');
            raw = build_pairs(toSend);
            raw = raw.subpair.Item1;

            simplePrint(raw, 0);
            condense(raw);
            Console.WriteLine("hold");

        }

        static void condense(Pair inp) {
            int[] tree;
            List<(int, int)> literals = new List<(int, int)>();
            int depth = find_depth(inp);
            tree = new int[(int)Math.Pow(2, depth)];
            populate_tree(ref tree, inp, depth);
            (int, int) process = (-1,-1);
            int paircoord = -1;

            for(int i = 0; i < tree.Length; i++) {
                if(tree[i] != -1)
                    literals.Add((i, tree[i]));
            }

            literals.Sort();

            while(true){
                for(int i = 0; i < literals.Count; i++) {
                    if(literals[i].Item1 < ((int)Math.Pow(2, depth) / 2) -1)
                        continue; //ignore anything below depth 6

                    
                }
                //explode left
                for(int i = paircoord; i > ((int)Math.Pow(2, depth) / 2)- 1; i--) {
                    if(tree[i] != -1)
                        tree[i] += process.Item1;
                }
                //explode right
            }
            Console.WriteLine("HOld");

            //explode: any populated nodes between 31 and 63 need to be exploded
            //call until we're 4 calls deep, check if one or the other is lit(false):
            //next call gives us an integer, add integer to said lit side, lit(false) becomes new pair(0)

            //primary check: look for splits to perform at each level


        }
        static void populate_tree(ref int[] tree, Pair inp, int depth) {
            int n = 0;
            for(int k = depth; k > 0; k--){
                for(int i = 0; i < (int)Math.Pow(2,k) / 2; i++) {
                    string bin = Convert.ToString(n, 2);
                    for(int j = bin.Length; j < k - 1; j++) {
                        bin = bin.Insert(0, "0");
                    }
                    int tmp = traverse(inp, bin);
                    int arr = ((int)Math.Pow(2, k) / 2) + Convert.ToInt32(bin,2) - 1;
                    tree[arr] = tmp;
                    n++;
                }
                n = 0;
            }
        }
        
        static int traverse(Pair inp, string path) {
            int debug = inp.value;
            if(path == "")
                return inp.value;
            char dir = path[0];
            string shorter = path.Remove(0, 1);
            if(dir == '0'){
                if(inp.subpair.Item1 == null)
                    return -1;
                return traverse(inp.subpair.Item1, shorter);
            }
            else{
                if(inp.subpair.Item2 == null)
                    return -1;
                return traverse(inp.subpair.Item2, shorter);
            }
        }

        static int find_depth(Pair inp) {
            if(inp == null)
                return 1;
            if(inp.lit)
                return 1;
            int num;
            int num2;
            num = 1 + find_depth(inp.subpair.Item1);
            num2 = 1 + find_depth(inp.subpair.Item2);
            return num > num2 ? num : num2;
        }

        static void simplePrint(Pair inp, int i) {
            if(inp.lit == true) {
                Console.WriteLine("Num: " + inp.value + ". Gen: " + i);
                return;
            }
            simplePrint(inp.subpair.Item1, i + 1); 
        }

        static Pair build_pairs(List<char> input) {
            Stack<char> depth = new Stack<char>();
            Pair toReturn;
            Pair left = null;
            Pair right = null;
            while(input.Count > 0) {
                char c = input[0];
                if(c == '[') {
                    depth.Push(c);
                    input.RemoveAt(0);
                    if(left == null)
                        left = build_pairs(input);
                    else if(right == null) {
                        right = build_pairs(input);
                    }
                }
                else if(c >= 48 && c <= 57) {
                    if(left == null)
                        left = new Pair(c - 48);
                    else
                        right = new Pair(c - 48);
                    input.RemoveAt(0);
                }
                else if(c == ']'){//wrap up what we have and return it
                    input.RemoveAt(0);
                    break;
                }
                else
                    input.RemoveAt(0);

            }
            toReturn = new Pair(left, right);
            return toReturn;
        }
        /*
        static void reduce(ref string input) {
            Stack<char> stk = new Stack<char>();
            Stack<(int, int)> pairs = new Stack<(int, int)>();

            int? valL = null;
            int? valR = null;
            (int, int) lpair;
            (int, int) mpair;
            (int, int) rpair;

            char cur;
            while(input.Length > 0){
                cur = input[0];
                string middle;
                if(48 <= cur && cur <= 57) {
                    if(input[1] >= 48 && input[1] <= 57){ //check for 2 digit numbers
                        middle = cur.ToString() + input[1];
                        input.Remove(0, 2);
                    }
                    else {
                        middle = cur.ToString();
                        input.Remove(0, 1);
                    }

                    if(valL == null) {
                        valL = Convert.ToInt32(cur.ToString(), 10);
                    }
                    else
                        valR = Convert.ToInt32(cur.ToString(), 10);
                }

                if(cur == '[') {
                    stk.Push(cur); 
                }
                if(stk.Count >= 5) { //the next pair we read needs to explode

                }

            }
        }
        */
    }

    class Pair {
        public Pair() { //This "pair" contains at least one subpair
            lit = false;
            value = -1;
        }
        public Pair(int num) { //This "pair" is actually just a glorified Int
            value = num;
            lit = true;
        }
        public Pair(Pair left, Pair right) {
            lit = false;
            subpair = (left, right);
            value = -1;
        }


        /*
        public static bool operator==(Pair a,Pair b){
            if(a == null && b == null)
                return true;
            if(a.lit == b.lit && a.value == b.value && a.subpair.Item1 == b.subpair.Item1 && a.subpair.Item2 == b.subpair.Item2)
                return true;
            return false;
        }
        public static bool operator !=(Pair a, Pair b) {
            if(a != null || b != null)
                return true;
            if(a.lit != b.lit || a.value != b.value || a.subpair.Item1 != b.subpair.Item1 || a.subpair.Item2 != b.subpair.Item2)
                return true;
            return false;
        }
        */
        public void clear(){ subpair.Item1 = null; subpair.Item2 = null; }
        public void set_left(Pair pair) {
            subpair = (pair, subpair.Item2);
        }
        public void set_right(Pair pair) {
            subpair = (subpair.Item1, pair);
        }
        public void set_lit(bool lit) { this.lit = lit; }
        public void set_val(int val){ value = val; }
        public void set_prev(Pair prev){ this.prev = prev; }

        public bool test_null() {
            if(subpair.Item1 == null || subpair.Item2 == null)
                return true;
            return false;
        }

        public int value;
        public bool lit;
        public Pair prev;
        public (Pair,Pair) subpair;
    }

    public enum Dex{
        left,
        right
    }
}
