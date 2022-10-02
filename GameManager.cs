using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Animator teapotAnim;
    public GameObject Panel;
    public List<GameObject> playObjects;
    public List<Transform> Spaces;
    public List<GameObject> PiecesInPlay;
    public int Level = 1;
    public int randomObj;
    public int randomSpot;
    public float randomRot;
    public Quaternion rQuat;
    [SerializeField] private bool Countdown;
    public int GameState;
    public int Lives = 3;
    public int newObj;
    public bool NotFirstTurn;
    public GameObject PieceSelection;
    public PoissonPointsToTransforms _poissonTransforms;
    public bool waitforanim;
    public TMP_Text UIText;
    public GameObject Playbutton;
    public GameObject ContinueButton;
    public int Score = 2;
    public int Items_Remembered;
    public Clock _clock;
    public PieceSelector _pieceSelector;
    public bool FastMode;
    public float time1;
    public float time2;
    public bool PoissonTest = true;
    private void Start()
    {
        StartCoroutine(DelayedFill());   
    }
    IEnumerator DelayedFill()
    {
        yield return new WaitForSecondsRealtime(2);
        FillSpaces();
        yield return null;
    }
    private void FillSpaces()
    {
        foreach (var Space in _poissonTransforms.PlaySpaces)
        {
            //GameObject nGO = new GameObject();
            Spaces.Add(Space.transform);            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (FastMode) { time1 = 2f; time2 = 5f; } else { time1 = 8f; time2 = 10f; }
        if (waitforanim)
        {
            if (teapotAnim.GetCurrentAnimatorStateInfo(0).IsTag("hiding")) { waitforanim = false; StartCoroutine(SpawnNewPiece()); }
        }
    }
    public void AddObject() 
    {
        HideItems();
        waitforanim = true;
        
    }
    IEnumerator SpawnNewPiece()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnPiece();
    }
    private void Poisson_Test()
    {
        if (PoissonTest)
        {
            for (int i = 0; i < Spaces.Count; i++)
            {
                var newItemT = Instantiate(playObjects[randomObj], Spaces[randomSpot].position, rQuat);
                newItemT.transform.parent = null;
                newItemT.GetComponent<Item>().ID = newObj;
                PiecesInPlay.Add(newItemT);
                Spaces.Remove(Spaces[randomSpot]);
                RandomiseValues();
            }
            return;
        }
    }
    private void SpawnPiece()
    {

        var newItem = Instantiate(playObjects[randomObj], Spaces[randomSpot].position, rQuat);
        newItem.transform.parent = null;
        newItem.GetComponent<Item>().ID = newObj;
        PiecesInPlay.Add(newItem);
        Spaces.Remove(Spaces[randomSpot]);
        if (Spaces.Count == 0) { Panel.SetActive(true); UIText.text = "Well done, you've been paying attention. You remembered "+Score+" objects. Thanks for playing! Dont forget to vote for this Ludum Dare 51 submission. That is the end of the game in its current state"; Time.timeScale = 0; }
        RandomiseValues();
        StartCoroutine(ThreeSeconds());
    }
    public void RandomiseValues()
    {
        randomObj = Random.Range(0, playObjects.Count);
        randomSpot = Random.Range(0, Spaces.Count);
        randomRot = Random.Range(-0.18f, 0.18f) * 100;
        rQuat.eulerAngles = new Vector3(-90, randomRot, 0);
    }
    public void StartPlay()
    {
        RandomiseValues();
        Playbutton.SetActive(false);
        Panel.SetActive(false);
        AddObject();
        _clock.enabled = true;
    }
    public void ContinuePlay()
    {
        ContinueButton.SetActive(false);
        Panel.SetActive(false);
        newObj += 1;
        AddObject();
    }
    public void ShowItems()
    {
        _pieceSelector.CanClick = true;
        _pieceSelector.ResetColourClearList();
        ResetAllPlayPieceColor();
        AllowPieceSelection();
        teapotAnim.SetTrigger("showItems");
        Countdown = true;
        StartCoroutine(TenSeconds());
    }
    public void HideItems()
    {
        _pieceSelector.CanClick = true;
        teapotAnim.SetTrigger("hideItems");
        _pieceSelector.ResetColourClearList();
        ResetAllPlayPieceColor();
    }
    IEnumerator ThreeSeconds()
    {
        yield return new WaitForSecondsRealtime(time1);
        ShowItems();
        yield return null;
    }
    IEnumerator TenSeconds()
    {
        yield return new WaitForSecondsRealtime(time2);
        _pieceSelector.CanClick = false;
        if (Countdown && NotFirstTurn) { Countdown = false; if (!_pieceSelector.SelectedPiece) { LoseLife(); } else { MakeGuess(_pieceSelector.SelectedPiece.GetComponent<Item>().ID); } }
        if (!NotFirstTurn) { NotFirstTurn = true; Countdown = false; newObj += 1; AddObject(); }
        yield return null; 
    }
    public void LoseLife()
    {        
        foreach (var item in PiecesInPlay)
        {
            item.GetComponent<HighlightPiece>().selected = true;
            item.GetComponent<HighlightPiece>().DefaultColor();
        }
        PiecesInPlay[newObj].GetComponent<HighlightPiece>().SetGreen();
        Lives -= 1;
        Debug.Log("Life -1");
        Panel.SetActive(true);
        ContinueButton.SetActive(true);
        UIText.text = "Time's up! You lost a life";
        if(Lives <= 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        Debug.Log("GameOver");
        Panel.SetActive(true);
        ContinueButton.SetActive(false);
        Items_Remembered = Score;
        UIText.text = "Game Over! Items Remembered: " +Items_Remembered;
    }
    public void MakeGuess(int ObjNo)
    {
        foreach (var item in PiecesInPlay)
        {
            item.GetComponent<HighlightPiece>().selected = true;
            item.GetComponent<HighlightPiece>().DefaultColor();
        }
        Countdown = false;
        if (ObjNo == newObj) 
        { 
            _pieceSelector.SelectedPiece.GetComponent<HighlightPiece>().SetGreen();
            Debug.Log("Score");
            Panel.SetActive(true);
            UIText.text = "Correct!";
            Score += 1;
            StartCoroutine(Correct());
        }
        else { WrongGuess(); }
    }
    IEnumerator Correct()
    {        
        yield return new WaitForSecondsRealtime(3);        
        ContinuePlay();
        yield return null;
    }
    public void WrongGuess()
    {
        _pieceSelector.SelectedPiece.GetComponent<HighlightPiece>().SetRed();
        PiecesInPlay[newObj].GetComponent<HighlightPiece>().SetGreen();
        Lives -= 1;
        Debug.Log("Life -1");
        Panel.SetActive(true);
        ContinueButton.SetActive(true);
        UIText.text = "Incorrect! You lost a life";
        if (Lives <= 0)
        {
            GameOver();
        }
    }
    public void ResetAllPlayPieceColor()
    {
        _pieceSelector.SelectedPiece = null;
        foreach (var item in PiecesInPlay)
        {
            item.GetComponent<HighlightPiece>().DefaultColor();
        }
    }
    public void AllowPieceSelection()
    {
        foreach (var item in PiecesInPlay)
        {
            item.GetComponent<HighlightPiece>().selected = false;
        }
    }
}
