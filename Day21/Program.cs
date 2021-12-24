using System;
using System.IO;
using System.Collections.Generic;

namespace Day21 {
    class Program {
        static void Main() {
            int p1, p2;
            Board board;
            using(StreamReader file = new StreamReader("input.txt")) {
                string[] tk = file.ReadLine().Split(':');
                tk[1] = tk[1].Remove(0, 1);
                p1 = Convert.ToInt32(tk[1], 10);
                tk = file.ReadLine().Split(':');
                tk[1] = tk[1].Remove(0, 1);
                p2 = Convert.ToInt32(tk[1], 10);
            }

            board = new Board(p1, p2);
            bool flip = false;
            while(flip == false) {
                flip = board.turn('1');
                if(flip)
                    break;
                flip = board.turn('2');
            }

            
        }

        static (int,int) quantum(int s1, int s2, int p1, int p2, int turn, int roll) {
            if(memo.ContainsKey(Func<s1,s2,p1,p2,turn,roll>))
                return memo[Func<s1, s2, p1, p2, turn, roll>]

        }

        public static Dictionary<Func<int, int, int, int, int, int>, (int, int)> memo = new Dictionary<Func<int, int, int, int, int, int>, (int, int)>();
        public static Func<s1,s2,p1,p2,turn,roll,f> Memoize<s1, s2, p1, p2, turn, roll, f>(this Func<s1, s2, p1, p2, turn, roll,f>,(int,int) n ) {
            if(memo.ContainsKey)
                return (a,b,c,d,e,g) => memo.TryGetValue(Func<a,b,c,d,e,g>,n);
        }

        
    }

    class Board {
        public Board(int p1, int p2) {
            P1pos = p1;
            P2pos = p2;
            die = 0; //initialize before first roll
            p1score = 0;
            p2score = 0;
            dicerolls = 0;
        }

        public bool turn(char player) {
            int rolled = 0;
            for(int i = 0; i < 3; i++)
                rolled += roll();
            if(player == '1') {
                P1pos += rolled;
                while(P1pos > 10)
                    P1pos -= 10;
                p1score += P1pos;
                return check_win(player);
            }
            else {
                P2pos += rolled;
                while(P2pos > 10)
                    P2pos -= 10;
                p2score += P2pos;
                return check_win(player);
            }
        }

        public bool check_win(char player) {
            if(player == '1' && p1score >= 1000) {
                Console.WriteLine("Win for player 1. final score: " + (p2score * dicerolls));
                return true;
            }
            else if(player == '2' && p2score >= 1000) {
                Console.WriteLine("Win for player 2. final score: " + (p1score * dicerolls));
                return true;
            }
            return false;
        }

        private int roll() {
            die++;
            if(die > 100)
                die = 1;
            dicerolls++;
            return die;
        }

        private int dicerolls;
        private int p1score;
        private int p2score;
        private int die;
        private int P1pos;
        private int P2pos;
    }
}
