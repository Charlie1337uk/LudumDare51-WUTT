using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public Camera _cam;
    public GameObject SelectedPiece;
    public GameManager GM;
    public List<GameObject> Selections;
    public bool CanClick;
    private void Start()
    {
        GM = GetComponent<GameManager>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&CanClick)
        {
            Ray _ray = _cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.GetComponent<Item>())
                {
                    SelectedPiece = hitInfo.collider.gameObject;
                    if (Selections.Count != 0)
                    {
                        foreach (var item in Selections)
                        {
                            item.GetComponent<HighlightPiece>().DefaultColor();
                        }
                    }
                    Selections.Clear();
                    Selections.Add(hitInfo.collider.gameObject);
                    Selections[0].GetComponent<HighlightPiece>().SetYellow();
                }
            }
        }
    }
    public void ResetColourClearList()
    {
        foreach (var item in Selections)
        {
            item.GetComponent<HighlightPiece>().DefaultColor();
        }
        Selections.Clear();
    }
}
