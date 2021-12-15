using System;
using System.IO;
using System.Collections.Generic;

///NOTE: unlike everything else to date this program targets .net 6.0 instead of .net 5.0

namespace Day15 {
    class Program{
        static void Main(){
            Node[,] smallnodes = new Node[100, 100]; //hardcoded to match input
            Node[,] bignodes = new Node[500, 500];
            int[,] distances = new int[100, 100];
            int[,] longdistances = new int[500, 500];
            for (int i = 0; i < distances.GetLength(1); i++){
                for (int j = 0; j < distances.GetLength(0); j++){
                    distances[j, i] = int.MaxValue;
                }
            }
            for (int i = 0; i < longdistances.GetLength(1); i++){
                for (int j = 0; j < longdistances.GetLength(0); j++){
                    longdistances[j, i] = int.MaxValue;
                }
            }

            using (StreamReader file = new StreamReader("input.txt")){
                for (int i = 0; i < smallnodes.GetLength(1); i++){
                    for (int j = 0; j < smallnodes.GetLength(0); j++){
                        int risk = file.Read();
                        smallnodes[j, i] = new Node(risk - 48, i, j);
                        bignodes[j, i] = new Node(risk - 48, i, j);
                    }
                    file.Read(); //remove newline
                }
            }

            //part 2: copy nodes into bignodes with a repetition condition
            //start down the line once, then across x for all x00 rows
            for (int i = 0; i < smallnodes.GetLength(1); i++){
                for (int j = smallnodes.GetLength(0); j < bignodes.GetLength(0); j++){
                    int newrisk = smallnodes[j % 100,i].get_risk() + (j / 100);
                    if (newrisk > 9)
                        newrisk -= 9;
                    
                    bignodes[j, i] = new Node(newrisk,i,j);
                }
            }
            for (int i = smallnodes.GetLength(1); i < bignodes.GetLength(1); i++){
                for (int j = 0; j < bignodes.GetLength(0); j++) {
                    int newrisk = bignodes[j, i % 100].get_risk() + (i / 100);
                    if (newrisk > 9)
                        newrisk -= 9;

                    bignodes[j, i] = new Node(newrisk, i, j);
                }
            }

            //use each node to construct a graph of itself
            construct(ref smallnodes);
            construct(ref bignodes);

            dijkstra(smallnodes, ref distances);
            Console.WriteLine("Lowest risk: " + distances[distances.GetLength(0) - 1, distances.GetLength(1) - 1]);
            dijkstra(bignodes, ref longdistances);
            Console.WriteLine("Lowest risk: " + longdistances[longdistances.GetLength(0) - 1, longdistances.GetLength(1) - 1]);
        }

        static void construct(ref Node[,] nodes){
            //reminder that nodes[i,j] is equivalent to nodes[y,x];
            for (int i = 0; i < nodes.GetLength(1); i++) {
                for (int j = 0; j < nodes.GetLength(0); j++){
                    if (i != 0)
                        nodes[i, j].add_connection(nodes[i - 1, j]);
                    if (j != 0)
                        nodes[i, j].add_connection(nodes[i, j - 1]);
                    if (j != nodes.GetLength(0) - 1)
                        nodes[i, j].add_connection(nodes[i, j + 1]);
                    if (i != nodes.GetLength(1) - 1)
                        nodes[i, j].add_connection(nodes[i + 1, j]);
                }
            }
        }

        static void dijkstra(Node[,] nodes, ref int[,] distances){
            PriorityQueue<int, (int, int)> pile = new PriorityQueue<int, (int, int)>();  //cool and good
            distances[0, 0] = 0;
            pile.Enqueue(0, (0, 0));
            do
            {
                pile.TryDequeue(out int dist, out (int, int) coords);
                List<Node> consider = nodes[coords.Item1, coords.Item2].get_connections();
                for (int i = 0; i < consider.Count; i++)
                {
                    int newdist = dist + consider[i].get_risk();
                    int cY = consider[i].get_coordY();
                    int cX = consider[i].get_coordX();
                    if (newdist < distances[cY, cX])
                    {
                        distances[cY, cX] = newdist;
                        pile.Enqueue(newdist, (cY, cX));
                    }
                }

            }
            while (pile.Count != 0);
        }

        class Node{
            public Node(int risk, int x, int y){
                lines = new List<Node>();
                this.risk = risk;
                coordX = x;
                coordY = y;
            }

            public int get_risk() { return risk; }
            public int get_coordX() { return coordX; }
            public int get_coordY() { return coordY; }
            public List<Node> get_connections() { return lines; }
            public void add_connection(Node toAdd){
                lines.Add(toAdd);
            }

            private int risk;
            private int coordX;
            private int coordY;
            private List<Node> lines; //link to each adjacent neighbor
        }
    }
}
