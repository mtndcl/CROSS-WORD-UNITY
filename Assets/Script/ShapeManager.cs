using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShapeManager : MonoBehaviour
{


    private float startPosX;

    private float startPosY;

    public bool isBeingHeld = false;

    public GameObject selectedobject;

    private Vector2 initposition;

    private GameObject ContainerPuzzleCell;

    public List<GameObject> SelectedCells;

    private  GameObject[,] GroundMatrix;

    private GameObject SuccesPanel;

    private GameObject[] Objects;

    private GameObject PuzzleCellContainer;

    public int CollisionNumber;

    private List<GameObject> PuzzleCellParents;

    private LayerMask mask;

    public Material metarial;

    private float ReagentSize;

    private float initialFingersDistance;

    private Vector3 initialScale;

    private GameObject gamescene;

    private GameObject Canvas;

    private Vector2 FirstClickLocation;

    private float ScreenWidth;

    private float ScreenHeight;

    private Vector3 GameSceneFirstLocation;

    private GameObject BoundsParent;

    public GameObject OverMenü;

    private String State;

    private bool GoSmall;
    void Start()
    {
        GoSmall = false;

        State = "null";

        BoundsParent = GameObject.FindGameObjectWithTag("bound");
        
        mask = LayerMask.GetMask("foreground");

        PuzzleCellParents = new List<GameObject>();

        CollisionNumber = 0;

         GroundMatrix =GetComponent<InitGame>().GroundMatrix;

        ContainerPuzzleCell = GameObject.FindGameObjectWithTag("containerpuzzlecell");

        SuccesPanel = GameObject.FindGameObjectWithTag("succespanel");

        SuccesPanel.SetActive(false);

        PuzzleCellContainer = GameObject.FindGameObjectWithTag("containerpuzzlecell");

        ReagentSize = GameData.Blocksize / 20;

        gamescene = GameObject.FindGameObjectWithTag("gamescene");

        Canvas = GameObject.FindGameObjectWithTag("canvas");

        ScreenHeight = Canvas.GetComponent<RectTransform>().rect.height;

        ScreenWidth = Canvas.GetComponent<RectTransform>().rect.width;

    }
    void Update()
    {

        if (GameData.GameState=="playing")
        {
           

            if (selectedobject==null  &&  Input.touchCount>0)
            {

                if (Input.touchCount == 1 && State=="null")
                {

                    Panning();
                    
                }
                else if (Input.touchCount==2)
                {
                    
                    Zoomm();
                }
            }
            GamePlay();
        }
    }
    private void Panning()
    {
        if ( Input.GetTouch(0).phase==TouchPhase.Began)
        {
            FirstClickLocation = Input.GetTouch(0).position;
            GameSceneFirstLocation = gamescene.transform.position;
        }
        Vector3 Movement = Input.GetTouch(0).position- FirstClickLocation;


        Vector2 first= BoundsParent.transform.GetChild(0).transform.position;
        Vector2 second = BoundsParent.transform.GetChild(1).transform.position;


        if (Math.Abs(Movement.x)> Math.Abs(Movement.y))
        {
            if (Movement.x > 0)
            {



                if (first.x < -ScreenWidth / 8)
                {
                    gamescene.transform.position = GameSceneFirstLocation + new Vector3(Movement.x, 0, 0);
                }
            }
            else if (Movement.x < 0)
            {

                if (second.x > ScreenWidth * 11 / 10)
                {
                    gamescene.transform.position = GameSceneFirstLocation + new Vector3(Movement.x, 0, 0);
                }

            }
        }
        else
        {
            if (Movement.y > 0)
            {
                if (second.y < -ScreenHeight / 10)
                {
                    gamescene.transform.position = GameSceneFirstLocation + new Vector3(0, Movement.y, 0);
                }

               
            }
            else if (Movement.y < 0)
            {
                if (first.y > ScreenHeight * 11 / 10)
                {
                    gamescene.transform.position = GameSceneFirstLocation + new Vector3(0, Movement.y, 0);
                }

            }
        }

        

          
    }
    private void GamePlay()
    {
        MoveObject();
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            FirstClickLocation = Camera.main.ScreenToWorldPoint(mousePos);
            isBeingHeld = true;

            selectedobject = ClickSelect();

           
            if (selectedobject != null && selectedobject.transform.parent != null && selectedobject.tag == "puzzlecell")
            {
               

                foreach (Transform cell in selectedobject.transform.parent)
                {
                        foreach (Transform v in cell.transform)
                        {
                            if (v.tag == "line")
                            {
                                Destroy(v.gameObject);
                            }
                        }
                }

                startPosX = mousePos.x - selectedobject.transform.parent.transform.position.x;
                startPosY = mousePos.y - selectedobject.transform.parent.transform.position.y;
                selectedobject.transform.parent.transform.SetSiblingIndex(ContainerPuzzleCell.transform.childCount-1);

                initposition = selectedobject.transform.parent.transform.position;
             
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            State = "null";
            print("State  set to: " + State);
            isBeingHeld = false;

            if (selectedobject != null && selectedobject.transform.parent != null && selectedobject.tag == "puzzlecell")
            {

                if(CollisionNumber>0)
                {
                    selectedobject.transform.parent.position = initposition;
                }
                else
                {
                    FitOnGround();

                    CalculateConnectStation();

                    CheckGameover();

                }

            }

            SelectedCells.Clear();
        }
    }
    private void Zoomm()
    {

        if (Input.touches.Length == 2)
        {

            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = gamescene.transform.localScale;

                State = "zoom";
              

            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {

                var currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;
                Vector3 scalingamount = initialScale * scaleFactor;
                Vector2 first = BoundsParent.transform.GetChild(0).transform.position;
                Vector2 second = BoundsParent.transform.GetChild(1).transform.position;

                if (scalingamount.x > 1f && scalingamount.x < 1.7f )
                {

                    ///Gamescene go to smaller
                    if (scaleFactor<1 &&  GoSmall)
                    {
                        GameData.Blocksize = GroundMatrix[0, 0].GetComponent<Image>().rectTransform.rect.width * scalingamount.x;
                        ReagentSize = GameData.Blocksize / 20;
                        gamescene.transform.localScale = scalingamount;
                    }else if(scaleFactor > 1){
                        GameData.Blocksize = GroundMatrix[0, 0].GetComponent<Image>().rectTransform.rect.width * scalingamount.x;
                        ReagentSize = GameData.Blocksize / 20;
                        gamescene.transform.localScale = scalingamount;
                    }

                    

                }

                
               GoSmall = false;

               if (first.x <= -ScreenWidth/10 && second.x >= ScreenWidth*11/10 && first.y >= ScreenHeight*11/10 && second.y <= -ScreenHeight/10)
               {

                  GoSmall = true;
               }
                
                
                
                
               

            }
        }


    }
    private void FitOnGround()
    {

        foreach (Transform puzzleparent in PuzzleCellContainer.transform)
        {
            foreach (Transform child in puzzleparent.transform)
            {

                PuzzleCellData data = child.GetComponent<PuzzleCellData>();

                child.transform.position = GroundMatrix[data.ground_X, data.ground_Y].transform.position;

            }

        }
    }
    private void CheckGameover()
    {


        Objects = GameObject.FindGameObjectsWithTag("puzzlecellparent");

        int objectnumber = 0;

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i].transform.childCount > 0)
            {
                objectnumber++;
            }

        }

        if (objectnumber == 1)
        {


            if (GameData.level==Puzzle.leveldata.Length)
            {
                OverMenü.SetActive(true);
                SuccesPanel.SetActive(false);
            }
            else
            {
                SuccesPanel.SetActive(true);
                GameData.GameState = "waiting";
            }
           
        }
    }
    private GameObject ClickSelect(){
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, Mathf.Infinity, mask);


        
        if (hit)
        {
          

            if (hit.transform.gameObject.tag == "puzzlecell")
            {

               // print("-----------------------------puzzle cell ");
                for (int i=0;i< hit.transform.gameObject.transform.parent.childCount;i++)
                {
                    SelectedCells.Add(hit.transform.gameObject.transform.parent.GetChild(i).gameObject);

                }
                return hit.transform.gameObject;

            }
            return hit.transform.gameObject;

        }
         return null;
    }
    private void CalculateConnectStation()
    {


        SetLine();
        List<Transform> AllCell = Allcells();
        for (int i = 0; i < SelectedCells.Count; i++)
        {



            


            PuzzleCellParents.Clear();
            List<char> wordvertical = new List<char>();
            List<char> wordhroizantal = new List<char>();
           /// print("selected : " + SelectedCells[i].GetComponent<PuzzleCellData>().c );
           
           

            String vert = String.Join("", FindVerticalWord(FindHeadVertical(SelectedCells[i], AllCell), AllCell, wordvertical).ToArray());

            String hort = String.Join("", FindHorizantalWord(FindHeadHorizantal(SelectedCells[i], AllCell), AllCell, wordhroizantal).ToArray());

            for (int a = 0; a < Puzzle.Words.Count; a++)
                {
                    if (Puzzle.Words[a] == vert  || Puzzle.Words[a] == hort)
                    {
                        Connect(SelectedCells[i].transform.parent.gameObject);

                        
                        Puzzle.Words.RemoveAt(a);
                        
                    }
             


            }

           
           


        }
    }
    private void Connect(GameObject selectedparent)
    {
        List<Transform> newchilds = new List<Transform>();

        foreach (GameObject puzzleparent  in PuzzleCellParents)
        {
          //  print("All  parents id : "+ puzzleparent.GetComponent<PuzzleparentData>().id);
           
                foreach (Transform c in puzzleparent.transform)
                {
                //print("child : " + c.GetComponent<PuzzleCellData>().c );
                foreach (Transform v in c.transform)
                    {
                        //print("tag  : " + v.name);
                        if (v.tag == "line")
                        {

                           
                            Destroy(v.gameObject);
                        }

                    }
                    newchilds.Add(c);

                }
            


        }
        

        for (int a = 0; a < newchilds.Count; a++)
        {

            newchilds[a].SetParent(selectedparent.transform);
        }
    }
    private GameObject FindHeadVertical(GameObject selected, List<Transform> Allcells)
    {

        foreach (Transform other in Allcells)
        {
            PuzzleCellData data = selected.GetComponent<PuzzleCellData>();
            PuzzleCellData newdata = other.GetComponent<PuzzleCellData>();
            if (data.ground_X == newdata.ground_X && data.ground_Y == newdata.ground_Y + 1
            &&
            data.Cell_x == newdata.Cell_x && data.Cell_y == newdata.Cell_y + 1)
            {

                // print("searcinggg...........new head " +other.GetComponent<PuzzleCellData>().c);
                return FindHeadVertical(other.gameObject, Allcells);

            }

        }

        return selected;

    }
    private List<Transform> Allcells()
    {
        List<Transform> others = new List<Transform>();
        others.Clear();
        foreach (Transform puzzleparent in PuzzleCellContainer.transform)
        {
           

                foreach (Transform i in puzzleparent.transform)
                {
                   
                    others.Add(i);
                }
            
        }

        return others;
    }
    private List<char> FindHorizantalWord(GameObject selected, List<Transform> Allcells, List<char> a)
    {
        a.Add(selected.GetComponent<PuzzleCellData>().c);

        if (!PuzzleCellParents.Contains(selected.transform.parent.gameObject))
        {
          
            PuzzleCellParents.Add(selected.transform.parent.gameObject);
        }

        foreach (Transform other in Allcells)
        {
            PuzzleCellData data = selected.GetComponent<PuzzleCellData>();
            PuzzleCellData newdata = other.GetComponent<PuzzleCellData>();
            if (data.ground_X + 1 == newdata.ground_X && data.ground_Y == newdata.ground_Y
            &&
            data.Cell_x + 1 == newdata.Cell_x && data.Cell_y  == newdata.Cell_y

            )
            {
                

                return FindHorizantalWord(other.gameObject, Allcells, a);

            }

        }

        return a;

    }
    private List<char> FindVerticalWord(GameObject selected, List<Transform> Allcells, List<char> a)
    {
        a.Add(selected.GetComponent<PuzzleCellData>().c);
        if (!PuzzleCellParents.Contains(selected.transform.parent.gameObject))
        {

            PuzzleCellParents.Add(selected.transform.parent.gameObject);
        }

        foreach (Transform other in Allcells)
        {
            PuzzleCellData data = selected.GetComponent<PuzzleCellData>();
            PuzzleCellData newdata = other.GetComponent<PuzzleCellData>();
            if (data.ground_X == newdata.ground_X && data.ground_Y + 1 == newdata.ground_Y
            &&
            data.Cell_x == newdata.Cell_x && data.Cell_y + 1 == newdata.Cell_y

            )
            {
                if (!PuzzleCellParents.Contains(other.parent.gameObject))
                {
                 //   print("the other id : "+ other.parent.gameObject.GetComponent<PuzzleparentData>().id);
                    PuzzleCellParents.Add(other.parent.gameObject);
                }
                
                return FindVerticalWord(other.gameObject, Allcells, a);

            }

        }

        return a;

    }
    private GameObject FindHeadHorizantal(GameObject selected,List<Transform> Allcells)
    {

        foreach (Transform other in Allcells)
        {
            PuzzleCellData data = selected.GetComponent<PuzzleCellData>();
            PuzzleCellData newdata = other.GetComponent<PuzzleCellData>();
            if (data.ground_X == newdata.ground_X + 1 && data.ground_Y == newdata.ground_Y
            &&
            data.Cell_x == newdata.Cell_x + 1 && data.Cell_y == newdata.Cell_y)
            {

               // print("searcinggg...........new head " +other.GetComponent<PuzzleCellData>().c);
                return FindHeadHorizantal(other.gameObject, Allcells);
               
            }
            
        }

        return selected;

    }
    private void SetLine()
    {


        List<Transform> othercell = othercells();

        for (int i = 0; i < SelectedCells.Count; i++)
        {

            foreach (Transform other in othercell)
            {
                GameObject selected = SelectedCells[i];
                PuzzleCellData data = selected.GetComponent<PuzzleCellData>();
                PuzzleCellData newdata = other.GetComponent<PuzzleCellData>();
                if (data.ground_X + 1 == newdata.ground_X && data.ground_Y == newdata.ground_Y)
                {

                    CreateLineVertical(GameData.Blocksize / 2, 0, selected, data);

                }
                if (data.ground_X == newdata.ground_X + 1 && data.ground_Y == newdata.ground_Y )
                {
                    CreateLineVertical(-GameData.Blocksize / 2, 0, selected, data);

                }
                if (data.ground_X == newdata.ground_X && data.ground_Y + 1 == newdata.ground_Y )
                {
                    CreateLineHorizantal(0, -GameData.Blocksize / 2, selected, data);
                    
                }
                if (data.ground_X == newdata.ground_X && data.ground_Y == newdata.ground_Y + 1)
                {
                    CreateLineHorizantal(0, GameData.Blocksize / 2, selected, data);
                 
                }
            }
            
        }
    }
    private void CreateLineHorizantal(float v1, float v2, GameObject selected, PuzzleCellData data)
    {
        GameObject child = new GameObject();
        child.tag = "line";
        child.transform.position = selected.transform.position + new Vector3(v1, v2, 0);
        child.AddComponent<RectTransform>();
        child.GetComponent<RectTransform>().sizeDelta = new Vector2(GameData.Blocksize, ReagentSize);
        child.AddComponent<Image>();
        child.GetComponent<Image>().color = GroundMatrix[data.ground_X, data.ground_Y].GetComponent<Image>().color;
        child.transform.SetParent(selected.transform);
    }
    private void CreateLineVertical(float v1, float v2,GameObject selected, PuzzleCellData data)
    {
        GameObject child = new GameObject();
        child.tag = "line";
        child.transform.position = selected.transform.position + new Vector3(v1, v2, 0);
        child.AddComponent<RectTransform>();
        child.GetComponent<RectTransform>().sizeDelta = new Vector2(ReagentSize, GameData.Blocksize);
        child.AddComponent<Image>();
        child.GetComponent<Image>().color = GroundMatrix[data.ground_X, data.ground_Y].GetComponent<Image>().color;
        child.transform.SetParent(selected.transform);
    }
    private List<Transform> othercells()
    {

        List<Transform> others = new List<Transform>();
        others.Clear();
        foreach (Transform puzzleparent in PuzzleCellContainer.transform)
        {
            if (puzzleparent.GetComponent<PuzzleparentData>().id!=SelectedCells[0].transform.parent.GetComponent<PuzzleparentData>().id)
            {

                foreach (Transform i in puzzleparent.transform)
                {
                   // print("others added : " + i.GetComponent<PuzzleCellData>().c);
                    others.Add(i);
                }
            }
        }

         return others;

    }
    private void MoveObject()
    {
        if (isBeingHeld == true  )
        {

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if (selectedobject != null && selectedobject.transform.parent != null  && selectedobject.tag == "puzzlecell")
            {
                selectedobject.transform.parent.transform.position = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
            }



            /*
             
         if (selectedobject != null && selectedobject.tag != "puzzlecell")
            {
                selectedobject.transform.position = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
            }*/

        }
    }

}
