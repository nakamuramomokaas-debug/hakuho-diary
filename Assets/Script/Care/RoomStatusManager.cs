using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ngroDefine;

public class RoomStatusManager : MonoBehaviour
{
    // //宣言
    // [System.Serializable] public struct MyRoomStatusBaff
    // {//何パーセントバフがかかるか
    //     public int simple;//+エゴー協調-
    //     public int shape;//自立ー依存
    //     public int classic; //外交的ー内向的
    //     public int gaming;//直観ー理性
    //     public int japaneseTaste;//温厚ー好戦的
    // }
    [SerializeField] MousePointer mousePointer;
    [SerializeField] MyData myDataScr;
    [SerializeField] TextMeshProUGUI simpleText;
    [SerializeField] TextMeshProUGUI shapeText;
    [SerializeField] TextMeshProUGUI classicText;
    [SerializeField] TextMeshProUGUI gamingText;
    [SerializeField] TextMeshProUGUI japaneseTasteText;
    [SerializeField] GameObject simpleUpParticle;
    [SerializeField] GameObject shapeUpParticle;
    [SerializeField] GameObject classicUpParticle;
    [SerializeField] GameObject gamingUpParticle;
    [SerializeField] GameObject japaneseTasteUpParticle;
    [SerializeField] GameObject uiFurnitureCanvas;
    [SerializeField] Item startWallItem;//prefab
    [SerializeField] Item startFloorItem;

    private int nowSimple = 0;
    private int nowShape = 0;
    private int nowClassic = 0;
    private int nowGaming = 0;
    private int nowJapanese = 0;

    private MyRoomStatusBaff nowWallStatus;
    private MyRoomStatusBaff nowFloorStatus;

    //シーンが読み込まれるたびに呼ばれる
    void Awake() 
    {
        if(myDataScr.GetDataInt("simpleText") == -99999) nowSimple = 0;
        else nowSimple = myDataScr.GetDataInt("simpleText");
        if(myDataScr.GetDataInt("shapeText") == -99999) nowShape = 0;
        else nowShape = myDataScr.GetDataInt("shapeText");
        if(myDataScr.GetDataInt("classicText") == -99999) nowClassic = 0;
        else nowClassic = myDataScr.GetDataInt("classicText");
        if(myDataScr.GetDataInt("gamingText") == -99999) nowGaming = 0;
        else nowGaming = myDataScr.GetDataInt("gamingText");
        if(myDataScr.GetDataInt("japaneseTasteText") == -99999) nowJapanese = 0;
        else nowJapanese = myDataScr.GetDataInt("japaneseTasteText");

        simpleText.text = nowSimple.ToString(); 
        shapeText.text = nowShape.ToString(); 
        classicText.text = nowClassic.ToString(); 
        gamingText.text = nowGaming.ToString(); 
        japaneseTasteText.text = nowJapanese.ToString(); 

        nowWallStatus = startWallItem.GetMyRoomStatusBaff();
        nowFloorStatus = startFloorItem.GetMyRoomStatusBaff();
    }

    public int[] GetRoomStatus()
    {
        var num = new int[5];
        num[0] = nowSimple;
        num[1] = nowShape;
        num[2] = nowClassic;
        num[3] = nowGaming;
        num[4] = nowJapanese;
        return num;
    }

    public void PutWall(MyRoomStatusBaff wallBaff)
    {
        Debug.Log("wall" + wallBaff.simple);
        nowSimple -= nowWallStatus.simple;
        nowShape -= nowWallStatus.shape;
        nowClassic -= nowWallStatus.classic;
        nowGaming -= nowWallStatus.gaming;
        nowJapanese -= nowWallStatus.japaneseTaste;

        nowSimple += wallBaff.simple;
        nowShape += wallBaff.shape;
        nowClassic += wallBaff.classic;
        nowGaming += wallBaff.gaming;
        nowJapanese += wallBaff.japaneseTaste;

        simpleText.text = nowSimple.ToString(); 
        shapeText.text = nowShape.ToString(); 
        classicText.text = nowClassic.ToString(); 
        gamingText.text = nowGaming.ToString(); 
        japaneseTasteText.text = nowJapanese.ToString(); 
        nowWallStatus = wallBaff;
    }

