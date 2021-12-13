using System;
using System.Collections.Generic;
using System.IO;

namespace Day13 {
    class Program {
        static void Main() {
            Paper paper;
            int initX, initY;
            List<KeyValuePair<int,int>> points = new List<KeyValuePair<int,int>>();
            List<KeyValuePair<char,int>> instructions = new List<KeyValuePair<char,int>>();
            using(StreamReader file = new StreamReader("input.txt")) {
                string input;
                string[] tk;
                while(true) {
                    input = file.ReadLine();
                    if(input == "")
                        break;
                    tk = input.Split(',');
                    points.Add(new KeyValuePair<int,int>(Int32.Parse(tk[0]),Int32.Parse(tk[1])));
                }
                while(!file.EndOfStream) {
                    input = file.ReadLine();
                    tk = input.Split(' '); //fold along x=123
                    tk = tk[2].Split('='); //x=123
                    instructions.Add(new KeyValuePair<char,int>(tk[0][0],Int32.Parse(tk[1])));
                }
            }
            points.Sort((x,y) => x.Key.CompareTo(y.Key)); //Sort by x value, ascending
            initX = points[points.Count - 1].Key + 1;
            points.Sort((x,y) => x.Value.CompareTo(y.Value)); //Sort by y value, ascending
            initY = points[points.Count - 1].Value + 1;

            paper = new Paper(initX,initY);

            for(int i = 0; i < points.Count; i++) {
                paper.plot(points[i].Key,points[i].Value);
            }

            //paper.print_paper(); //DEBUG: only feasible for small paper size

            if(instructions[0].Key == 'x') //
                paper.fold_horiz(instructions[0].Value);
            else
                paper.fold_vert(instructions[0].Value);

            Console.WriteLine("Total dots after one fold: " + paper.count_dots());

            //paper.print_paper(); 

            for(int i = 1; i < instructions.Count; i++) {
                if(instructions[i].Key == 'x')
                    paper.fold_horiz(instructions[i].Value);
                else
                    paper.fold_vert(instructions[i].Value);
                //
            }
            paper.print_paper();
        }
    }

    class Paper {
        public Paper(int x,int y) {
            columns = x;
            rows = y;
            paper = new int[columns,rows];
        }

        public void fold_vert(int x) { //vertical fold
            int[,] temp;
            //fold over line s.t. X = x
            //new Ydim:
            if(x < (rows / 2)) {
                rows -= x;
                temp = new int[columns,rows];
                //Note that some screwy adjustments will be needed because the coordinate system may change
                for(int i = 0; i < columns; i++) { //manually copy everything below the line to the new array
                    for(int j = x; j < paper.GetLength(1); j++) {
                        temp[i,j - x] = paper[i,j];
                    }
                }

                for(int i = 0; i < columns; i++) { //copy everything above the line
                    for(int j = 0; j < x; j++) {
                        if(paper[i,j] == 1) {
                            temp[i,x - j] = 1;
                        }
                    }
                }
            }
            else {
                rows = x;
                temp = new int[columns,rows];

                for(int i = 0; i < columns; i++) {
                    for(int j = 0; j < rows; j++) {
                        temp[i,j] = paper[i,j];
                    }
                }

                for(int i = 0; i < columns; i++) {
                    for(int j = x; j < paper.GetLength(1); j++) {
                        if(paper[i,j] == 1) {
                            temp[i,j - (2 * (j - rows))] = 1;
                        }
                    }
                }
            }
            paper = temp;

        }

        public void fold_horiz(int y) { //Horizontal fold
            int[,] temp;
            if(y < (columns / 2)) {
                columns -= y;
                temp = new int[columns,rows];

                for(int i = y; i < paper.GetLength(0); i++) {
                    for(int j = 0; j < rows; j++) {
                        temp[i - y,j] = paper[i,j];
                    }
                }

                for(int i = 0; i < y; i++) {
                    for(int j = 0; j < rows; j++) {
                        if(paper[i,j] == 1) {
                            temp[y - i,j] = 1;
                        }
                    }
                }

            }
            else {
                columns = y;
                temp = new int[columns,rows];

                for(int i = 0; i < columns; i++) {
                    for(int j = 0; j < rows; j++) {
                        temp[i,j] = paper[i,j];
                    }
                }

                for(int i = y; i < paper.GetLength(0); i++) {
                    for(int j = 0; j < rows; j++) {
                        if(paper[i,j] == 1) {
                            temp[y - (i - y),j] = 1;
                        }
                    }
                }

            }
            paper = temp;
        }

        public void plot(int x,int y) { //plot a point somewhere on the Paper
            paper[x,y] = 1;
        }

        public int count_dots() { //return count of all filled spots
            int tally = 0;
            for(int i = 0; i < columns; i++) {
                for(int j = 0; j < rows; j++) {
                    if(paper[i,j] == 1)
                        tally++;
                }
            }
            return tally;
        }

        public void print_paper() {
            for(int i = 0; i < rows; i++) {
                for(int j = 0; j < columns; j++) {
                    if(paper[j,i] == 1)
                        Console.Write('@'); //print in a way that's more easily readable
                    else
                        Console.Write('.');
                }
                Console.Write('\n');
            }
            Console.Write('\n');
        }

        private int columns;
        private int rows;
        private int[,] paper;
    }
}
