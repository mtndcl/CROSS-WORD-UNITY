using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InitGame : MonoBehaviour
{

    private GameObject Canvas;

    private GameObject GroundCell;

    private GameObject PuzzleCell;

    public GameObject[,] GroundMatrix;

    private GameObject PuzzleCellParent;

    private GameObject ContainerPuzzleCell;

    private GameObject ContainerGroundCell;

    private GameObject HintPanel;

    private GameObject PuzzleCellContainer;

    private float blocksize;

    private GameObject Gamemenü;

    public GameObject BoundsParents;

    private GameObject OverMenü;
    void Start()
    {
        OverMenü = GameObject.FindGameObjectWithTag("over");

        OverMenü.SetActive(false);

        PuzzleCellContainer = GameObject.FindGameObjectWithTag("containerpuzzlecell");

        Canvas = GameObject.FindGameObjectWithTag("canvas");

        GroundCell = GameObject.FindGameObjectWithTag("groundcell");

        ContainerPuzzleCell = GameObject.FindGameObjectWithTag("containerpuzzlecell");

        ContainerGroundCell = GameObject.FindGameObjectWithTag("containergroundcell");

        PuzzleCellParent = GameObject.FindGameObjectWithTag("puzzlecellparent");

        PuzzleCell = GameObject.FindGameObjectWithTag("puzzlecell");

        HintPanel = GameObject.FindGameObjectWithTag("hintpanel");

        Gamemenü = GameObject.FindGameObjectWithTag("gamemenü");

        Gamemenü.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "LEVEL : " + GameData.level.ToString() + " / " + Puzzle.leveldata.Length;

        GameData.Blocksize = Canvas.GetComponent<RectTransform>().rect.width/20;

        blocksize = GameData.Blocksize;

        PuzzleCell.GetComponent<RectTransform>().sizeDelta=new Vector2 (blocksize ,blocksize);

        PuzzleCell.GetComponent<BoxCollider2D>().size = new Vector2(blocksize*7/8, blocksize * 7 / 8);

        GroundCell.GetComponent<RectTransform>().sizeDelta = new Vector2(blocksize, blocksize);

        GroundCell.GetComponent<CircleCollider2D>().radius = blocksize / 20;

        BoundsParents = GameObject.FindGameObjectWithTag("bound");

        BoundsParents.transform.GetChild(0).transform.position=new Vector3(0,Canvas.GetComponent<RectTransform>().rect.height);
        BoundsParents.transform.GetChild(1).transform.position = new Vector3(Canvas.GetComponent<RectTransform>().rect.width,0 );

        CreateGround();
        
        Puzzle.SetQuestion(HintPanel);


       
        
        SetGame();
      
    }
    public void SetGame()
    {
        Invoke("CreatePuzzleCells", 2);

    }
    public void CreatePuzzleCells()
    {

        int id = 0;

        int cellid = 0;

        float xlocation = Canvas.GetComponent<RectTransform>().rect.width / 4+blocksize/2;

        float ylocatioon = Canvas.GetComponent<RectTransform>().rect.height - HintPanel.GetComponent<RectTransform>().rect.height*2 +blocksize / 2;

        for (int i = 0; i < Puzzle.sections.Count; i++)
        {
            GameObject puzzleparent = Instantiate(PuzzleCellParent, new Vector2(xlocation, ylocatioon), Quaternion.identity, ContainerPuzzleCell.transform);

            puzzleparent.name = "PuzzleParent " + i.ToString();

            if (xlocation+ Canvas.GetComponent<RectTransform>().rect.width / 4 < Canvas.GetComponent<RectTransform>().rect.width*3/4)
            {
                xlocation += Canvas.GetComponent<RectTransform>().rect.width /3;

            }
            else
            {

                xlocation = Canvas.GetComponent<RectTransform>().rect.width / 4 + blocksize / 2;
                ylocatioon -= Canvas.GetComponent<RectTransform>().rect.height/4;
            }

            puzzleparent.GetComponent<PuzzleparentData>().id = id++;

            for (int j = 0; j < Puzzle.sections[i].Part.Count; j++)
            {
               
         
                float x = Puzzle.sections[i].Part[j].X - Puzzle.sections[i].Min_X;
                float y = Puzzle.sections[i].Part[j].Y - Puzzle.sections[i].Min_Y;

                GameObject cell = Instantiate(PuzzleCell, new Vector2((x) * blocksize, (-y) * blocksize), Quaternion.identity, puzzleparent.transform);
                cell.transform.localPosition = new Vector2((x) * blocksize, (-y) * blocksize);

                
                cell.GetComponent<PuzzleCellData>().c = Puzzle.sections[i].Part[j].Key;
                cell.GetComponent<PuzzleCellData>().id = cellid++;
                cell.GetComponent<PuzzleCellData>().Cell_x = Puzzle.sections[i].Part[j].X;
                cell.GetComponent<PuzzleCellData>().Cell_y = Puzzle.sections[i].Part[j].Y;


                cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Puzzle.sections[i].Part[j].Key.ToString();
            }
        }

        Invoke("FitOnGround", 0.1f);
        GameData.GameState = "playing";
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
    private void CreateGround()
    {
        float heigth = Canvas.GetComponent<RectTransform>().rect.height;
        float width = Canvas.GetComponent<RectTransform>().rect.width;

        int count = 0;

        GroundMatrix = new GameObject[(int)(width / blocksize + 2), (int)(heigth / blocksize + 2)];

        int x_axis = 0;
        int y_axis = 0;

        for (float y = heigth/2 ; y >= -heigth / 2 - blocksize; y -= blocksize)
        {
            for (float x = -width / 2; x <= width / 2 + blocksize; x += blocksize)
            {


                GameObject ground = Instantiate(GroundCell, new Vector2(x+Canvas.GetComponent<RectTransform>().rect.width/2+blocksize/2, y+Canvas.GetComponent<RectTransform>().rect.height / 2- blocksize / 2), Quaternion.identity, ContainerGroundCell.transform);
                
                ground.GetComponent<GrounCellData>().Ground_x = x_axis;
                ground.GetComponent<GrounCellData>().Ground_y = y_axis;
              
                GroundMatrix[x_axis, y_axis] = ground;

                x_axis++;
     
                count++;

                float rr = Random.Range(0.0f, 0.5f);
              
                ground.GetComponent<Image>().color = new Color(GameData.color.r + rr, GameData.color.g  + rr, GameData.color.b + rr);

            }
            y_axis++;
            x_axis = 0;
        }

       // GameData.x =GroundMatrix[0,0].transform.position.x;
        //GameData.y = GroundMatrix[0, 0].transform.position.y;

    }
}



