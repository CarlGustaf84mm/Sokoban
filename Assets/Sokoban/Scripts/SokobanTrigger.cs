using System.Collections;
using UnityEngine;

public class SokobanTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            GameManager.TargetsAchived++;
            Debug.Log("箱をパレットに載せる(合計 " + GameManager.TargetsAchived.ToString() + 
                "自分" + GameManager.TargetsToWin.ToString() + " )");
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            GameManager.TargetsAchived--;
            Debug.Log("パレットから取り出した箱（合計 " + GameManager.TargetsAchived.ToString() + "из " + GameManager.TargetsToWin.ToString() + " )");
        }
    }
}