    public void PutFloor(MyRoomStatusBaff floorBaff)
    {
        Debug.Log("floor"+ floorBaff.simple);
        nowSimple -= nowFloorStatus.simple;
        nowShape -= nowFloorStatus.shape;
        nowClassic -= nowFloorStatus.classic;
        nowGaming -= nowFloorStatus.gaming;
        nowJapanese -= nowFloorStatus.japaneseTaste;
        
        nowSimple += floorBaff.simple;//nowとnew注意！
        nowShape += floorBaff.shape;
        nowClassic += floorBaff.classic;
        nowGaming += floorBaff.gaming;
        nowJapanese += floorBaff.japaneseTaste;

        simpleText.text = nowSimple.ToString(); 
        shapeText.text = nowShape.ToString(); 
        classicText.text = nowClassic.ToString(); 
        gamingText.text = nowGaming.ToString(); 
        japaneseTasteText.text = nowJapanese.ToString(); 

        nowFloorStatus = floorBaff;
    }

    public void PutFurniture(MyRoomStatusBaff roomStatus)
    {
        if(roomStatus.simple > 0) 
        {
            var pos = new Vector3(-3.25f, -4f, 0f);
            var obj = Instantiate(simpleUpParticle, uiFurnitureCanvas.transform, false);
            obj.transform.position += pos;
        }
        nowSimple += roomStatus.simple;
        simpleText.text = nowSimple.ToString(); 
        //myDataScr.SetDataInt("simpleText", nowSimple);

        if(roomStatus.shape > 0) 
        {
            var pos = new Vector3(-1.5f, -4f, 0f);
            var obj = Instantiate(shapeUpParticle, uiFurnitureCanvas.transform, false);
            obj.transform.position += pos;
        }
        nowShape += roomStatus.shape;
        shapeText.text = nowShape.ToString(); 
        //myDataScr.SetDataInt("shapeText", nowShape);
    
        if(roomStatus.classic > 0) 
        {
            var pos = new Vector3(0f, -4f, 0f);
            var obj = Instantiate(classicUpParticle, uiFurnitureCanvas.transform, false);
            obj.transform.position += pos;
        }
        nowClassic += roomStatus.classic;
        classicText.text = nowClassic.ToString(); 
        //myDataScr.SetDataInt("classicText", nowClassic);
        
        if(roomStatus.gaming > 0)
        {
            var pos = new Vector3(1.5f, -4f, 0f);
            var obj = Instantiate(gamingUpParticle, uiFurnitureCanvas.transform, false);
            obj.transform.position += pos;
        }
        nowGaming += roomStatus.gaming;
        gamingText.text = nowGaming.ToString(); 
        //myDataScr.SetDataInt("gamingText", nowGaming);

        if(roomStatus.japaneseTaste > 0) 
        {
            var pos = new Vector3(3.25f, -4f, 0f);
            var obj = Instantiate(japaneseTasteUpParticle, uiFurnitureCanvas.transform, false);
            obj.transform.position += pos;
        }
        nowJapanese += roomStatus.japaneseTaste;
        japaneseTasteText.text = nowJapanese.ToString(); 
        //myDataScr.SetDataInt("japaneseTasteText", nowJapanese);
    }

    public void RestoreFurniture(MyRoomStatusBaff roomStatus)
    {
        nowSimple -= roomStatus.simple;
        simpleText.text = nowSimple.ToString(); 
        //myDataScr.SetDataInt("simpleText", nowSimple);
    
        nowShape -= roomStatus.shape;
        shapeText.text = nowShape.ToString(); 
        //myDataScr.SetDataInt("shapeText", nowShape);
    
        nowClassic -= roomStatus.classic;
        classicText.text = nowClassic.ToString(); 
        //myDataScr.SetDataInt("classicText", nowClassic);
        
        nowGaming -= roomStatus.gaming;
        gamingText.text = nowGaming.ToString(); 
        //myDataScr.SetDataInt("gamingText", nowGaming);

        nowJapanese -= roomStatus.japaneseTaste;
        japaneseTasteText.text = nowJapanese.ToString(); 
        //myDataScr.SetDataInt("japaneseTasteText", nowJapanese);
    }
}
