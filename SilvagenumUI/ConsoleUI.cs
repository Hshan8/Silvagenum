using SilvagenumData;
using SilvagenumLogic;

namespace SilvagenumUI;
public class ConsoleUI : IUserInterface
{
    private readonly IRepo activeRepo;
    delegate void Del(Person person);
    enum Menu
    {
        Main,
        MainWithSelectedPerson,
        Interact,
        Edit
    }

    public ConsoleUI(IRepo repo)
    {
        activeRepo = repo;
    }

    public void Run()
    {
        Person? selectedPerson = null;
        Person? returnedPerson = null;
        Menu position = Menu.Main;

        Console.WriteLine("Welcome to the Silvagenum app!");

        void DisplayAllHandle(Person selectedPerson) => DisplayAll(activeRepo.GetAll(), "The repository is empty.");
        void SearchIdHandle(Person selectedPerson) => returnedPerson = SearchById();
        void SearchNameHandle(Person selectedPerson) => returnedPerson = SearchByName();
        void AddPersonHandle(Person selectedPerson) => returnedPerson = AddNewPerson();
        void InteractHandle(Person selectedPerson) => position = Menu.Interact;
        void ShowDetailsHandle(Person selectedPerson) => Console.WriteLine(selectedPerson.ToString(LevelOfDetail.detailed));
        void EditHandle(Person selectedPerson) => position = Menu.Edit;
        void DeleteHandle(Person? selectedPerson)
        {
            Delete(ref selectedPerson);
            position = Menu.Main;
        }

        bool exit = false;
        bool back = false;
        while (!exit)
        {
            switch (position)
            {
                case Menu.Main:
                    exit = DisplayMenu(GenerateDescription(position, selectedPerson),
                                       null,
                                       DisplayAllHandle,
                                       SearchIdHandle,
                                       SearchNameHandle,
                                       AddPersonHandle);
                    break;

                case Menu.MainWithSelectedPerson:
                    exit = DisplayMenu(GenerateDescription(position, selectedPerson),
                                       selectedPerson,
                                       DisplayAllHandle,
                                       SearchIdHandle,
                                       SearchNameHandle,
                                       AddPersonHandle,
                                       InteractHandle);
                    break;

                case Menu.Interact:
                    back = DisplayMenu(GenerateDescription(position, selectedPerson),
                                       selectedPerson,
                                       ShowDetailsHandle,
                                       EditHandle,
                                       DeleteHandle);
                    if (back) position = Menu.MainWithSelectedPerson;
                    break;

                case Menu.Edit:
                    back = DisplayMenu(GenerateDescription(position, selectedPerson),
                                       selectedPerson,
                                       EditFirstName,
                                       EditSurname,
                                       EditBirthDate,
                                       EditDeathDate,
                                       EditMother,
                                       EditFather,
                                       EditChildren);
                    if (back) position = Menu.Interact;
                    break;

                default:
                    Console.WriteLine("Error! Something went wrong and you ended up outside of any of the menus. Returning to the main menu.");
                    position = selectedPerson is null ? Menu.Main : Menu.MainWithSelectedPerson;
                    break;
            }
            selectedPerson = returnedPerson ?? selectedPerson;
            if (selectedPerson != null && position == Menu.Main) position = Menu.MainWithSelectedPerson;
            Console.WriteLine(selectedPerson);
        }
        activeRepo.Save();
        Console.WriteLine("See you!");
    }

