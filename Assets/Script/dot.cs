using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dot : MonoBehaviour
{
    [Header ("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    private GameObject otherDot;
    private board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 temPosition;
    public float swipeAngle = 0f;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public float swipeResist = 0.3f;



    void Start()
    {
        board = FindObjectOfType<board>();
       // targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
       // row = targetY;
       // column = targetX;
       // previousRow = row;
       // previousColumn = column;


    }


    void Update()
    {
        FindMatches();
       
        if (isMatched)
        {
            SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
            myRenderer.color = new Color(0f, 0f, 0f, 0.2f);
        }
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temPosition, 7f * Time.deltaTime);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

        }
        else
        {
          
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = temPosition;

        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            
            temPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, temPosition, 7f * Time.deltaTime);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
          
            temPosition = new Vector2(transform.position.x, targetY);
            transform.position = temPosition;

        }
    }
    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<dot>().isMatched)
            {
                otherDot.GetComponent<dot>().row = row;
                otherDot.GetComponent<dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(0.5f);
              

            }
            else
            {
                board.DestroyMatches();

            }
            otherDot = null;
        }

    }

    private void OnMouseDown()
    {
     
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        


    }
    private void OnMouseUp()
    {
       
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        
    }
    void CalculateAngle()
    {

        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI; //mouse hareketlerinin hangi açýyla geldiðini arctangent ile bulma                                                                                                         // Debug.Log(swipeAngle);
            SwipeSPrites();
            Debug.Log(swipeAngle);
          
        }
       

    }
    void SwipeSPrites()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.widht - 1)
        {
            //saða 
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //yukarý 
            otherDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //sola 
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //aþaðý 
            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMove());
    }
    void FindMatches()
    {
        if (column > 0 && column < board.widht - 1)
        {

            GameObject leftDot = board.allDots[column - 1, row];
            GameObject rightDot = board.allDots[column + 1, row];
            if (leftDot != null && rightDot != null)
            {
                if (leftDot.tag == this.gameObject.tag && rightDot.tag == this.gameObject.tag)
                {
                    leftDot.GetComponent<dot>().isMatched = true;
                    rightDot.GetComponent<dot>().isMatched = true;
                    isMatched = true;

                }
            }

        }
        if (row > 0 && row < board.height - 1)
        {

            GameObject upDot = board.allDots[column, row + 1];
            GameObject downDot = board.allDots[column, row - 1];
            if (upDot != null && downDot != null)
            {
                if (upDot.tag == this.gameObject.tag && downDot.tag == this.gameObject.tag)
                {
                    upDot.GetComponent<dot>().isMatched = true;
                    downDot.GetComponent<dot>().isMatched = true;
                    isMatched = true;

                }
            }
        }
    }
}

