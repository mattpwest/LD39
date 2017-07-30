using System;

namespace Assets.Scripts.UI.Events
{
    class JumpEvent : IEvent
    {
        private ShipResources shipResources;
        private String title;
        private String flavourText;
        private float startTime;


        public JumpEvent(ShipResources shipResources, String title, String flavourText)
        {
            this.shipResources = shipResources;
            this.title = title;
            this.flavourText = flavourText;
        }

        public bool HasDialog()
        {
            return false;
        }

        public void HideDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowDialog()
        {
            throw new NotImplementedException();
        }

        public void ExecuteStep()
        {
            startTime = shipResources.TimeLeft;

            shipResources.Jump();
        }

        public EventResult GetResult(bool wasAttacked)
        {
            var timeGained = (int) Math.Round(shipResources.TimeLeft - startTime);

            return new EventResult()
            {
                Title = title,
                FlavourText = flavourText,
                Gain1 = new EventResultItem() {Name = "Time", Value = timeGained}
            };
        }

        public bool IsBattle()
        {
            return false;
        }
    }
}
