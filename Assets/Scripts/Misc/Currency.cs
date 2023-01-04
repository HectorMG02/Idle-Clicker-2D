public static class Currency
{
    public static string MoneyToText(this int money)
    {
        int charactersLength = money.ToString().Length;

        switch (charactersLength)
        {
            case > 3 and <= 6:
                return money.ToString("0,.##K");
            case > 6 and <= 9:
                return money.ToString("0,,.##M");
            case >= 9:
                return money.ToString("0,,,.##B");
        }
        
        return money.ToString();
    }
}