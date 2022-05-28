namespace YTE.Common.DTOS
{
    public class ListItem<TText, TValue>
    {
        public TText Text { get; set; }
        public TValue Value { get; set; }
    }
}
