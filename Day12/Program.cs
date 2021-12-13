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
            List<string> unique_paths = new List<string>(); //track each path produced with this?
            Dictionary<string,int> indices = new Dictionary<string,int>(); //Map a node name to its position in nodes
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
            for(int i = 0; i < nodes_toMake.Count; i++) { //construct N nodes, add to node list
                nodes.Add(new Node(nodes_toMake[i]));
                indices[nodes_toMake[i]] = i;
            }
            for(int i = 0; i < nodes_raw.Count; i++) {
                int node_key = indices[nodes_raw[i].Key]; //make each node point to its partner
                int node_value = indices[nodes_raw[i].Value];
                nodes[node_key].add_node(nodes[node_value]);
                nodes[node_value].add_node(nodes[node_key]); 
            }

            
            for(int i = 0; i < nodes[indices["start"]].connections.Count; i++) {
                string path = "start,";
                unique_paths = unique_paths.Union(find_path(nodes[indices["start"]].connections[i],ref visited,path,false)).ToList();
            }


            Console.WriteLine("unique paths found:" + unique_paths.Count);

        }

        static List<string> find_path(Node current, ref Dictionary<string,int> visited, string path, bool twice) {
            List<string> paths = new List<string>();
            Dictionary<string,int> visitation = new Dictionary<string,int>(visited);
            bool numSmall = twice;
            if(current.get_name() == "start")
                return paths;
            //part 2: change twice comparison from false to true
            if(current.get_type() == Type.Small && visitation.ContainsKey(current.get_name()) && twice == false && visited[current.get_name()] >= 1) //part 1: > 0
                return paths;

            if(current.get_name() == "end") {
                path += "end";
                paths.Add(path);
                return paths;
            }
            if(visitation.ContainsKey(current.get_name()))
                visitation[current.get_name()]++;
            else
                visitation[current.get_name()] = 1;
            if(current.get_type() == Type.Small && visitation[current.get_name()] >= 2)
                numSmall = true;
            path += current.get_name() + ',';
            for(int i = 0; i < current.connections.Count; i++) {
                //part 2: change "false" to "numSmall"
                paths = paths.Union(find_path(current.connections[i],ref visitation,path,false)).ToList();
            }
            return paths;
        }
       

    }

    public class Node {

        public Node(string name) {
            this.name = name;
            connections = new List<Node>();
            type = name[0] < 95 ? Type.Large : Type.Small; //technically 97 but irrelevant to structure
        }


        public void add_node(Node toAdd) {
            if(!connections.Contains(toAdd)) //if it's already in this list then drop it
                connections.Add(toAdd);
        }

        public string get_name() { return name; }
        public Type get_type() { return type; }

        private Type type;
        public List<Node> connections;
        private string name;
    }

    public enum Type {
        Large,
        Small
    }
}
