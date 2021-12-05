using System;
using System.Collections.Generic;
using System.IO;

namespace Day05 {
    class Program {
        static void Main() {
            List<line> lines = new List<line>();
            List<line> part1 = new List<line>();
            int[,] map = new int[1000,1000];
            int overlaps = 0;

            using(StreamReader file = new StreamReader("input.txt")) {
            //using(StreamReader file = new StreamReader("test_input.txt")) {
                string[] raw;
                string[] raw2;
                int X1, Y1, X2, Y2;
                while(!file.EndOfStream) {
                    raw = file.ReadLine().Split(" -> ");
                    raw2 = raw[0].Split(',');
                    X1 = Int32.Parse(raw2[0]);
                    Y1 = Int32.Parse(raw2[1]);
                    raw2 = raw[1].Split(',');
                    X2 = Int32.Parse(raw2[0]);
                    Y2 = Int32.Parse(raw2[1]);
                    lines.Add(new line(X1,Y1,X2,Y2));
                    if(is_hv(X1,Y1,Int32.Parse(raw2[0]),Int32.Parse(raw2[1]))) {
                        part1.Add(new line(X1,Y1,X2,Y2));
                    }
                }
            }

            //part 1
            //draw every line on the map one at a time
            //value of 0 implies nothing is there, 1 means one line, 2+ means at least one line collision
            for(int i = 0; i < lines.Count; i++) {
                if(lines[i].line_type() == orientation.Horizontal) {
                    int yval = lines[i].get_y1();
                    for(int j = lines[i].get_x1(); j < lines[i].get_x2() + 1; j++) { //GUARANTEE: x1 < x2
                        map[yval,j]++;
                    }
                }
                else if(lines[i].line_type() == orientation.Vertical){ //line_type() returns Vertical
                    int xval = lines[i].get_x1();
                    for(int j = lines[i].get_y1(); j < lines[i].get_y2() + 1; j++) { //GUARANTEE: y1 < y2
                        map[j,xval]++;
                    }
                }
                
                else { //part 2
                    int X1, X2, Y1, Y2;
                    int offset = 0;
                    X1 = lines[i].get_x1();
                    X2 = lines[i].get_x2();
                    Y1 = lines[i].get_y1();
                    Y2 = lines[i].get_y2();
                    //2 possible cases when plotting diagonal lines:
                    //y1 > y2 (positive slope)
                    //y1 < y2 (negative slope)
                    if(Y1 < Y2) {
                        for(int j = X1; j <= X2; j++) {
                            map[Y1 + offset,j]++;
                            offset++;
                        }
                    }
                    else {
                        for(int j = X1; j <= X2; j++) {
                            map[Y1 - offset,j]++;
                            offset++;
                        }
                
                    }
                }
                
            }

            //now look over the entire map and count instances of <val> greater than 1
            for(int i = 0; i < map.GetLength(0); i++) { //assuming this returns x dimension length?
                for(int j = 0; j < map.GetLength(1); j++) { //y dimension?
                    if(map[i,j] > 1)
                        overlaps++;
                }
            }
            Console.WriteLine("Line overlaps found: " + overlaps);
            //print_map(map); DEBUG: ONLY FOR SMALL (<50) ARRAYS
        }

        public static bool is_hv(int x1, int y1, int x2, int y2) {
            if(x1 == x2 || y1 == y2)
                return true;
            return false;
        }

        public static void print_map(int[,] map) {
            for(int i = 0; i < map.GetLength(0); i++) { //fun fact, map[x,y] is actually map[y,x].
                for(int j = 0; j < map.GetLength(1); j++) { //x dimension!@
                    Console.Write(map[i,j]);
                }
                Console.Write('\n');
            }
        }


    }

    class line {
        public line(int ix1,int iy1,int ix2,int iy2) {
            //set xy1/xy2 based on least euclidean distance from 0,0
            //this is extraneous because slope is always 0, 1, or undef.
            /*
            double p1, p2;
            p1 = Math.Sqrt(Math.Pow(ix1,2) + Math.Pow(iy1,2));
            p2 = Math.Sqrt(Math.Pow(ix2,2) + Math.Pow(iy2,2));
            
            if(p1 < p2) {
                x1 = ix1; x2 = ix2; y1 = iy1; y2 = iy2;
            }
            else {
                x1 = ix2; x2 = ix1; y1 = iy2; y2 = iy1;
            }
            */
            if(ix1 < ix2) {
                x1 = ix1; x2 = ix2; y1 = iy1; y2 = iy2;
            }
            else if(ix1 == ix2) {
                if(iy1 < iy2) {
                    x1 = ix1; x2 = ix2; y1 = iy1; y2 = iy2;
                }
                else {
                    x1 = ix2; x2 = ix1; y1 = iy2; y2 = iy1;
                }
            }
            else {
                x1 = ix2; x2 = ix1; y1 = iy2; y2 = iy1;
            }
        }

        public bool isStraight() { //Test if the line is either vertical or horizontal
            if(x1 == x2 || y1 == y2)
                return true;
            return false;
        }

        public orientation line_type() {
            if(y1 == y2)
                return orientation.Horizontal;
            if(x1 == x2)
                return orientation.Vertical;
            return orientation.Neither;
        }

        public int test_overlap(line input) { //test how many times this line and another supplied line overlap
            //TODO: put this together in the future maybe
            return 0;
        }

        public int get_x1() { return x1; }
        public int get_x2() { return x2; }
        public int get_y1() { return y1; }
        public int get_y2() { return y2; }

        private int x1, x2;
        private int y1, y2;
    }

    public enum orientation {
        Horizontal,
        Vertical,
        Neither
    }
}
