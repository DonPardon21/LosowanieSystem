using LosowanieSystem.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

public class FileOperations
{
    public string filePath { get; set; } = Path.Combine(FileSystem.Current.AppDataDirectory, "Uczeń.txt");
    public FileOperations() { }

    public void FileLoad(ObservableCollection<Person> studentsList)
    {
        Debug.WriteLine(filePath);
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
            return;
        }

        StreamReader streamReader = new StreamReader(filePath);
        string line;

        while ((line = streamReader.ReadLine()) != null)
        {
            string[] lineSplited = line.Split('/');
            if (lineSplited.Length >= 4) // Sprawdź, czy tablica ma wystarczającą liczbę elementów
            {
                string name = lineSplited[0];
                string surname = lineSplited[1];
                string studentClass = lineSplited[3];
                int number = 0;

                if (Int32.TryParse(lineSplited[2], out int n)) { number = n; }

                studentsList.Add(new Person
                {
                    Name = name,
                    Surname = surname,
                    Number = number,
                    StudentClass = studentClass
                });
            }
        }

        streamReader.Close();
    }

    public void SaveStudents(ObservableCollection<Person> studentsList)
    {
        List<string> list = new List<string>();
        foreach (Person person in studentsList)
        {
            string expresion = $"{person.Name}/{person.Surname}/{person.Number}/{person.StudentClass}";
            list.Add(expresion);
        }

        File.WriteAllLines(filePath, list);
    }
}


