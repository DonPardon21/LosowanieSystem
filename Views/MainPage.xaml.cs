using System.Collections.ObjectModel;
using System.Linq;
using LosowanieSystem.Models;

namespace LosowanieSystem.Views;

public partial class MainPage : ContentPage
{
    private string newClass;

    public ObservableCollection<Person> studentsList { get; set; } = new ObservableCollection<Person>();
    public ObservableCollection<Person> selectedClassStudents { get; set; } = new ObservableCollection<Person>();
    public ObservableCollection<string> classlist { get; set; } = new ObservableCollection<string>();
    private string selectedClass { get; set; }
    private int randomIndex { get; set; }

    public FileOperations file_operations { get; set; } = new FileOperations();

    public MainPage()
    {
        InitializeComponent();
        file_operations.FileLoad(studentsList);


        foreach (var item in studentsList.DistinctBy(el => el.StudentClass))
        {
            classlist.Add(item.StudentClass);
        }

        BindingContext = this;
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            selectedClassStudents.Clear();
            selectedClass = picker.Items[selectedIndex];
            foreach (var item in studentsList.Where(item => item.StudentClass == selectedClass))
            {
                selectedClassStudents.Add(item);
            }

            gridSelecetedClassEdit.IsVisible = true;
        }
    }

    private void Button_Random(object sender, EventArgs e)
    {
        Random rnd = new Random();
        int selectedClassStudentsCount = selectedClassStudents.Count;
        if (selectedClassStudentsCount > 1)
        {
            while (true)
            {
                int index = rnd.Next(0, selectedClassStudentsCount);
                if (index != randomIndex)
                {
                    randomIndex = rnd.Next(0, selectedClassStudentsCount);
                    break;
                }

            }
            wylosowanyUczen.Text = $"{selectedClassStudents[randomIndex].Name} {selectedClassStudents[randomIndex].Surname} {selectedClassStudents[randomIndex].Number}";
        }
        else
        {
            wylosowanyUczen.Text = "Nie mozna wylosowac, dodaj wiecej uczniow";
        }



    }

    private void Button_Delete(object sender, EventArgs e)
    {
        Person student = (Person)((Button)sender).BindingContext;
        studentsList.Remove(student);
        selectedClassStudents.Remove(student);

        file_operations.SaveStudents(studentsList);
    }

    private void Button_ADD(object sender, EventArgs e)
    {
        string name = editorName.Text;
        string surename = editorSurename.Text;

        Person student = new Person
        {
            Name = name,
            Surname = surename,
            Number = selectedClassStudents.Count + 1,
            StudentClass = selectedClass
        };

        studentsList.Add(student);
        selectedClassStudents.Add(student);

        file_operations.SaveStudents(studentsList);
    }

    private void Button_NewClass(object sender, EventArgs e)
    {
        string newClassName = newClassEditor.Text; 

        if (!string.IsNullOrEmpty(newClassName)) 
        {
            if (!classlist.Contains(newClassName))
            {
                classlist.Add(newClassName);
            }
            else
            {
                DisplayAlert("Uwaga", "Ta klasa już istnieje.", "OK");
                return; 
            }
        }
        else
        {
            DisplayAlert("Uwaga", "Nazwa klasy nie może być pusta.", "OK");
        }
    }

}
