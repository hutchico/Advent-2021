using System;
using System.Collections.Generic;
using System.IO;

namespace Day09 {
    class Program {
        static void Main() {
            int[,] map = new int[100,100];
            int[,] checke = new int[100,100]; //fun fact, "checked" is a namespace collision
            int risk_tally = 0;
            List<(int, int)> lowpoints = new List<(int, int)>(); //needed for part 2

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
                for(int y = 0; y < map.GetLength(1); y++) {
                    int lower_neighbor = 0;
                    int cur_depth = map[x,y];
                    if(x != 0 && map[x - 1,y] <= cur_depth) 
                        lower_neighbor++;
                    if(x != map.GetLength(0) - 1 && map[x + 1,y] <= cur_depth) 
                        lower_neighbor++;
                    if(y != 0 && map[x,y - 1] <= cur_depth) 
                        lower_neighbor++;
                    if(y != map.GetLength(1) - 1 && map[x,y + 1] <= cur_depth) 
                        lower_neighbor++;

                    if(lower_neighbor == 0) //nothing around this is lower/equal to it so it must be low
                        checke[x,y] = 2;
                    else
                        checke[x,y] = 1;
                }
            }

            //now go through the map and tally all depth spots where checke == 2
            for(int i = 0; i < checke.GetLength(0); i++) {
                for(int j = 0; j < checke.GetLength(1); j++) {
                    if(checke[i,j] == 2) {
                        risk_tally += 1 + map[i,j];
                        lowpoints.Add((i, j));
                    }
                }
            }
            Console.WriteLine("Risk levels: " + risk_tally);

            //part 2: find basins
            //strategy: start at each low point, count outward (recursive)
            for(int i = 0; i < lowpoints.Count; i++) {
                int basin = find_basin_size(map,ref checke,lowpoints[i].Item1,lowpoints[i].Item2);
                basins.Add(basin);
            }
            basins.Sort(); //sort ascending
            Console.WriteLine("3 basin product: " + basins[basins.Count - 3] * basins[basins.Count - 2] * basins[basins.Count - 1]);

        }

        static int find_basin_size(int[,] map,ref int[,] checke,int start_x,int start_y) {
            if(checke[start_x,start_y] > 2)  //base case, in part of basin already mapped by another branch
                return 0;
           if(map[start_x,start_y] == 9)  //base case, 9 serves as barriers between basins
                return 0;
            
            checke[start_x,start_y] = 3;

            int minusX = start_x != 0 ? find_basin_size(map, ref checke, start_x - 1, start_y) : 0;
            int minusY = start_y != 0 ? find_basin_size(map, ref checke, start_x, start_y - 1) : 0;
            int plusX = start_x != map.GetLength(0) - 1 ? find_basin_size(map, ref checke, start_x + 1, start_y) : 0;
            int plusY = start_y != map.GetLength(1) - 1 ? find_basin_size(map, ref checke, start_x, start_y + 1) : 0;

            return 1 + minusX + minusY + plusX + plusY;
        }
    }
}
