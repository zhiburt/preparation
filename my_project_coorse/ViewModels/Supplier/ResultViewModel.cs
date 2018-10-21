using System;

namespace preparation.ViewModels.Supplier
{
    public class ResultViewModel : IResultViewModel
    {
        public ResultViewModel(string message, bool state)
        {
            Message = message;
            State = state;
        }

        public ResultViewModel()
        {
        }


        public bool State { get; }
        public string Message { get; }

        public override bool Equals(object obj)
        {
            var model = obj as ResultViewModel;
            return model != null &&
                   State == model.State &&
                   Message == model.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(State, Message);
        }
    }
}