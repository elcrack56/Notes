using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class NotesViewModelDL : IQueryAttributable
{
    public ObservableCollection<ViewModels.NoteViewModel> AllNotesDL { get; }
    public ICommand NuevoCommandDL { get; }
    public ICommand SelectNoteCommandDL { get; }

    public NotesViewModelDL()
    {
        AllNotesDL = new ObservableCollection<ViewModels.NoteViewModel>(Models.NoteDL.LoadAll().Select(n => new NoteViewModel(n)));
        NuevoCommandDL = new AsyncRelayCommand(NuevaNoteAsyncDL);
        SelectNoteCommandDL = new AsyncRelayCommand<ViewModels.NoteViewModel>(SelectNoteAsyncDL);
    }

    private async Task NuevaNoteAsyncDL()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsyncDL(ViewModels.NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.IdentificadorDL}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModel matchedNote = AllNotesDL.Where((n) => n.IdentificadorDL == noteId).FirstOrDefault();

            if (matchedNote != null)
                AllNotesDL.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModel matchedNote = AllNotesDL.Where((n) => n.IdentificadorDL == noteId).FirstOrDefault();

            if (matchedNote != null)
            {
                matchedNote.ReloadDL();
                AllNotesDL.Move(AllNotesDL.IndexOf(matchedNote), 0);
            }

            else
                AllNotesDL.Insert(0, new NoteViewModel(Models.NoteDL.Load(noteId)));
        }
    }
}