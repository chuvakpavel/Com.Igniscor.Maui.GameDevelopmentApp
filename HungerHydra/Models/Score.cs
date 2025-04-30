namespace HungerHydra.Models;

internal class Score
{
    public int Id { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }

    public Score(int value)
    {
        Value = value;
        Date = DateTime.Now.Date;
    }
}