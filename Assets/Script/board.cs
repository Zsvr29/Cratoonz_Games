using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class board : MonoBehaviour
{
   
    public int height;
    public int widht;

    public int offset;
    private  BackgroundTile[,]  alltiles;
    public GameObject[] dots;
    public GameObject tilePrefabs;
    public GameObject[,] allDots;
    public int control;


    void Start()
    {
        alltiles = new BackgroundTile[height, widht];
        allDots =  new GameObject[widht, height];
        SetUp();

    }

    private void SetUp()
        {

        for (int i = 0; i < widht; i++)
        {
            for (int j= 0;  j<height; j++)

            {
                Vector2 tempposition = new Vector2(i,j + offset);
                Vector2 tempposition2 = new Vector2(i,j );

              GameObject BackgroundTile=  Instantiate(tilePrefabs,tempposition2 ,Quaternion.identity) as GameObject;
                BackgroundTile.transform.parent = this.transform;
                BackgroundTile.name = "(" + i + "," + j + ")";
                int dotUse = Random.Range(0, dots.Length);
                int maxIterations = 0;

                while (MatchesAt (i , j ,dots[dotUse])  && maxIterations< 100 )
                {
                    dotUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;
                GameObject dot = Instantiate(dots[dotUse], tempposition, Quaternion.identity);
                dot.GetComponent<dot>().row = j;
                dot.GetComponent<dot>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }

    }  
    private  bool MatchesAt(int column , int row, GameObject piece)
    {

        if (column > 1 &&  row > 1)
        {
            if (allDots [column -1,row].tag==piece .tag && allDots[column-2 ,row ])
            {
                return true;
            }
            if (allDots[column, row -1].tag == piece.tag && allDots[column , row -2])
            {
                return true;
            }

        }
        else if (column <= 1 || row <=1)
        {
            if (row>1 )
            {
                if (allDots[column ,row -1 ].tag==piece.tag && allDots[column, row -2].tag == piece.tag )
                {
                    return true;

                }
            }

            if ( column  > 1)
            {
                if (allDots[column-1, row ].tag == piece.tag && allDots[column -2, row ].tag == piece.tag)
                {
                    return true;

                }
            }
        }


        return false;


    }
     private void DestroyMatchesAt( int column , int row)
    {
        if (allDots[column ,row ].GetComponent<dot>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }
    public void DestroyMatches()
    {
        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        StartCoroutine(DecreaseRowCo());
    }

    private  IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i,j] == null)
                {
                    nullCount++;
                }
                else if (nullCount>0)
                {
                    allDots[i, j].GetComponent<dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;

        }
        yield return  new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard()
    {
        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 temposition = new Vector2(i, j + offset);
                    int dotUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotUse], temposition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<dot>().row = j;
                    piece.GetComponent<dot>().column= i;
                    Destroy(allDots[control, j]);
                }
            }

        }
    }
     private  bool MatchesOnBoard()
    {

        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j] .GetComponent<dot> ().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;

    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

         while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(0.5f);
       
    }
}
