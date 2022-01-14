using System;
using System.IO;
using System.Collections.Generic;

namespace Day07 {
    class Program {
        static void Main() {
            List<int> crabs = new List<int>();
            int fuel = 0;
            int median;
            int avg = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                string[] tkn = file.ReadLine().Split(',');
                for(int i = 0; i < tkn.Length; i++)
                    crabs.Add(Int32.Parse(tkn[i]));
            }
            crabs.Sort();
            //theory: minimum distance is the median value of the array?
            median = crabs[crabs.Count / 2];
            for(int i = 0; i < crabs.Count; i++)
                fuel += Math.Abs(crabs[i] - median);
            Console.WriteLine("Fuel needed: " + fuel);

            //part 2:
            //theory: go from median value to average value?
            for(int i = 0; i < crabs.Count; i++)
                avg += crabs[i];
            avg /= crabs.Count;
            fuel = 0;
            for(int i = 0; i < crabs.Count; i++)
                fuel += compute_fuel(crabs[i],avg);
            Console.WriteLine("Fuel needed: " + fuel);
        }

        static int compute_fuel(int start,int end) {
            int distance = Math.Abs(start - end);
            int total = 0;
            for(int i = 0; i < distance; i++) 
                total += i + 1;
            return total;
        }
    }
}
