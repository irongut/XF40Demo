namespace XF40Demo.Models
{
    public class CacheTime
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public CacheTime(string text, int value)
        {
            Text = text;
            Value = value;
        }
    }
}
