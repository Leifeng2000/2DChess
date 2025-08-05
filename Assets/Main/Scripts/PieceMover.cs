using UnityEngine;
using System.Collections;

public class PieceMover : MonoBehaviour 
{
    public LuxChess2D manager;
    public int index;
    private Transform piece;

    private bool isDragging = false;

    public void Update(){
        if(isDragging){
            piece.transform.position = Input.mousePosition;
        }
    }


    public void OnMouseDownEvent() 
    {
        if (manager.GetPieceAt(index) != Defs.Empty)
        {
            GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[4]);
            
            piece = transform.GetChild(0);
            piece.SetParent(transform.parent, true);

            manager.DragingFrom = index;
            isDragging = true;
        }
    }



    public void OnMouseUpEvent() 
    {

        GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[5]);

        if (!isDragging)
            return;

        int x = Mathf.Abs(Mathf.RoundToInt((-224 - piece.transform.localPosition.x) / 64));
        int y = 7 - Mathf.Abs(Mathf.RoundToInt((-224 - piece.transform.localPosition.y) / 64));
        
        int myindex = x + y * 8;

        if (manager.TryToMove(manager.DragingFrom, myindex))
        {
            MovePiece(manager.SquaresObj[myindex]);
            manager.UpdateBoard();

        }
        else
            ReturnPiece();

        manager.DragingFrom = -1;
        isDragging = false;

    }

    private void ReturnPiece() {
        piece.SetParent(manager.SquaresObj[manager.DragingFrom], false);
        piece.localPosition = Vector3.zero;
    }

    private void MovePiece(Transform to) {

        var t = to.GetChild(0);
        if (t) 
        {
            t.SetParent(transform, true);
            t.localPosition = Vector3.zero;
        }
        piece.SetParent(to, true);
        piece.localPosition = Vector3.zero;

    }
}
