using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroy : MonoBehaviour
{
    //メインカメラに映らなくなった瞬間の処理
    private void OnBecameInvisible()
    {
        //メインカメラに映らなくなったアイテムを破棄
        GameObject.Destroy(this.gameObject);
    }
}
