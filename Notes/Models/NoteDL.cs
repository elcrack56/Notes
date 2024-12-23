using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

internal class NoteDL
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public NoteDL()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }
    public void SaveDL() =>
    File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void DeleteDL() =>
    File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static NoteDL Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<NoteDL> LoadAll()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        return Directory

                .EnumerateFiles(appDataPath, "*.notes.txt")

                .Select(filename => NoteDL.Load(Path.GetFileName(filename)))

                .OrderByDescending(note => note.Date);
    }
}