    private static string GenerateDescription(Menu position, Person? selected)
    {
        switch (position)
        {
            case Menu.Main:
                return "Enter the number to choose an option:\n"
                       + "\n 1 - Display the full list of persons in the current repo"
                       + "\n 2 - Search person by ID"
                       + "\n 3 - Search person by name"
                       + "\n 4 - Add a new person"
                       + "\n\n 0 - Exit the app";

            case Menu.MainWithSelectedPerson:
                return "Enter the number to choose an option:\n"
                       + "\n 1 - Display the full list of persons in the current repo"
                       + "\n 2 - Search person by ID"
                       + "\n 3 - Search person by name"
                       + "\n 4 - Add a new person"
                       + $"\n 5 - View or modify {selected}"
                       + "\n\n 0 - Exit the app";

            case Menu.Interact:
                return $" 1 - Details of {selected}"
                       + $"\n 2 - Edit {selected}"
                       + $"\n 3 - Delete {selected}"
                       + "\n\n 0 - Back to main menu";

            case Menu.Edit:
                string? fatherName = selected!.Father?.ToString();
                string? motherName = selected.Mother?.ToString();
                string? birthDateString = selected.BirthDate?.ToString();
                string? deathDateString = selected.DeathDate?.ToString();
                return $"You're editing {selected}, choose the info to edit:"
                                 + $"\n 1 - Name: {selected.Name}"
                                 + $"\n 2 - Surname: {selected.Surname}"
                                 + $"\n 3 - Birth date: {birthDateString ?? "undefined"}"
                                 + $"\n 4 - Death date: {deathDateString ?? "undefined"}"
                                 + $"\n 5 - Mother: {motherName ?? "undefined"}"
                                 + $"\n 6 - Father: {fatherName ?? "undefined"}"
                                 + $"\n 7 - Children: {selected.Children.Count}"
                                 + "\n\n 0 - Back to interaction menu";

            default:
                return "Error! Unknown type of description was demanded.";
        }
    }

    private static bool DisplayMenu(string description, Person? person, params Del[] delegates)
    {
        Console.WriteLine(description);
        while (true)
        {
            int input = ProvideValidInt(maxValid: delegates.Length);
            if (input == 0)
            {
                return true;
            }

            delegates[input - 1].Invoke(person);            //"person" should only ever be null for methods which don't take it as a parameter
            return false;
        }
    }

    private static void DisplayAll(List<Person> list, string emptySourceMessage = "")
    {
        if (list.Count > 0)
        {
            foreach (Person person in list)
            {
                Console.WriteLine(person);
            }
        }
        else { Console.WriteLine(emptySourceMessage); }
    }

    private Person? SearchById(string description = "Enter the id to search for:", List<Person>? list = null)
    {
        Console.WriteLine(description);
        int id = ProvideValidInt();
        Person? person = null;
        if (list?.Exists(x => x.Id == id) ?? true)
        {
            person = activeRepo.Get(id);
        }
        DescribeSearchResults(person);
        return person;
    }

    private Person? SearchByName()
    {
        Console.WriteLine("Enter the name to search for (case insensitive):");

        List<Person>? list = activeRepo.Get(ProvideValidString());

        DescribeSearchResults(list);
        if (!(list?.Count > 1))
        {
            return list?[0];
        }
        DisplayAll(list!);

        Console.WriteLine("\n 1 - Select a person from the list by id"
                          + "\n 0 - Back to the main menu");

        if (ProvideValidInt(maxValid: 1) == 1)
        {
            return SearchById("Enter the id to select from the list:", list);
        }
        return null;
    }

    private static string DescribeSearchResults(Person? person) => person switch
    {
        null => "No person found!",
        _ => $"Person found: {person}"
    };
    private static string DescribeSearchResults(List<Person>? list) => !(list?.Count > 1) ? DescribeSearchResults(person: list?[0]) : $"Found multiple results:";

    private Person AddNewPerson()
    {
        Console.WriteLine("Enter the (first) name of the new person:");
        string name = ProvideValidString();

        Console.WriteLine("Select the gender of the new person - [m]ale or [f]emale:");
        Gender gender = SelectGender();

        Person newPerson = activeRepo.Add(name, gender);
        Console.WriteLine($"New person added: {newPerson}");
        return newPerson;
    }

