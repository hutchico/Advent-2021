using System;
using System.Collections.Generic;
using System.IO;

namespace Day04 {
    class Program {
        static void Main() {
            List<board> boards = new List<board>();
            List<int> column = new List<int>();
            List<List<int>> row = new List<List<int>>();
            string[] numbers; //yeah yeah whatever we'll need to convert them at some point might as well do it on demand
            using(StreamReader file = new StreamReader("input.txt")) {
                string[] line;
                numbers = (file.ReadLine()).Split(',');
                while(!file.EndOfStream) {
                    file.ReadLine(); //ignore buffer lines
                    for(int i = 0; i < 5; i++) {
                        line = file.ReadLine().Split(' ');
                        for(int j = 0; j < line.Length; j++) {
                            if(line[j] == "") //catch single digit space separators because I don't know how it will handle those
                                continue;
                            column.Add(Int32.Parse(line[j]));
                        }
                        row.Add(new List<int>(column));
                        column.Clear();
                    }
                    boards.Add(new board(row));
                    row.Clear();
                }
            }

            //now to run the sequence of numbers from the first line of the input file
            //part 1 + part 2
            bool part2_switch = false;
            bool end = false;
            for(int i = 0; i < numbers.Length; i++) {
                for(int j = 0; j < boards.Count; j++) {
                    if(boards[j].mark_num(Int32.Parse(numbers[i]))) { //will return true if a board wins
                        if(part2_switch == false) {
                            Console.WriteLine("First winning board score: " + boards[j].sum_board() * Int32.Parse(numbers[i]));
                            part2_switch = true; //ignore winning boards after this point, we only care about the last one in the list
                        }
                        if(boards.Count == 1) {//only one board left
                            Console.WriteLine("Final winning board score: " + boards[0].sum_board() * Int32.Parse(numbers[i]));
                            end = true;
                            break;
                            //This is valid because there's 100 numbers in the input sequence, meaning every possible number will be called at some point.
                            //  Guarantees every board will win at some point
                        }
                        boards.RemoveAt(j); //remove the board from the pool since for part 2 purposes we're concerned with last valid board
                        j--;
                    }
                }
                if(end)
                    break;
            }
        }
    }
    class board {
        public board(List<List<int>> state) {
            boardContents = new List<List<int>>(state);
            boardState = new List<List<int>> { new List<int> { 0,0,0,0,0 },new List<int> { 0,0,0,0,0 },new List<int> { 0,0,0,0,0 },new List<int> { 0,0,0,0,0 },new List<int> { 0,0,0,0,0 } };
        }
        public bool mark_num(int num) {
            for(int i = 0; i < 5; i++) {
                int ind = boardContents[i].IndexOf(num);
                if(ind != -1) {
                    boardState[i][ind] = 1;
                    if(checkWin(i,ind))
                        return true;
                }
            }
            return false;
        }

        public int sum_board() {
            int tally = 0;
            for(int i = 0; i < boardContents.Count; i++) {
                for(int j = 0; j < boardContents[0].Count; j++) {
                    if(boardState[i][j] != 1)
                        tally += boardContents[i][j];
                }
            }
            return tally;
        }

        public bool checkWin(int x, int y) { //Scan the board for victory conditions around the number we just marked off
            //4 possible cases: horiz, vert
            // diagUp, diagDown only if x == y
            int HCount = 0;
            int Vcount = 0;
            int DUcount = 0;
            int DDcount = 0;
            for(int i = 0; i < 5; i++) { //across
                HCount += boardState[i][y];
                Vcount += boardState[x][i];
            }
            if(HCount == 5 || Vcount == 5)
                return true;

            if(x != y || x + y != 4) //check if number is in one of the valid diagonals
                return false;

            if(y <= 2) { 
                for(int i = 0; i < 5; i++) {
                    DDcount += boardState[i][i];
                }
                if(DDcount == 5)
                    return true;
            }

            else if(y >= 2) { //diag up, catches y == x == 3
                for(int i = 0; i < 5; i++) {
                    DUcount += boardState[i][4 - i];
                }
                if(DUcount == 5)
                    return true;
            }
            return false;
        }

        private List<List<int>> boardState;
        private List<List<int>> boardContents;
    }

}
