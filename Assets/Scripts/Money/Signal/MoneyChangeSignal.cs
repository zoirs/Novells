public class MoneyChangeSignal {
    
    private int newBalance;

    public MoneyChangeSignal(int newBalance) {
        this.newBalance = newBalance;
    }

    public int NewBalance => newBalance;
}