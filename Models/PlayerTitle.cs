namespace ValoStats.Models;

public class TitleData
{
    public string uuid { get; set; }
    public string displayName { get; set; }
    public string titleText { get; set; }
}

public class TitleResponse
{
    public int status { get; set; }
    public TitleData data { get; set; }
}