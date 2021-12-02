using System;
using System.IO;
using System.Collections.Generic;

namespace Day02 {
    class Program {
        static void Main() {
            List<string[]> inputs = new List<string[]>();
            int depth = 0;
            int horiz_pos = 0;
            int aim = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                string raw;
                string[] tokens;
                while(!file.EndOfStream) {
                    raw = file.ReadLine();
                    tokens = raw.Split(' ');
                    inputs.Add(tokens);
                }
            }
            for(int i = 0; i < inputs.Count; i++) {
                switch(inputs[i][0]) {
                    case "forward":
                        horiz_pos += Int32.Parse(inputs[i][1]);
                        break;
                    case "up":
                        depth -= Int32.Parse(inputs[i][1]);
                        break;
                    case "down":
                        depth += Int32.Parse(inputs[i][1]);
                        break;
                }
            }
            Console.WriteLine("Horizontal position: " + horiz_pos + ", depth: " + depth + '\n');
            Console.WriteLine("Product: " + (horiz_pos * depth));
            //part 2
            depth = 0;
            horiz_pos = 0;
            for(int i = 0; i < inputs.Count; i++) {
                switch(inputs[i][0]) {
                    case "forward":
                        horiz_pos += Int32.Parse(inputs[i][1]);
                        depth += aim * Int32.Parse(inputs[i][1]);
                        break;
                    case "up":
                        aim -= Int32.Parse(inputs[i][1]);
                        break;
                    case "down":
                        aim += Int32.Parse(inputs[i][1]);
                        break;
                }
            }
            Console.WriteLine("Horizontal position: " + horiz_pos + ", depth: " + depth + '\n');
            Console.WriteLine("Product: " + (horiz_pos * depth));
        }
    }
}
