﻿using System;
using System.IO;
using System.Collections.Generic;

namespace Day02 {
    class Program {
        static void Main() {
            List<string[]> inputs = new List<string[]>();
            int depth1 = 0;
            int depth2 = 0;
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
                        depth2 += aim * Int32.Parse(inputs[i][1]);
                        break;
                    case "up":
                        depth1 -= Int32.Parse(inputs[i][1]);
                        aim -= Int32.Parse(inputs[i][1]);
                        break;
                    case "down":
                        depth1 += Int32.Parse(inputs[i][1]);
                        aim += Int32.Parse(inputs[i][1]);
                        break;
                }
            }
            Console.WriteLine("Horizontal position: " + horiz_pos + ", depth1 and 2: " + depth1 + " " + depth2);
            Console.WriteLine("Product part 1: " + (horiz_pos * depth1));
            Console.WriteLine("Product part 2: " + (horiz_pos * depth2));
        }
    }
}
