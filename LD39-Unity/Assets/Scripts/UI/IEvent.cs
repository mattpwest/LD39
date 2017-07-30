namespace Assets.Scripts.UI
{
    public interface IEvent
    {
        void HideDialog();
        void ShowDialog();
        void ExecuteStep();
        EventResult GetResult(bool wasAttacked);
        bool IsBattle();
    }

    public class EventResult
    {
        public string Title { get; set; }
        public string FlavourText { get; set; }

        public EventResultItem Cost1 { get; set; }
        public EventResultItem Cost2 { get; set; }
        public EventResultItem Cost3 { get; set; }

        public EventResultItem Gain1 { get; set; }
        public EventResultItem Gain2 { get; set; }
    }

    public class EventResultItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
