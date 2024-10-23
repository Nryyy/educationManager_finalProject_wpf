public class FinancialManager
{
    private Stack<FinancialTransaction> transactions = new Stack<FinancialTransaction>();

    public void AddTransaction(FinancialTransaction transaction)
    {
        transactions.Push(transaction);
    }

    public FinancialTransaction UndoLastTransaction()
    {
        return transactions.Pop();
    }

    public FinancialTransaction ViewLastTransaction()
    {
        return transactions.Peek();
    }
}
