using UnityEngine;
using UnityEngine.UI;

namespace ngroDefine
{
    [System.Serializable] public class  MyRoomStatusBaff
    {//何パーセントバフがかかるか
        public int simple;//+エゴー協調-
        public int shape;//自立ー依存
        public int classic; //外交的ー内向的
        public int gaming;//直観ー理性
        public int japaneseTaste;//温厚ー好戦的
    }

    [System.Serializable] public class PersonalityStatus
    {
        public int egoist;//100エゴー協調0 シンプルでエゴ。思考。
        public int independence;//100自立ー依存0 シャープで自立。好戦的。
        public int diplomatic; //100外交的ー内向的0 クラシックで外交的。協調。
        public int intuition;//100直観ー思考0 ゲーミングで直観。依存。
        public int Gentleness;//100温厚ー好戦的0 和風で温厚。内向的。
    }
}

public class EnumDefinition
{
    public enum BGs
    {
        ReoRoomMorning,
        ReoRoomNight,
        ScoolMorning,
        SchoolEvening,
        RiverSideMorning,
        RiverSideEvening,
        SchoolLoadMorning,
        SchoolLoadEvening,
        NONE,
    }

    // このゲームで使う汎用Enum
    public enum Characters
    {
        NONE,
        凪,
        玲王,
        ばあや,
        舐岡,
        小さい凪,
    }

   public enum CharacterBody//上に追加しちゃダメ。崩れる
    {
        SchoolNagiNormal,
        SchoolNagiHuon,
        SchoolNagiOdoroki,
        SchoolReoNormal,
        SchoolReoSmile,
        SchoolReoOdoroki,
        BayaCloseEye,
        BayaOpenEye,
        NagiUniform,
        ReoUniform,
        NameokaNormal,
        NameokaOdoroki,
        MiniNagi,
        NONE,
    }

    public enum NagiEvolutionType
    {
        None,//凪が変わらない
        Normal,
        Dog,
        Cat,
        SportsMan,
    }

    public enum NagiMotionType
    {
        Sit,
        Catch,
        Bath,
    }

    public enum ItemType
    {
        ConsumptionItem,
        WallItem,
        FloorItem,
        FurnitureItem,

        InteriorItem,//WallFloorFurniture
        AllItem,//上記すべてのタイプを扱う
    }
}
 