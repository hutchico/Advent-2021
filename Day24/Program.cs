using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day24 {
    class Program {
        static void Main() {
            List<List<string>> prog = new List<List<string>>();
            ALU monad = new ALU();
            using(StreamReader file = new StreamReader("input.txt")) {
                List<string> line = new List<string>();
                while(!file.EndOfStream) {
                    line = file.ReadLine().Split(' ').ToList();
                    while(line.Count < 3) {
                        line.Add(null);
                    }
                    prog.Add(line);
                }
            }

            long max = 99999999999999; //okay so bruteforce isn't gonna do it
            while(true) {
                string to_test = max.ToString();
                for(int i = 0; i < prog.Count; i++) {
                    if(prog[i][0] == "inp"){
                        monad.execute(prog[i][0], prog[i][1], to_test[0].ToString());
                        to_test = to_test.Remove(0, 1);
                    }
                    else {
                        monad.execute(prog[i][0], prog[i][1], prog[i][2]);
                    }
                }
                if(monad.check_valid() == true) {
                    break;
                }
                else{
                    max--;
                    //Console.WriteLine("z: " + monad.getZ());
                    monad.reset();
                }
            }
            Console.WriteLine("Largest valid number found: " + max);
        }
    }

    class ALU {

        public ALU() {
            reset();
        }

        public void execute(string instruction, string regA, string regB) {
            char A = regA[0];

            switch(instruction) {
                case "inp":
                    W = Convert.ToInt32(regB,10); //Per input, there will never be an input operation performed on X, Y, or Z
                    break;
                case "add":
                    add(A, regB);
                    break;
                case "mul":
                    mul(A, regB);
                    break;
                case "div":
                    div(A, regB);
                    break;
                case "mod":
                    mod(A, regB);
                    break;
                case "eql":
                    eql(A, regB);
                    break;
            }
        }

        private void add(char reg1, string reg2) {
            int middle = 0;
            switch(reg2){
                case "w":
                    middle = W;
                    break;
                case "x":
                    middle = X;
                    break;
                case "y":
                    middle = Y;
                    break;
                case "z":
                    middle = Z;
                    break;
                default:
                    middle = Convert.ToInt32(reg2, 10);
                    break;
            }
            switch(reg1) {
                case 'w':
                    W += middle;
                    break;
                case 'x':
                    X += middle;
                    break;
                case 'y':
                    Y += middle;
                    break;
                case 'z':
                    Z += middle;
                    break;
            }
        }
        private void mul(char reg1, string reg2) {
            int middle = 0;
            switch(reg2) {
                case "w":
                    middle = W;
                    break;
                case "x":
                    middle = X;
                    break;
                case "y":
                    middle = Y;
                    break;
                case "z":
                    middle = Z;
                    break;
                default:
                    middle = Convert.ToInt32(reg2, 10);
                    break;
            }
            switch(reg1) {
                case 'w':
                    W *= middle;
                    break;
                case 'x':
                    X *= middle;
                    break;
                case 'y':
                    Y *= middle;
                    break;
                case 'z':
                    Z *= middle;
                    break;
            }
        }
        private void div(char reg1, string reg2) {
            int middle = 0;
            switch(reg2) {
                case "w":
                    middle = W;
                    break;
                case "x":
                    middle = X;
                    break;
                case "y":
                    middle = Y;
                    break;
                case "z":
                    middle = Z;
                    break;
                default:
                    middle = Convert.ToInt32(reg2, 10);
                    break;
            }
            switch(reg1) {
                case 'w':
                    W /= middle;
                    break;
                case 'x':
                    X /= middle;
                    break;
                case 'y':
                    Y /= middle;
                    break;
                case 'z':
                    Z /= middle;
                    break;
            }
        }
        private void mod(char reg1, string reg2) {
            int middle = 0;
            switch(reg2) {
                case "w":
                    middle = W;
                    break;
                case "x":
                    middle = X;
                    break;
                case "y":
                    middle = Y;
                    break;
                case "z":
                    middle = Z;
                    break;
                default:
                    middle = Convert.ToInt32(reg2, 10);
                    break;
            }
            switch(reg1) {
                case 'w':
                    W %= middle;
                    break;
                case 'x':
                    X %= middle;
                    break;
                case 'y':
                    Y %= middle;
                    break;
                case 'z':
                    Z %= middle;
                    break;
            }
        }
        private void eql(char reg1, string reg2) {
            int middle = 0;
            switch(reg2) {
                case "w":
                    middle = W;
                    break;
                case "x":
                    middle = X;
                    break;
                case "y":
                    middle = Y;
                    break;
                case "z":
                    middle = Z;
                    break;
                default:
                    middle = Convert.ToInt32(reg2, 10);
                    break;
            }
            switch(reg1) {
                case 'w':
                    if(W == middle)
                        W = 1;
                    else
                        W = 0;
                    break;
                case 'x':
                    if(X == middle)
                        X = 1;
                    else
                        X = 0;
                    break;
                case 'y':
                    if(Y == middle)
                        Y = 1;
                    else
                        Y = 0;
                    break;
                case 'z':
                    if(Z == middle)
                        Z = 1;
                    else
                        Z = 0;
                    break;
            }
        }
        public bool check_valid() {
            if(Z == 0)
                return true;
            return false;
        }

        public void reset() {
            W = X = Y = Z = 0;
        }

        public int getW() { return W; }
        public int getX() { return X; }
        public int getY() { return Y; }
        public int getZ() { return Z; }

        private int W;
        private int X;
        private int Y;
        private int Z;
    }

    //leaving this commented because the thought process was there at least
    /*
    class Instruction {
        public Instruction(string inst, char? reg, int? val) {
            desc = inst;
            type = reg == null ? 1 : 0;
            type = inst == "inp" ? 2 : type;
            if(reg != null)
                this.reg = (char)reg;
            if(val != null)
                this.val = (int)val;
        }

        public int get_type(){ return type; }
        public char get_reg(){ return reg; }
        public int get_val(){ return val; }
        //TODO: function that returns either reg or val?

        private string desc;
        private int type;
        private int val;
        private char reg;
    }
    */
}
