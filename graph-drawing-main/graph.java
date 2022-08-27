/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javaapplication2;


/**
 *
 * @author pc
 */
public class graph {
    protected static int nodenumber,eadges;


     public graph(int nodenumber,int eadges)
    {
        this.nodenumber=nodenumber;
        this.eadges=eadges;
//         for(int i=0;i<nodenumber;i++)
//        {
//            checkofnode[i]=false;
//        }
    }
    public int getNodenumber() {
        return nodenumber;
    }

    public void setNodenumber(int nodenumber) {
        this.nodenumber = nodenumber;
    }

    public int getEadges() {
        return eadges;
    }

    public void setEadges(int eadges) {
        this.eadges = eadges;
    }
    public static int[][] adj_graph = new int[1000][1000];   
    
    //   this is constructor for the class
   
    ////// this method to clear the adj_matrix of graph
    public void clear ()
    {
        for(int i=0;i<nodenumber;i++)
        {
            for (int j=0;j<nodenumber;j++)
            {
                adj_graph[i][j]=0;
            }
        }
    }
    

    ///////////////////////////to display the adj_matrix of the  graph 
    
    public void displaymatrix()
    {
        System.out.println(nodenumber);
        for (int i=0;i<nodenumber;i++)
        {
            for (int j=0;j<nodenumber;j++)
            {
                System.out.print(adj_graph[i][j]+"  ");
            }
            System.out.println(" ");
        }
    }

  
  
/////////////////////DELETE OR POP NODE ////////////////////////////////////////

}
