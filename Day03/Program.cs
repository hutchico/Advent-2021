using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day03 {
    class Program {
        static void Main() {
            int numOnes = 0;
            int numZero = 0;
            string gamma_raw = "";
            string epsilon_raw = "";
            int gamma;
            int epsilon;
            List<String> inputs = new List<string>();
            using(StreamReader file = new StreamReader("input.txt")) {
                while(!file.EndOfStream) {
                    inputs.Add(file.ReadLine());
                }
            }
            for(int i = 0; i < inputs[0].Length; i++) {
                for(int j = 0; j < inputs.Count; j++) {
                    if(inputs[j][i] == '1') {
                        numOnes++;
                    }
                    else
                        numZero++;
                }
                if(numOnes > numZero) {
                    gamma_raw += '1';
                    epsilon_raw += '0';
                }
                else { 
                    gamma_raw += '0';
                    epsilon_raw += '1';
                }
                numOnes = 0;
                numZero = 0;
            }
            /*
             * So I tried to be super slick here and recognize that epsilon is just the inversion of gamma.
             * Problem is c# doesn't have a quick and easy way to integrate that,
             *     Bitwise inversion (~) covers the entirety of the 32 bit integer space, not just the bits I'm concerned with.
             * Didn't want to mess with bit masking so I just opted to build separate strings.
             */
            gamma = Convert.ToInt32(gamma_raw,2);
            epsilon = Convert.ToInt32(epsilon_raw,2);

            Console.WriteLine("Product: " + gamma * epsilon);

            //Part 2
            List<string> oxygen = new List<string>(inputs);
            List<string> carbon = new List<string>(inputs);
            char target;
            inputs = null; //

            for(int i = 0; i < oxygen[0].Length; i++) {
                if(oxygen.Count == 1) //target value reached
                    break;
                for(int j = 0; j < oxygen.Count; j++) {
                    if(oxygen[j][i] == '1') {
                        numOnes++;
                    }
                    else
                        numZero++;
                }
                if(numOnes >= numZero)
                    target = '1';
                else
                    target = '0';
                for(int j = 0; j < oxygen.Count; j++) {
                    if(oxygen[j][i] != target) {
                        oxygen.RemoveAt(j);
                        j--;
                    }
                }
                numOnes = 0;
                numZero = 0;
            }
            
            for(int i = 0; i < carbon[0].Length; i++) {
                if(carbon.Count == 1) //target value reached
                    break;
                for(int j = 0; j < carbon.Count; j++) {
                    if(carbon[j][i] == '1') {
                        numOnes++;
                    }
                    else
                        numZero++;
                }
                if(numOnes < numZero)
                    target = '1';
                else
                    target = '0';
                for(int j = 0; j < carbon.Count; j++) {
                    if(carbon[j][i] != target) {
                        carbon.RemoveAt(j);
                        j--;
                    }
                }
                numOnes = 0;
                numZero = 0;
            }
            Console.WriteLine("Product: " + Convert.ToInt32(oxygen[0],2) * Convert.ToInt32(carbon[0],2));
        }
    }
}
