namespace RigolController.Mvvm
{
    using ReactiveUI;

    public class ViewModel : ViewModel<object>
    {
        public ViewModel (object model) : base(model)
        {
            
        }

        public ViewModel () : base(null)
        {

        }
    }

    public class ViewModel<T> : ReactiveObject
    {
        public ViewModel(T model)
        {
            this.model = model;
        }

        private T model;
        public T Model
        {
            get { return model; }
            set { this.RaiseAndSetIfChanged(ref model, value); }
        }
    }

}
