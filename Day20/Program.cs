using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20 {
    class Program {
        static void Main() {
            List<char> IEA;
            char[,] image;
            List<List<char>> image_raw = new List<List<char>>();
            using(StreamReader file = new StreamReader("input.txt")) {
                IEA = file.ReadLine().ToList();
                file.ReadLine(); //discard empty line
                while(!file.EndOfStream) {
                    image_raw.Add(file.ReadLine().ToList());
                }
            }

            image = new char[image_raw.Count, image_raw[0].Count];
            for(int i = 0; i < image.GetLength(1); i++) {
                for(int j = 0; j < image.GetLength(0); j++) {
                    image[j, i] = image_raw[j][i];
                }
            }

            //part 1: i < 2
            for(int i = 0; i < 2; i++) {
                image = apply_iea(image, IEA, i % 2);
            }
            Console.WriteLine("2 passes number of lit pixels counted: " + count(image));
            //part 2: i < 50
            for(int i = 2; i < 50; i++) {
                image = apply_iea(image, IEA, i % 2);
            }
            Console.WriteLine("50 passes number of lit pixels counted: " + count(image));
        }

        static char[,] apply_iea(char[,] image, List<char> iea, int mode) {
            int sizeX, sizeY;
            List<char> chars;
            char[,] copy;
            char[,] paste;
            sizeX = image.GetLength(1) + 4; //expand boundaries to account for "infinity"
            sizeY = image.GetLength(0) + 4;
            copy = new char[sizeY, sizeX];
            paste = new char[sizeY - 2, sizeX - 2];
            for(int i = 0; i < copy.GetLength(1); i++) {
                for(int j = 0; j < copy.GetLength(0); j++) {
                    if(i <= 1 || j <= 1 || i >= copy.GetLength(1) - 2 || j >= copy.GetLength(0) - 2)
                        copy[j, i] = mode == 1 ? '#' : '.'; //eric you cheeky little—
                    else
                        copy[j, i] = image[j - 2, i - 2];
                }
            }
            for(int i = 1; i < copy.GetLength(1) - 1; i++) {
                for(int j = 1; j < copy.GetLength(0) - 1; j++) {
                    chars = new List<char>();
                    for(int k = -1; k < 2; k++) {
                        for(int l = -1; l < 2; l++) {
                            chars.Add(copy[j + k, i + l]);
                        }
                    }
                    paste[j - 1, i - 1] = iea[compute_binary(chars)];
                }
            }
            return paste;
        }

        static int compute_binary(List<char> chars) {
            string bin = "";
            for(int i = 0; i < chars.Count; i++)
                bin += chars[i] == '#' ? '1' : '0';
            return Convert.ToInt32(bin, 2);
        }
        static int count(char[,] image) {
            int num = 0;
            for(int i = 0; i < image.GetLength(0); i++) {
                for(int j = 0; j < image.GetLength(1); j++) {
                    num += image[j, i] == '#' ? 1 : 0;
                }
            }
            return num;
        }
        static void print_image(char[,] image) {
            for(int i = 0; i < image.GetLength(0); i++) {
                for(int j = 0; j < image.GetLength(1); j++) {
                    Console.Write(image[i, j]);
                }
                Console.Write('\n');
            }
        }
    }
}
