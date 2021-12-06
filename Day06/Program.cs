using System;
using System.IO;
using System.Collections.Generic;

namespace Day06 {
    class Program {
        static void Main() {
            List<int> school = new List<int>();
            tracker li = new tracker();
            using(StreamReader file = new StreamReader("input.txt")) {
                string[] raw = file.ReadLine().Split(',');
                for(int i = 0; i < raw.Length; i++) {
                    school.Add(Int32.Parse(raw[i]));
                    switch(Int32.Parse(raw[i])) {
                        case 0:
                            li.day0++;
                            break;
                        case 1:
                            li.day1++;
                            break;
                        case 2:
                            li.day2++;
                            break;
                        case 3:
                            li.day3++;
                            break;
                        case 4:
                            li.day4++;
                            break;
                        case 5:
                            li.day5++;
                            break;
                        case 6:
                            li.day6++;
                            break;
                        case 7:
                            li.day7++;
                            break;
                        case 8:
                            li.day8++;
                            break;
                    }
                }
            }

            for(int i = 0; i < 80; i++) { //Part 1: 80 day span, part 2: 256 day span
                age_fish(ref school);
            }
            Console.WriteLine("Number of fish after 80 days: " + school.Count);
            for(int i = 0; i < 256; i++) {
                age_fish_2(ref li);
            }
            Console.WriteLine("Number of fish after 256 days: " + sum_school(li));
        }
        static void age_fish(ref List<int> school) {
            int base_size = school.Count;
            for(int i = 0; i < base_size; i++) { //only cover fish present at the start of day
                school[i]--;
                if(school[i] < 0) { //reset counter and spawn a new fish
                    school[i] = 6;
                    school.Add(8);
                }
            }
        }

        static void age_fish_2(ref tracker li) { //shuffle every fish count down one indice and tack on the new ones at the end
            Int64 temp;
            Int64 temp2;
            temp = li.day7;
            li.day7 = li.day8;
            li.day8 = li.day0;
            temp2 = li.day6;
            li.day6 = temp + li.day0;
            temp = li.day5;
            li.day5 = temp2;
            temp2 = li.day4;
            li.day4 = temp;
            temp = li.day3;
            li.day3 = temp2;
            temp2 = li.day2;
            li.day2 = temp;
            temp = li.day1;
            li.day1 = temp2;
            li.day0 = temp;
        }

        static Int64 sum_school(tracker li) {
            return li.day0 + li.day1 + li.day2 + li.day3 + li.day4 + li.day5 + li.day6 + li.day7 + li.day8;
        }

        static void print_school(List<int> school, int day) {
            Console.Write("Day " + day + ": ");
            for(int i = 0; i < school.Count; i++) {
                Console.Write(school[i] + ',');
            }
            Console.Write('\n');
        }

       public struct tracker { //keep track visually of each fish
            public Int64 day0;
            public Int64 day1;
            public Int64 day2;
            public Int64 day3;
            public Int64 day4;
            public Int64 day5;
            public Int64 day6;
            public Int64 day7;
            public Int64 day8;
        }
    }
}
