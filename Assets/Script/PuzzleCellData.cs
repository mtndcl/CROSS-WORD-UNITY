using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCellData : MonoBehaviour
{


    public int Cell_x;

    public int Cell_y;

    public int ground_X;

    public int ground_Y;

    public char c;

    public int id;

    

    private GameObject GameManager;

    private void Start()
    {
        ground_X = -1;

        ground_Y = -1;

        GameManager = GameObject.FindGameObjectWithTag("gamemanager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag=="groundcell")
        {

            ground_X = collision.gameObject.GetComponent<GrounCellData>().Ground_x;
            ground_Y = collision.gameObject.GetComponent<GrounCellData>().Ground_y;
        }else if (collision.gameObject.tag == "puzzlecell")
        {


            GameManager.GetComponent<ShapeManager>().CollisionNumber++;
          
        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "puzzlecell")
        {


            GameManager.GetComponent<ShapeManager>().CollisionNumber--;

        }
    }




}


