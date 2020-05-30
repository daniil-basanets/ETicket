using System;
using Prism.Mvvm;

namespace ETicketMobile.ViewModels.Tickets
{
    public class AreaViewModel : BindableBase
    {
        public int Id { get; set; }

        private bool selected;
        public bool Selected
        {
            get => selected;
            set
            {
                SetProperty(ref selected, value);
                SelectionChanged?.Invoke();
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Action SelectionChanged { get; set; }
    }
}