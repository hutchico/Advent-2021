using System;
using System.Collections.Generic;
using System.IO;

namespace Day17 {
    class Program {
        static void Main() {
            (int, int) yrange;
            (int, int) xrange;

            int max_height = -1;
            int min_x = 0; //sanity check: starting x velocity < 0 will never be positive
            int max_x;
            int min_y;
            int max_y;
            List<(int, int)> valid = new List<(int, int)>();
            using(StreamReader file = new StreamReader("input.txt")) {
                string entry = file.ReadLine();
                string[] tk = entry.Split(',');
                string[] tk2 = tk[0].Split('=');
                tk2 = tk2[1].Split("..");
                xrange = (Int32.Parse(tk2[0]), Int32.Parse(tk2[1]));
                tk2 = tk[1].Split('=');
                tk2 = tk2[1].Split("..");
                yrange = (Int32.Parse(tk2[0]), Int32.Parse(tk2[1]));
            }
            //determine X range:
            max_x = xrange.Item2 + 1; //sanity check: x velocity > max range will overshoot target immediately
            (int, int) x_window = find(min_x, max_x, xrange, 'x');

            //determine Y range:
            min_y = yrange.Item1 - 1; //sanity check: y velocity < max range will overshoot target immediately
            max_y = yrange.Item2 * -1 * 10; //educated guess based on original numbers, can adjust if necessary
            (int, int) y_window = find(min_y, max_y, yrange, 'y');

            //we have a range of values for X and Y which will land it in the window in question
            //now we run each until it's outside that window and see what the max height is
            //also verify that the coords in question actually hit said window
            for(int i = x_window.Item1; i <= x_window.Item2; i++) {
                for(int j = y_window.Item1; j <= y_window.Item2; j++) {
                    if(run(i, j, xrange, yrange)) {
                        valid.Add((i, j));
                    }
                    else //skip finding this height if it's not in the window
                        continue;
                    int ret = find_height(i, j);
                    if(ret > max_height)
                        max_height = ret;
                }
            }
            Console.WriteLine("Max height: " + max_height);
            Console.WriteLine("Total valid coords: " + valid.Count);
        }

        static (int, int) find(int min_val, int max_val, (int, int) range, char mode) {
            int pos = 0;
            int min = 0;
            int max = 0;
            //iterate between min and max
            //for each:
            //if n is starting velocity, step through and if there's ever a point where min < n < max, break
            List<bool> tested = new List<bool>();
            for(int i = min_val; i <= max_val; i++) {
                int start = i;
                while(true) {
                    step(ref pos, ref start, mode);
                    if(pos >= range.Item1 && pos <= range.Item2) {
                        tested.Add(true);
                        break;
                    }
                    if(mode == 'x' && start <= 0) { //different exit conditions for x and y axis
                        tested.Add(false);
                        break;
                    }
                    if(mode == 'y' && pos < range.Item1) {
                        tested.Add(false);
                        break;
                    }
                }
                pos = 0;
            }
            //find lowest and highest occurences of a true evaluation, return those
            for(int i = 0; i < tested.Count; i++) {
                if(tested[i] == true) {
                    min = i;
                    break;
                }
            }
            for(int i = tested.Count - 1; i >= 0; i--) {
                if(tested[i] == true) {
                    max = i;
                    break;
                }
            }
            return (min_val + min, min_val + max); //inc low value by min/max range to get total range of options
        }
        static int find_height(int x, int y) {
            int xpos = 0;
            int ypos = 0;
            int yprev;
            int height = 0;
            while(true) {
                if(ypos > height)
                    height = ypos;
                yprev = ypos;
                full_step(ref xpos, ref ypos, ref x, ref y);
                if(ypos < yprev)
                    break;
            }
            return height;
        }
        static bool run(int x, int y, (int, int) targetX, (int, int) targetY) {
            int xpos = 0;
            int ypos = 0;
            int yvel = y;
            int xvel = x;
            while(true) {
                full_step(ref xpos, ref ypos, ref xvel, ref yvel);
                if(targetX.Item1 <= xpos && xpos <= targetX.Item2 &&
                    targetY.Item1 <= ypos && ypos <= targetY.Item2) {
                    return true;
                }
                if(ypos < targetY.Item1 || xpos > targetX.Item2)
                    return false;
            }
        }
        static void step(ref int pos, ref int vel, char dir) {
            pos += vel;
            if(dir == 'x' && vel <= 0) {
                return;
            }
            else
                vel -= 1;
        }
        static void full_step(ref int xpos, ref int ypos, ref int xvel, ref int yvel) {
            xpos += xvel;
            ypos += yvel;
            if(xvel > 0)
                xvel -= 1;
            yvel -= 1;
        }
    }
}
