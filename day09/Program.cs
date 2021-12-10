using System;
using System.Collections.Generic;
using System.IO;

namespace Day09 {
    class Program {
        static void Main() {
            int[,] map = new int[100,100];
            int[,] checke = new int[100,100]; //fun fact, "checked" is a namespace collision
            int risk_tally = 0;
            List<KeyValuePair<int,int>> lowpoints = new List<KeyValuePair<int,int>>(); //needed for part 2

            List<int> basins = new List<int>();

            using(StreamReader file = new StreamReader("input.txt")) {
                for(int i = 0; i < map.GetLength(0); i++) {
                    for(int j = 0; j < map.GetLength(1); j++) {
                        int entry = file.Read();
                        while(entry == 13 || entry == 10) //read and discard trailing characters
                            entry = file.Read();
                        map[i,j] = entry - 48; //ascii garbage
                    }
                }
            }

            //part 1

            //original design was a convoluted solution built around rolling a marble down a hill.
            // that was dumb, so bruteforce it is.
            for(int x = 0; x < map.GetLength(0); x++) {
                int minusX;
                int minusY;
                int plusX;
                int plusY;

                for(int y = 0; y < map.GetLength(1); y++) {
                    minusX = minusY = plusX = plusY = 0;
                    int cur_depth = map[x,y];
                    if(x != 0 && map[x - 1,y] <= cur_depth) {
                        minusX = 1;
                    }
                    if(x != map.GetLength(0) - 1 && map[x + 1,y] <= cur_depth) {
                        plusX = 1;
                    }
                    if(y != 0 && map[x,y - 1] <= cur_depth) {
                        minusY = 1;
                    }
                    if(y != map.GetLength(1) - 1 && map[x,y + 1] <= cur_depth) {
                        plusY = 1;
                    }
                    if(minusX + minusY + plusX + plusY == 0) //nothing around this is lower/equal to it so it must be low
                        checke[x,y] = 2;
                    else
                        checke[x,y] = 1;
                }
            }

            //ball is done rolling, now go through the map and tally all depth spots where checke == 2
            for(int i = 0; i < checke.GetLength(0); i++) {
                for(int j = 0; j < checke.GetLength(1); j++) {
                    if(checke[i,j] == 2) {
                        risk_tally += 1 + map[i,j];
                        lowpoints.Add(new KeyValuePair<int,int>(i,j));
                    }
                }
            }
            Console.WriteLine("Risk levels: " + risk_tally);

            //part 2: find basins
            //strategy: start at each low point, count outward (recursive)
            for(int i = 0; i < lowpoints.Count; i++) {
                int basin = find_basins(map,ref checke,lowpoints[i].Key,lowpoints[i].Value);
                basins.Add(basin);
            }
            basins.Sort();
            Console.WriteLine("3 basin product: " + basins[basins.Count - 3] * basins[basins.Count - 2] * basins[basins.Count - 1]);

        }

        static int find_basins(int[,] map,ref int[,] checke,int start_x,int start_y) {
            if(checke[start_x,start_y] > 2) { //base case, in part of basin already mapped by another branch
                return 0;
            }
            else if(map[start_x,start_y] == 9) { //base case, 9 counts as barriers between basins
                return 0;
            }
            checke[start_x,start_y] = 3;

            int minusX = 0;
            int minusY = 0;
            int plusX = 0;
            int plusY = 0;

            if(start_x != 0) {
                minusX = find_basins(map,ref checke,start_x - 1,start_y);
            }
            if(start_y != 0) {
                minusY = find_basins(map,ref checke,start_x,start_y - 1);
            }
            if(start_x != map.GetLength(0) - 1) {
                plusX = find_basins(map,ref checke,start_x + 1,start_y);
            }
            if(start_y != map.GetLength(1) - 1) {
                plusY = find_basins(map,ref checke,start_x,start_y + 1);
            }

            return 1 + minusX + minusY + plusX + plusY;
        }
    }
}
