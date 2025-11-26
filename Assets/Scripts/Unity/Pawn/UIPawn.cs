using LudoGames.Interface.Pawns;
using LudoGames.Unity.Boards;
using UnityEngine;

namespace LudoGames.Unity.Pawns
{
    class UIPawn : MonoBehaviour
    {
        public IPawn pawn;
        

        public void MovePawn(TileManager tileManager)
        {
            var uiTIle =  tileManager.GetUITile(pawn.Coordinate);
            
            if(uiTIle)
            {
                transform.position = uiTIle.transform.position;
                Debug.Log("Test");
            }
        }
    }
    
}