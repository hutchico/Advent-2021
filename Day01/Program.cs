using System;
using System.Collections.Generic;
using System.IO;


namespace Day01 {
    class Program {
        static void Main() {
            int previous;
            int current;
            int num_occurences = 0;
            //Part 1:
            using(StreamReader file = new StreamReader("input.txt")) {
                previous = Int32.Parse(file.ReadLine());
                while(!file.EndOfStream) {
                    current = Int32.Parse(file.ReadLine());
                    if(current > previous) {
                        num_occurences++;
                    }
                    previous = current;
                }
            }
            Console.WriteLine("Part 1 answer: " + num_occurences);

            //Part 2:

            int previous_window = 0;
            int current_window;
            num_occurences = 0;

            using(StreamReader file = new StreamReader("input.txt")) {
                Window tracker = new Window(Int32.Parse(file.ReadLine()),Int32.Parse(file.ReadLine()),Int32.Parse(file.ReadLine()));
                previous_window = tracker.get_sum();
                while(!file.EndOfStream) {
                    tracker.add_measurement(Int32.Parse(file.ReadLine()));
                    current_window = tracker.get_sum();
                    if(current_window > previous_window) {
                        num_occurences++;
                    }
                    previous_window = current_window;
                }
            }
            Console.WriteLine("Part 2 answer: " + num_occurences);
        }
    }
}
    class Window{
        public Window(int in1, int in2, int in3) { 
            meas0 = in1; meas1 = in2; meas2 = in3;
            meas0_age = 2; meas1_age = 1; meas2_age = 0;
        }

        public int get_sum() {
            return meas0 + meas1 + meas2;
        }

        public void add_measurement(int input) { //Replace the oldest measurement
            switch(find_oldest()) {
                case 0:
                    meas0 = input;
                    meas0_age = -1;
                    break;
                case 1:
                    meas1 = input;
                    meas1_age = -1;
                    break;
                case 2:
                    meas2 = input;
                    meas2_age = -1;
                    break;
            }
            age_meas();
        }

        private void age_meas() {
            meas0_age++;
            meas1_age++;
            meas2_age++;
        }

        private int find_oldest() {
            int start = -2;
            int target = -1;
            if(meas0_age > start) {
                start = meas0_age;
                target = 0;
            }
            if(meas1_age > start) {
                start = meas1_age;
                target = 1;
            }
            if(meas2_age > start) {
                start = meas2_age;
                target = 2;
            }

            return target;
        }

        private int meas0_age;
        private int meas1_age;
        private int meas2_age;
        private int meas0;
        private int meas1;
        private int meas2;
    }


