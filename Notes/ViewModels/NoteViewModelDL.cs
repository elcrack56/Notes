using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class NoteViewModel : ObservableObject, IQueryAttributable
{
    private Models.NoteDL _note;

    public string TextDL
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime DateDL => _note.Date;

    public string IdentificadorDL => _note.Filename;

    public ICommand GuardarCommandDL { get; private set; }
    public ICommand EliminarCommandDL { get; private set; }

    public NoteViewModel()
    {
        _note = new Models.NoteDL();
        GuardarCommandDL = new AsyncRelayCommand(GuardarDL);
        EliminarCommandDL = new AsyncRelayCommand(EliminarDL);
    }

    public NoteViewModel(Models.NoteDL note)
    {
        _note = note;
        GuardarCommandDL = new AsyncRelayCommand(GuardarDL);
        EliminarCommandDL = new AsyncRelayCommand(EliminarDL);
    }

    private async Task GuardarDL()
    {
        _note.Date = DateTime.Now;
        _note.SaveDL();
        await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
    }

    private async Task EliminarDL()
    {
        _note.DeleteDL();
        await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.NoteDL.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void ReloadDL()
    {
        _note = Models.NoteDL.Load(_note.Filename);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(TextDL));
        OnPropertyChanged(nameof(DateDL));
    }
}