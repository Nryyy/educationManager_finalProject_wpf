public class FinancialTransaction
{
    public string Type { get; set; } // "Profit" or "Expense"
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Time { get; set; }

    public FinancialTransaction(string type, decimal amount, string description)
    {
        Type = type;
        Amount = amount;
        Description = description;
        Time = DateTime.Now;
    }
}
