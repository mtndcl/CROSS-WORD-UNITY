using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInput : MonoBehaviour
{

    private GameObject GameManager;

    private GameObject SuccesPanel;

    private GameObject HintPanel;

    private int x;

    private int y;

    private GameObject gamemenü;

    private GameObject overpanel;

    private GameObject gamescene;

    private Vector3 InitGameSceneLocation;

    private GameObject MyCamera;
   
    void Start()
    {
        MyCamera = GameObject.FindGameObjectWithTag("MainCamera");

        gamescene = GameObject.FindGameObjectWithTag("gamescene");

        overpanel = GameObject.FindGameObjectWithTag("over");

        gamemenü = GameObject.FindGameObjectWithTag("gamemenü");

        x = 0;

        y = 0;

        GameManager = GameObject.FindGameObjectWithTag("gamemanager");

        SuccesPanel = GameObject.FindGameObjectWithTag("succespanel");

        HintPanel = GameObject.FindGameObjectWithTag("hintpanel");

        InitGameSceneLocation = gamescene.transform.position;
    }
    private void Update()
    {


        ClickOnBackButton();
        
    }
    private void ClickOnBackButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gamemenü.SetActive(true);
            }
        }

        gamemenü.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "LEVEL : " + GameData.level.ToString() + " / " + Puzzle.leveldata.Length;
    }
    public void ClicktoStartGame()
    {
        MyCamera.GetComponent<Camera>().backgroundColor = GameData.color;
        gamemenü.SetActive(false);
      
        HintPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text= GameData.level.ToString() + " / " + Puzzle.leveldata.Length;
    }
    public void ClicktoBack()
    {

        gamemenü.SetActive(true);
    }
    public void ClicktoNextLevel()
    {

        gamescene.transform.localScale = new Vector3(1,1,1);

        GameData.Blocksize= GameData.Blocksize = GameManager.GetComponent<InitGame>().GroundMatrix[0,0].GetComponent<Image>().rectTransform.rect.width;

        MyCamera.GetComponent<Camera>().backgroundColor = GameData.color;

        gamescene.transform.position = InitGameSceneLocation;

        ResetColor();

        InvokeRepeating("ResetGroundColor", 0, 0.0005f);

        x = 0;

        y = 0;
       
        DeletePuzzleCell();
       
        GameData.level++;

        GameData.GameState = "creating";

        Puzzle.SetQuestion(HintPanel);

        HintPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.level.ToString() + " / " + Puzzle.leveldata.Length;

        GameManager.GetComponent<InitGame>().SetGame();

        SuccesPanel.SetActive(false);

    }
    private void ResetColor()
    {
        GameData.color.r = UnityEngine.Random.Range(0, 255f)/255f;
        GameData.color.g = UnityEngine.Random.Range(0, 255f)/255f;
        GameData.color.b = UnityEngine.Random.Range(0, 255f)/255f;
    }
    private void DeletePuzzleCell()
    {
        GameObject container = GameObject.FindGameObjectWithTag("containerpuzzlecell");


        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void ResetGroundColor()
    {
            GameObject[,] groundmatrix = GameManager.GetComponent<InitGame>().GroundMatrix;

            float rr = Random.Range(0.0f, 0.5f);

            groundmatrix[x, y].GetComponent<Image>().color =new Color(GameData.color.r + rr, GameData.color.g + rr, GameData.color.b  + rr);
      
          if (x < groundmatrix.GetLength(0))
          {
                     x++;
                    if (x==groundmatrix.GetLength(0))
                    {
                        x = 0;
                        y++;
                    }
                    if (y== groundmatrix.GetLength(1)) {

                        y = 0;
                        CancelInvoke("ResetGroundColor");
                    }
           }
    }
    public void ClickonQuit()
    {
        Application.Quit();
    }
    public void ClickbacktoMenü()
    {
        SuccesPanel.SetActive(true);
        GameData.level = 0;
        overpanel.SetActive(false);
        gamemenü.SetActive(true);
        ClicktoNextLevel();

    }
}
