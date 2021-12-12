using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day12 {
    class Program {
        static void Main() {
            List<KeyValuePair<string,string>> nodes_raw = new List<KeyValuePair<string,string>>();
            List<string> nodes_toMake = new List<string>();
            List<Node> nodes = new List<Node>();
            Dictionary<string,int> indices = new Dictionary<string,int>(); //Map a node name to its position in nodes
            List<string> unique_paths = new List<string>(); //track each path produced with this?
            //List<string> visited = new List<string>();
            Dictionary<string,int> visited = new Dictionary<string,int>();
            using(StreamReader file = new StreamReader("input.txt")) {
                while(!file.EndOfStream) {
                    string[] tk = file.ReadLine().Split('-');
                    if(!nodes_toMake.Contains(tk[0]))
                        nodes_toMake.Add(tk[0]);
                    if(!nodes_toMake.Contains(tk[1]))
                        nodes_toMake.Add(tk[1]);
                    nodes_raw.Add(new KeyValuePair<string,string>(tk[0],tk[1]));
                }
            }
            //nodes_raw.Sort((x,y) => x.Key.CompareTo(y.Key));
            nodes_toMake.Sort();
            for(int i = 0; i < nodes_toMake.Count; i++) { //construct N nodes, add to node list
                nodes.Add(new Node(nodes_toMake[i]));
                indices[nodes_toMake[i]] = i;
            }
            for(int i = 0; i < nodes_raw.Count; i++) {
                int noderef1 = indices[nodes_raw[i].Key];
                int noderef2 = indices[nodes_raw[i].Value];
                nodes[noderef1].add_node(nodes[noderef2]);
                nodes[noderef2].add_node(nodes[noderef1]);
            }

            visited["start"] = 1; //part 1: 1, part 2: 2
            for(int i = 0; i < nodes[indices["start"]].get_connection_size(); i++) {
                string path = "start,";
                //unique_paths.AddRange(find_path(nodes[indices["start"]].connections[i],ref visited,path));
                unique_paths = unique_paths.Union(find_path(nodes[indices["start"]].connections[i],ref visited,path)).ToList();
            }


            Console.WriteLine("unique paths found:" + unique_paths.Count);

        }

        static List<string> find_path(Node current, ref Dictionary<string,int> visited, string path) {
            List<string> paths = new List<string>();
            Dictionary<string,int> visitation = new Dictionary<string,int>(visited);
            if(current.get_type() == Type.Small && visitation.ContainsKey(current.get_name()) && visited[current.get_name()] > 0) //part 1: > 0
                return paths;
            if(current.get_name() == "end") {
                path += "end";
                paths.Add(path);
                return paths;
            }
            //visitation.Add(current.get_name());
            if(visitation.ContainsKey(current.get_name()))
                visitation[current.get_name()]++;
            else
                visitation[current.get_name()] = 1;
            path += current.get_name() + ',';
            for(int i = 0; i < current.get_connection_size(); i++) {
                //paths.AddRange(find_path(current.connections[i],ref visitation,path));
                paths = paths.Union(find_path(current.connections[i],ref visitation,path)).ToList();
            }
            return paths;
        }
       

    }

    public class Node {

        public Node(string name) {
            this.name = name;
            visited = false;
            connections = new List<Node>();
            type = name[0] < 95 ? Type.Large : Type.Small; //technically 97 but irrelevant to structure
        }


        public void add_node(Node toAdd) {
            if(!connections.Contains(toAdd)) //if it's already in this list then drop it
                connections.Add(toAdd);
        }

        public string get_name() { return name; }
        public List<Node> get_connections() { return connections; }
        public int get_connection_size() { return connections.Count; }
        public bool get_visited() { return visited; }
        public Type get_type() { return type; }

        private bool visited; //record if pathfinding algorithm has already visited this node once (if small)
        private Type type;
        public List<Node> connections;
        private string name;
    }

    public enum Type {
        Large,
        Small
    }
}
