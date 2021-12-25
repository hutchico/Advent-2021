using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day25 {
    class Program {
        static void Main() {
            char[,] board;
            List<List<char>> herd_raw = new List<List<char>>();
            using(StreamReader file = new StreamReader("input.txt")){
                while(!file.EndOfStream){
                    List<char> line = new List<char>(file.ReadLine());
                    herd_raw.Add(line);
                }
            }

            board = new char[herd_raw.Count,herd_raw[0].Count];
            for(int i = 0; i < board.GetLength(1); i++) {
                for(int j = 0; j < board.GetLength(0); j++) {
                    board[j, i] = herd_raw[j][i];
                }
            }
            //print(board);
            Console.WriteLine("hold");
            int steps = 0;
            while(true) { //step
                //print(board);
                int moved = 0;
                steps++;
                moved += move_east(ref board);
                moved += move_south(ref board);
                if(moved == 0)
                    break;
            }
            Console.WriteLine("First step with no movement: " + steps);
        }

        static int move_east(ref char[,] board){
            bool moved = false;
            bool occupied;
            for(int i = 0; i < board.GetLength(0); i++) { //y direction
                occupied = board[i, 0] == '.' ? false : true; //measure whether the space is occupied at start of inner loop
                for(int j = 0; j < board.GetLength(1); j++) { //x direction
                    int mod = (j + 1) % board.GetLength(1);
                    if(board[i,j] == '>' && board[i, mod] == '.'){
                        if(j == board.GetLength(1) - 1 && occupied)
                            continue;
                        board[i, mod] = board[i, j];
                        board[i, j] = '.';
                        moved = true;
                        j++; //move forward one extra so we're not counting this one twice
                    }
                }
            }
            return moved ? 1 : 0;
        }
        static int move_south(ref char[,] board) {
            bool moved = false;
            bool occupied;
            for(int i = 0; i < board.GetLength(1); i++) { //x direction
                occupied = board[0, i] == '.' ? false : true;
                for(int j = 0; j < board.GetLength(0); j++) { //y direction
                    if(board[j, i] == 'v' && board[(j + 1) % board.GetLength(0),i] == '.') {
                        if(j == board.GetLength(0) - 1 && occupied)
                            continue;
                        board[(j + 1) % board.GetLength(0), i] = board[j, i];
                        board[j, i] = '.';
                        moved = true;
                        j++;
                    }
                }
            }
            return moved ? 1 : 0;
        }

        static void print(char[,] board) {
            for(int i = 0; i < board.GetLength(0); i++) {
                for(int j = 0; j < board.GetLength(1); j++) {
                    Console.Write(board[i, j]);
                }
                Console.Write('\n');
            }
            Console.Write('\n');
        }
    }
}
