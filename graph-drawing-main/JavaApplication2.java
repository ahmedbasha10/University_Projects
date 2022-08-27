/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javaapplication2;
import java.util.Scanner;


/**
 *
 * @author pc
 */

public class JavaApplication2 {
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
    
        Scanner input = new Scanner(System.in);
        int nodenumber, eadges_num;

        System.out.println("Enter  the number of nodes you need");
        nodenumber = input.nextInt();

        System.out.println("Enter  the number of eadges you need");
        eadges_num = input.nextInt();

        graph new_graph = new graph(nodenumber, eadges_num);
        new_graph.clear();

        eadges neweadge = new eadges(nodenumber, eadges_num);
        new_graph.adj_graph = neweadge.adj_graph;
        neweadge.input(eadges_num);
       
 
        int query_type;
        while (true)
        {
            System.out.println("Enter numberof choise type of query you want ?");
            System.out.println("1-add node");
            System.out.println("2-delete node");
            System.out.println("3-add edge or update ");
            System.out.println("4-delete eadge");
            System.out.println("5-clear graph");
            System.out.println("6-disply graph");
            System.out.println("7-calculate shortest path between two nodes");
            System.out.println("8-disply children of node");
            System.out.println("9-Exit");
            query_type = input.nextInt();
       
            switch (query_type) {
                case 1:
                    int addnode = input.nextInt();
                    
                    neweadge.add_node(addnode);
                    break;
                //////////////////////////
                case 2:
                    neweadge.popnode();
                    break;
                case 3:
                    neweadge.add_edge();
                    break;
                //////////////////////////
                case 4:
                    neweadge.Delete_Edge();
                    break;
                case 5:
                    new_graph.clear();
                    break;
                case 6:
                    new_graph.displaymatrix();
                    break;
                case 9:
                    break;
                 default:
                     System.out.println("Enter Valid Number!!");
            }
        }
    }
}
