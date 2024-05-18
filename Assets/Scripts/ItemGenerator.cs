using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //conePrefabを入れる
    public GameObject conePrefab;
    //スタート地点
    private int startPos = 40;
    //ゴール地点
    private int goalPos = 360;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    //unitychanのゲームオブジェクトName
    private const string unitychanName = "unitychan";
    //unitychanゲームオブジェクト
    private GameObject unitychan;
    //アイテムを更新する位置
    private int updateItemPosZ = 55;
    //現在のアイテムの最後尾位置
    private int currentMostTailItemPosZ = 90;
    //指定位置ごとにアイテムを生成するための位置
    private const int generateItemDiffPosZ = 15;

    const float coneDiffX = 0.4f;
    //乱数の最小・最大範囲
    const int randomMin = 0;
    const int randomMax = 10;
    //アイテムを置くZ座標のオフセットを決める乱数の範囲
    const int itemOffsetZMin = -5;
    const int itemOffsetZMax = 6;

    //アイテム生成確率
    private int[] generateItemProb = { 3,3,1,1,1,1,1,1,2,2,2};
    //アイテムのカテゴリ
    private enum ItemCategory { NONE = 0, COIN = 1, CAR = 2, CONE = 3};
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGenerateItem();
    }

    //初期化
    void Init()
    {
        //unitychanのゲームオブジェクト取得
        this.unitychan = GameObject.Find(unitychanName);
        //アイテムを一定範囲に初期配置
        for (int i = startPos; i < this.currentMostTailItemPosZ; i += generateItemDiffPosZ)
        {
            GenerateItem(i);
        }
    }
    //アイテムゲームオブジェクトを生成する
    void GenerateItemObject(GameObject obj, float x, float z)
    {
        GameObject item = Instantiate(obj);
        item.transform.position = new Vector3(x, item.transform.position.y, z);
    }
    //ランダムにアイテムを指定位置に生成する
    void GenerateItem(int diff)
    {
        //どのアイテムを出すのかをランダムに設定
        int num = Random.Range(randomMin, randomMax);
        if (IsGenerateItemCone(num))
        {
            //コーンをx軸方向に一直線に生成
            for (float j = -1; j <= 1; j += coneDiffX)
            {
                GenerateItemObject(conePrefab, 4 * j, diff);
            }
        }
        else
        {
            //レーンごとにアイテムを生成
            for (int j = -1; j <= 1; j++)
            {
                //アイテムの種類を決める
                int item = Random.Range(randomMin, randomMax);
                //アイテムを置くZ座標のオフセットをランダムに設定
                int offsetZ = Random.Range(itemOffsetZMin, itemOffsetZMax);
                //60%コイン配置:30%車配置:10%何もなし
                if (IsGenerateItemCoin(generateItemProb[item]))
                {
                    //コインを生成
                    GenerateItemObject(coinPrefab, posRange * j, diff + offsetZ);
                }
                else if (IsGenerateItemCar(generateItemProb[item]))
                {
                    //車を生成
                    GenerateItemObject(carPrefab, posRange * j, diff + offsetZ);
                }
            }
        }
    }
    //コーンの生成判定
    bool IsGenerateItemCone(int item)
    {
        if ((int)ItemCategory.CONE != item) return false;
        return true;
    }
    //コインの生成判定
    bool IsGenerateItemCoin(int item)
    {
        if ((int)ItemCategory.COIN != item) return false;
        return true;
    }
    //車の生成判定
    bool IsGenerateItemCar(int item)
    {
        if ((int)ItemCategory.CAR != item) return false;
        return true;
    }

    //アイテム生成の更新判定
    bool IsUpdateGenerateItem(int currentPos)
    {
        if (currentPos <= updateItemPosZ) return false;
        return true;
    }
    //アイテム生成を更新する
    void UpdateGenerateItem()
    {
        //アイテム生成を更新判定
        if (!IsUpdateGenerateItem((int)unitychan.transform.position.z)) return;
        this.currentMostTailItemPosZ += generateItemDiffPosZ;
        //アイテム生成位置がゴールよりも奥に配置されるか判定
        if (this.goalPos <= this.currentMostTailItemPosZ) return;
        GenerateItem(currentMostTailItemPosZ);
    }
}
