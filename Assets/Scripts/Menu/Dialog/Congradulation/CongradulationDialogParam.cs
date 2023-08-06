using UnityEngine.Events;

public class CongradulationDialogParam : ConfirmDialogParam {
    
    private int railwayCount;
    private int wagonCount;
    private int reward;

    public CongradulationDialogParam(string body, int railwayCount, int wagonCount, int reward, UnityAction yes, UnityAction no) : base(body, yes, no) {
        this.railwayCount = railwayCount;
        this.wagonCount = wagonCount;
        this.reward = reward;
    }

    public CongradulationDialogParam(string header, int railwayCount, int wagonCount, int reward, string body, UnityAction yes, UnityAction no,
        string yesText, string noText)
        : base(header, body, yes, no, yesText, noText) {
        this.railwayCount = railwayCount;
        this.wagonCount = wagonCount;
        this.reward = reward;
    }

    public int Reward => reward;

    public int RailwayCount => railwayCount;

    public int WagonCount => wagonCount;
}