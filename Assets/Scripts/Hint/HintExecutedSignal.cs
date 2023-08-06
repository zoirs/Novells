public class HintExecutedSignal {
    private bool _executed;

    public HintExecutedSignal(bool executed) {
        _executed = executed;
    }

    public bool Executed => _executed;
}