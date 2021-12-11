using System;
using System.IO;

namespace Day11 {
    class Program {
        static void Main() {
            int[,] jelly = new int[10,10];
            int[,] flashed = new int[jelly.GetLength(0),jelly.GetLength(1)];
            int generations = 100;
            flashCounter.flashes = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                for(int x = 0; x < jelly.GetLength(0); x++) {
                    for(int y = 0; y < jelly.GetLength(1); y++) {
                        int entry = file.Read();
                        while(entry == 13 || entry == 10) //read and discard trailing characters
                            entry = file.Read();
                        jelly[x,y] = entry - 48; //ascii garbage
                    }
                }
            }

            for(int i = 0; i < 100; i++) {
                build(ref jelly,ref flashed);
            }

            Console.WriteLine("Total flashes: " + flashCounter.flashes);

            while(true) {
                build(ref jelly,ref flashed);
                generations++;
                if(test_for_zero(jelly)) {
                    Console.WriteLine("0 on step: " + generations);
                    break;
                }
            }
        }

        static void build(ref int[,] map,ref int[,] flashed) {
            for(int x = 0; x < map.GetLength(0); x++) {
                for(int y = 0; y < map.GetLength(1); y++) {
                    if(map[x,y] + 1 > 9) { //if incrementing would put an octopus above 9, call flash()
                        flash(ref map,ref flashed,x,y);
                    }
                    else {
                        if(flashed[x,y] != 1)
                            map[x,y]++;         //otherwise, just inc it and move on assuming it's not zero
                    }
                }
            }
            flashed = new int[map.GetLength(0),map.GetLength(1)]; //reset this, it will populate with 0s
        }

        static void flash(ref int[,] map,ref int[,] flashed,int x,int y) { //the octopus at (x,y) flashes and applies light to 8(max) adjacents
            if(flashed[x,y] != 1)//this octopus has already been flashed once
                map[x,y]++;
            else
                return;
            if(map[x,y] > 9) {
                map[x,y] = 0;
                flashed[x,y] = 1;
                flashCounter.flashes++;

                if(x != 0) {
                    flash(ref map,ref flashed,x - 1,y);
                    if(y != 0) {
                        flash(ref map,ref flashed,x - 1,y - 1);
                    }
                    if(y != map.GetLength(1) - 1) {
                        flash(ref map,ref flashed,x - 1,y + 1);
                    }
                }
                if(x != map.GetLength(0) - 1) {
                    flash(ref map,ref flashed,x + 1,y);
                    if(y != map.GetLength(1) - 1) {
                        flash(ref map,ref flashed,x + 1,y + 1);
                    }
                }
                if(y != 0) {
                    flash(ref map,ref flashed,x,y - 1);
                    if(x != map.GetLength(0) - 1) {
                        flash(ref map,ref flashed,x + 1,y - 1);
                    }
                }
                if(y != map.GetLength(1) - 1) {
                    flash(ref map,ref flashed,x,y + 1);
                }
            }
        }

        static bool test_for_zero(int[,] map) { //am I missing something here?
            for(int x = 0; x < map.GetLength(0); x++) { //nope, it's literally that simple
                for(int y = 0; y < map.GetLength(1); y++) {
                    if(map[x,y] == 0)
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }

    }
    static class flashCounter {
        public static int flashes; //global variable because passing a single reference is cumbersome
    }
}