    private static Gender SelectGender()
    {
        while (true)
        {
            switch (ProvideValidString().ToLower())
            {
                case "m":
                    return Gender.male;
                case "f":
                    return Gender.female;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private void Delete(ref Person? toBeDeleted)
    {
        Console.WriteLine($"Are you sure you want to delete {toBeDeleted}? This is irreversible. [y/n]");
        while (true)
        {
            switch (ProvideValidString().ToLower())
            {
                case "y":
                    activeRepo.Delete(toBeDeleted!);
                    toBeDeleted = null;
                    return;
                case "n":
                    return;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private void EditFirstName(Person person)
    {
        Console.WriteLine($"Please enter the new name of {person}:");
        person.Name = ProvideValidString();
        Console.WriteLine($"The name of {person} was successfully changed.");
    }

    private void EditSurname(Person person)
    {
        Console.WriteLine($"Please enter the new surname of {person}:");
        person.Surname = ProvideValidString();
        Console.WriteLine($"The surname of {person} was successfully changed.");
    }

    private void EditBirthDate(Person person) => EditDate(person);
    private void EditDeathDate(Person person) => EditDate(person, true);
    private static void EditDate(Person person, bool editDeath = false)
    {
        string dateName = editDeath ? "death" : "birth";
        DateOnly? currentDate = editDeath ? person.DeathDate : person.BirthDate;
        DateOnly newDate;

        if (currentDate != null)
        {
            Console.WriteLine($"The current date of {dateName} of {person} is {currentDate}.");
        }
        else
        {
            Console.WriteLine($"There is no date of {dateName} currently set for {person}.");
        }

        Console.WriteLine($"Please enter the new date of {dateName} in DD.MM.YYYY format:");

        newDate = ProvideValidDate();
        if (editDeath)
        {
            person.DeathDate = newDate;
        }
        else
        {
            person.BirthDate = newDate;
        }

        Console.WriteLine($"The date of {dateName} of {person} was successfully changed to {newDate}.");
    }

    private void EditFather(Person child) => EditParent(child, Gender.male);
    private void EditMother(Person child) => EditParent(child, Gender.female);
    private void EditParent(Person child, Gender gender)
    {
        child.DescribeParent(gender);
        Console.WriteLine("Please select the method of providing one:");
        Person? newParent = ProvidePerson();
        if (newParent != null)
        {
            child.SetParent(newParent);
            Console.WriteLine($"{newParent} set as new {gender.ParentTerm()} of {child}.");
        }
    }

    private void EditChildren(Person parent)
    {
        int childCount;
        string deleteDescription;
        bool exit = false;
        Person? child;
        Person? newChild;

        while (!exit)
        {
            childCount = parent.Children.Count;
            deleteDescription = childCount > 0 ? "\n 2 - Remove a person from the list of children" : "";

            Console.WriteLine($"{parent} currently has {childCount} children.");
            Console.WriteLine("\n 1 - Add a new child"
                              + $"{deleteDescription}"
                              + "\n 0 - Back to the edit menu");

            switch (ProvideValidInt(maxValid: childCount > 0 ? 2 : 1))
            {
                case 1:
                    Console.WriteLine("Select the source of the new child:");
                    newChild = ProvidePerson();
                    if (newChild != null)
                    {
                        newChild.SetParent(parent);
                        Console.WriteLine($"{newChild} set as new child of {parent}.");
                    }
                    exit = true;
                    break;
                case 2:
                    Console.WriteLine("The current list of children:");
                    DisplayAll(parent.Children);
                    child = SearchById("Enter the id of the child to remove from the list:", list: parent.Children);
                    if (child != null)
                    {
                        child.DeleteParent(parent.Gender);
                        exit = true;
                    }
                    break;
                case 0:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private Person? ProvidePerson()
    {
        Person? result = null;
        void searchIdHandle(Person person) => result = SearchById();
        void searchNameHandle(Person person) => result = SearchByName();
        void addNewHandle(Person person) => result = AddNewPerson();

        bool exit = false;
        while (!exit)
        {
            exit = DisplayMenu(" 1 - Search person by ID"
                             + "\n 2 - Search person by name"
                             + "\n 3 - Add a new person"
                             + "\n\n 0 - Cancel",
                               null,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle);
            if (result == null) Console.WriteLine("Please try again or cancel the operation.");
        }
        if (result == null) Console.WriteLine("Operation canceled.");
        return result;
    }

    private static string ProvideValidString()
    {
        string? input = null;
        while (string.IsNullOrEmpty(input))
        {
            input = Console.ReadLine();
        }
        input = input.Trim();
        return input;
    }

    private static DateOnly ProvideValidDate()
    {
        DateOnly date;
        string? input = Console.ReadLine();
        while (!DateOnly.TryParse(input, out date))
        {
            Console.WriteLine("Incorrect input, please try again.");
            input = Console.ReadLine();
        }
        return date;
    }

    private static int ProvideValidInt(int maxValid = int.MaxValue)
    {
        int result;
        while (!int.TryParse(Console.ReadLine(), out result) || result > maxValid)
        {
            Console.WriteLine("Incorrect input, please try again.");
        }
        return result;
    }
}