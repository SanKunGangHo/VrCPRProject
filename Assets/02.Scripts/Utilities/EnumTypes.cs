namespace EnumTypes
{
    public enum SceneName // Scene 이름 종류
    {
        StartScene = 0,
        StationSingleScene,
        StationMultiScene,
        SubwaySingleScene,
        SubwayMultiScene,
        None
    }

    public enum SelectPlace // 장소 종류
    {
        Station = 0,
        Subway,
        None
    }

    // 
    public enum SelectPeople // 인원 수
    {
        Single = 1,
        Multi = 2,
        None
    }

    public enum IActionType // 상호작용 종류
    {
        None
    }
}