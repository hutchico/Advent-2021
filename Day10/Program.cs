using System;
using System.Collections.Generic;
using System.IO;

namespace Day10 {
    class Program {
        static void Main() {
            List<string> commands = new List<string>();
            List<char> illegals = new List<char>();
            List<string> legals = new List<string>();
            List<Int64> scores = new List<Int64>();
            Stack<char> log = new Stack<char>();
            List<Stack<char>> incompletes = new List<Stack<char>>();
            int value_sum = 0;
            using(StreamReader file = new StreamReader("input.txt")) {
                while(!file.EndOfStream)
                    commands.Add(file.ReadLine());
            }
            //part 1
            for(int i = 0; i < commands.Count; i++) {
                bool toggle = false;
                for(int j = 0; j < commands[i].Length; j++) {
                    char c = commands[i][j];
                    if(c == '[' || c == '<' || c == '(' || c == '{') {
                        log.Push(c);
                    }
                    else { //c == >,},], or )
                        if(c == ']') { //was going to switch case it but that already includes breaks
                            if(log.Peek() == '[')
                                log.Pop();
                            else {
                                illegals.Add(c);
                                toggle = true; //needed for part 2; remove this line from consideration
                                break;
                            }
                        }
                        else if(c == '}') {
                            if(log.Peek() == '{')
                                log.Pop();
                            else {
                                illegals.Add(c);
                                toggle = true;
                                break;
                            }
                        }
                        else if(c == '>') {
                            if(log.Peek() == '<')
                                log.Pop();
                            else {
                                illegals.Add(c);
                                toggle = true;
                                break;
                            }
                        }
                        else if(c == ')') {
                            if(log.Peek() == '(')
                                log.Pop();
                            else {
                                illegals.Add(c);
                                toggle = true;
                                break;
                            }
                        }
                    }
                }
                if(toggle) {
                    commands.RemoveAt(i);
                    i--;
                    toggle = false;
                }
                else {//this is an incomplete line not a corrupt one, save it for part 2
                    incompletes.Add(new Stack<char>(log));
                }
                log.Clear();
            }
            for(int i = 0; i < illegals.Count; i++) {
                switch(illegals[i]) {
                    case ')':
                        value_sum += 3;
                        break;
                    case ']':
                        value_sum += 57;
                        break;
                    case '}':
                        value_sum += 1197;
                        break;
                    case '>':
                        value_sum += 25137;
                        break;
                }
            }
            Console.WriteLine("Total score: " + value_sum);

            //part 2
            for(int i = 0; i < incompletes.Count; i++) {
                string end = "";
                while(true) {
                    try {
                        char c = incompletes[i].Pop();
                        switch(c) {
                            case '(':
                                end = ')' + end;
                                break;
                            case '[':
                                end = ']' + end;
                                break;
                            case '{':
                                end = '}' + end;
                                break;
                            case '<':
                                end = '>' + end;
                                break;
                        }
                    }
                    catch(InvalidOperationException) {
                        break;
                    }
                }
                legals.Add(end);
            }

            for(int i = 0; i < legals.Count; i++) {//tried to be cheeky and do this above but it did things in reverse order
                Int64 score = 0;
                for(int j = 0; j < legals[i].Length; j++) {
                    char c = legals[i][j];
                    switch(c) {
                        case ')':
                            score = score * 5 + 1;
                            break;
                        case ']':
                            score = score * 5 + 2;
                            break;
                        case '}':
                            score = score * 5 + 3;
                            break;
                        case '>':
                            score = score * 5 + 4;
                            break;
                    }
                }
                scores.Add(score);
            }
            scores.Sort(); //default sort is ascending order
            Console.WriteLine("End score: " + scores[scores.Count / 2]);
        }
    }
}
