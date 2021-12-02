using System;
using System.IO;

namespace Day02 {
    class Program {
        static void Main() {
            int depth = 0;
            int horiz_pos = 0;
            int aim = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                string raw;
                string[] tokens;
                while(!file.EndOfStream) {
                    raw = file.ReadLine();
                    tokens = raw.Split(' ');
                    switch(tokens[0]) {
                        case "forward":
                            horiz_pos += Int32.Parse(tokens[1]);
                            break;
                        case "up":
                            depth -= Int32.Parse(tokens[1]);
                            break;
                        case "down":
                            depth += Int32.Parse(tokens[1]);
                            break;
                    }
                }
            }
            Console.WriteLine("Horizontal position: " + horiz_pos + ", depth: " + depth + '\n');
            Console.WriteLine("Product: " + (horiz_pos * depth));
            //part 2
            depth = 0;
            horiz_pos = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                string raw;
                string[] tokens;
                while(!file.EndOfStream) {
                    raw = file.ReadLine();
                    tokens = raw.Split(' ');
                    switch(tokens[0]) {
                        case "forward":
                            horiz_pos += Int32.Parse(tokens[1]);
                            depth += aim * Int32.Parse(tokens[1]);
                            break;
                        case "up":
                            aim -= Int32.Parse(tokens[1]);
                            break;
                        case "down":
                            aim += Int32.Parse(tokens[1]);
                            break;
                    }
                }
            }
            Console.WriteLine("Horizontal position: " + horiz_pos + ", depth: " + depth + '\n');
            Console.WriteLine("Product: " + (horiz_pos * depth));
        }
    }
}
