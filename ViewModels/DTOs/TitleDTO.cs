using ValoStats.Models;

namespace ValoStats.ViewModels.DTOs;

public class TitleDTO
{
    public static TitleData TitleResponseToTitleData(TitleResponse Response)
    {
        return new TitleData()
        {
            uuid = Response.data.uuid,
            displayName = Response.data.displayName,
            titleText = Response.data.titleText
        };
    }
}