using SilvagenumData;
using SilvagenumLogic;

internal class Program
{
    public static Person? selectedPerson = null;
    delegate void Del(IRepo repo);

    private static void Main()
    {
        IRepo activeRepo = new DummyRepo();
        Console.WriteLine("Welcome to the Silvagenum app!");

        string finalDescription;
        string initialDescription = "Enter the number to choose an option:\n"
                                     + "\n 1 - Display the full list of persons in the current repo"
                                     + "\n 2 - Search person by ID"
                                     + "\n 3 - Search person by name"
                                     + "\n 4 - Add a new person";

        Del displayAllHandle = delegate (IRepo repo) { DisplayAll(repo.GetAll(), "The repository is empty."); };
        Del searchIdHandle = delegate (IRepo repo) { SearchById("Enter the id to search for:", repo: repo); };
        Del searchNameHandle = new(SearchByName);
        Del addNewHandle = new(AddNewPerson);
        Del interactHandle = new(InteractWith);

        bool exit = false;
        while (!exit)
        {
            if (selectedPerson is null)
            {
                finalDescription = initialDescription
                                   + "\n\n 0 - Exit the app";
                exit = DisplayMenu(activeRepo,
                               finalDescription,
                               displayAllHandle,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle);
            }
            else
            {
                finalDescription = initialDescription
                                   + $"\n 5 - View or modify {selectedPerson}"
                                   + "\n\n 0 - Exit the app";
                exit = DisplayMenu(activeRepo,
                               finalDescription,
                               displayAllHandle,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle,
                               interactHandle);
            }
        }
        Console.WriteLine("See you!");
    }

    private static bool DisplayMenu(IRepo repo, string description, params Del[] delegates)
    {
        string input;

        //Console.Clear();
        Console.WriteLine(description);
        while (true)
        {
            input = ProvideValidString();
            if (input == "0")
            {
                return true;
            }
            else
            {
                for (int i = 1; i <= delegates.Length; i++)
                {
                    if (input == $"{i}")
                    {
                        delegates[i - 1].Invoke(repo);
                        return false;
                    }
                }
                Console.WriteLine("Incorrect input, please try again.");
            }
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

    private static void SearchById(string description, IRepo? repo = null, List<Person>? list = null)
    {
        Console.WriteLine(description);

        Person? person = null;
        if (repo != null)
        {
            person = repo.Get(ProvideValidInt());
        }
        else if (list != null)
        {
            person = list.Find(x => x.Id == ProvideValidInt());
        }                                                           //dobre miejsce na dorzucenie łapania błędów

        if (person != null)
        {
            Console.WriteLine($"Person found: {person}");
            selectedPerson = person;
        }
        else
        {
            Console.WriteLine("No person found!");
        }
    }

    private static void SearchByName(IRepo repo)
    {
        Console.WriteLine("Enter the name to search for (case insensitive):");

        List<Person>? list = repo.Get(ProvideValidString());

        if (list is null || list?.Count == 0)
        {
            Console.WriteLine("No person found!");
        }
        else if (list?.Count == 1)
        {
            Console.WriteLine($"Person found: {list[0]}");
            selectedPerson = list[0];
        }
        else
        {
            Console.WriteLine($"Found multiple results:");
            DisplayAll(list!);

            Console.WriteLine("\n 1 - Select a person from the list by id"
                              + "\n 0 - Back to the main menu");
            bool exit = false;
            while (!exit)
            {
                switch (ProvideValidString())
                {
                    case "1":
                        SearchById("Enter the id to select from the list:", list: list);
                        exit = true;
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect input, please try again.");
                        break;
                }
            }
        }
    }

    private static void AddNewPerson(IRepo repo)
    {
        Gender gender = Gender.UNASSIGNED;

        Console.WriteLine("Enter the (first) name of the new person:");

        string name = ProvideValidString();

        Console.WriteLine("Select the gender of the new person - [m]ale or [f]emale:");
        while (gender == Gender.UNASSIGNED)
        {
            string input = ProvideValidString();
            switch (input)
            {
                case "m":
                    gender = Gender.male;
                    break;
                case "f":
                    gender = Gender.female;
                    break;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }

        selectedPerson = new(name, gender);
        repo.Add(selectedPerson);
        Console.WriteLine($"New person added: {selectedPerson}");
    }

    private static void InteractWith(IRepo repo)
    {
        bool wasEdited = false;
        string description;

        Del detailsHandle = delegate (IRepo repo) { selectedPerson!.ShowDetails(); };
        Del editHandle = delegate (IRepo repo) { wasEdited = EditSelected(repo); };
        Del deleteHandle = delegate (IRepo repo) { wasEdited = DeleteSelected(repo); };

        bool exit = false;
        while (!exit && selectedPerson is not null)
        {
            description = $" 1 - Details of {selectedPerson}"
                             + $"\n 2 - Edit {selectedPerson}"
                             + $"\n 3 - Delete {selectedPerson}"
                             + "\n\n 0 - Back to main menu";
            wasEdited = false;

            while (!exit && !wasEdited)
            {
                exit = DisplayMenu(repo,
                                   description,
                                   detailsHandle,
                                   editHandle,
                                   deleteHandle);
            }
        }
    }

    private static bool EditSelected(IRepo repo)        //WIP
    {
        string? fatherName;
        string? motherName;
        string? birthDateString;
        string? deathDateString;
        string description;
        bool wasEdited;
        bool wasEditedExport = false;

        Del nameHandle = delegate (IRepo repo) { wasEdited = EditName(false); };
        Del surnameHandle = delegate (IRepo repo) { wasEdited = EditName(true); };
        Del birthHandle = delegate (IRepo repo) { wasEdited = EditDate(false); };
        Del deathHandle = delegate (IRepo repo) { wasEdited = EditDate(true); };
        Del motherHandle = delegate (IRepo repo) { wasEdited = SetParent(repo, Gender.female); };
        Del fatherHandle = delegate (IRepo repo) { wasEdited = SetParent(repo, Gender.male); };
        Del childrenHandle = delegate (IRepo repo) { wasEdited = EditChildren(repo); };

        bool exit = false;
        while (!exit)
        {
            fatherName = selectedPerson!.Father?.ToString();
            motherName = selectedPerson!.Mother?.ToString();
            birthDateString = selectedPerson!.BirthDate?.ToString();
            deathDateString = selectedPerson!.DeathDate?.ToString();
            description = $"You're editing {selectedPerson}, choose the info to edit:"
                             + $"\n 1 - Name - {selectedPerson.Name}"
                             + $"\n 2 - Surname - {selectedPerson.Surname}"
                             + $"\n 3 - Birth date - {birthDateString ?? "undefined"}"
                             + $"\n 4 - Death date - {deathDateString ?? "undefined"}"
                             + $"\n 5 - Mother - {motherName ?? "undefined"}"
                             + $"\n 6 - Father - {fatherName ?? "undefined"}"
                             + $"\n 7 - Children - {selectedPerson.Children.Count}"
                             + "\n\n 0 - Back to interaction menu";
            wasEdited = false;

            while (!exit && !wasEdited)
            {
                exit = DisplayMenu(repo,
                                   description,
                                   nameHandle,
                                   surnameHandle,
                                   birthHandle,
                                   deathHandle,
                                   motherHandle,
                                   fatherHandle,
                                   childrenHandle);
            }
            wasEditedExport = wasEdited;
        }
        return wasEditedExport;
    }

    private static bool DeleteSelected(IRepo repo)
    {
        Console.WriteLine($"Are you sure you want to delete {selectedPerson}? This is irreversible. [y/n]");
        while (true)
        {
            switch (ProvideValidString().ToLower())
            {
                case "y":
                    repo.Delete(selectedPerson!);
                    selectedPerson = null;
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private static bool EditName(bool editSurname)
    {
        string sur = editSurname ? "sur" : "";

        Console.WriteLine($"Please enter the new {sur}name of {selectedPerson}:");

        string input = ProvideValidString();

        if (editSurname)
        {
            selectedPerson!.Surname = input;
        }
        else
        {
            selectedPerson!.Name = input;
        }

        Console.WriteLine($"The {sur}name of {selectedPerson} was successfully changed.");
        return true;
    }

    private static bool EditDate(bool editDeath)
    {
        string dateName = editDeath ? "death" : "birth";
        DateOnly? currentDate = editDeath ? selectedPerson!.DeathDate : selectedPerson!.BirthDate;

        if (currentDate is not null)
        {
            Console.WriteLine($"The current date of {dateName} of {selectedPerson} is {currentDate}.");
        }
        else
        {
            Console.WriteLine($"There is no date of {dateName} currently set for {selectedPerson}.");
        }

        Console.WriteLine($"Please enter the new date of {dateName} in DD.MM.YYYY format:");

        if (editDeath)
        {
            selectedPerson!.DeathDate = ProvideValidDate();
        }
        else
        {
            selectedPerson!.BirthDate = ProvideValidDate();
        }

        Console.WriteLine($"The date of {dateName} of {selectedPerson} was successfully changed to {ProvideValidDate()}.");
        return true;
    }

    private static bool SetParent(IRepo repo, Gender gender)
    {
        string parentType;
        string? currentParent;
        Person parent;

        if (gender == Gender.male)
        {
            parentType = "father";
            currentParent = selectedPerson!.Father?.ToString();
        }
        else
        {
            parentType = "mother";
            currentParent = selectedPerson!.Mother?.ToString();
        }

        if (currentParent is not null)
        {
            Console.WriteLine($"The current {parentType} of {selectedPerson} is {currentParent}. Please select the method of providing the new one:");
        }
        else
        {
            Console.WriteLine($"There is no {parentType} currently set for {selectedPerson}. Please select the method of providing one:");
        }

        parent = ProvidePerson(repo);

        if (gender == Gender.male)
        {
            selectedPerson.Father = parent;
        }
        else
        {
            selectedPerson.Mother = parent;
        }

        Console.WriteLine($"{parent} set as new {parentType} of {selectedPerson}.");
        return true;
    }

    private static bool EditChildren(IRepo repo)
    {
        int childCount;
        string deleteDescription;
        bool result = false;
        Person editedPerson;

        while (true)
        {
            childCount = selectedPerson!.Children.Count;
            deleteDescription = childCount > 0 ? "\n 2 - Remove a person from the list of children" : "";

            Console.WriteLine($"{selectedPerson} currently has {childCount} children.");
            Console.WriteLine("\n 1 - Add a new child"
                              + $"{deleteDescription}"
                              + "\n 0 - Back to the edit menu");

            switch (ProvideValidString())
            {
                case "1":
                    Console.WriteLine("Select the source of the new child:");
                    selectedPerson.Children.Add(ProvidePerson(repo));
                    result = true;
                    break;
                case "2":
                    if (childCount > 0)
                    {
                        Console.WriteLine("The current list of children:");
                        foreach (Person child in selectedPerson!.Children)
                        {
                            Console.WriteLine(child);
                        }
                        editedPerson = selectedPerson;
                        selectedPerson = null;
                        SearchById("Enter the id of the child to remove from the list:", list: editedPerson.Children);
                        if (selectedPerson != null)
                        {
                            if (editedPerson.Gender == Gender.male)
                            {
                                selectedPerson.Father = null;
                            }
                            else
                            {
                                selectedPerson.Mother = null;
                            }
                            result = true;
                        }
                    }
                    else { goto default; }
                    break;
                case "0":
                    return result;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private static Person ProvidePerson(IRepo repo)
    {
        Person tempPerson;
        Person result;
        string description = " 1 - Search person by ID"
                             + "\n 2 - Search person by name"
                             + "\n 3 - Add a new person";
        Del searchIdHandle = delegate (IRepo repo) { SearchById("Enter the id to search for:", repo: repo); };
        Del searchNameHandle = new(SearchByName);
        Del addNewHandle = new(AddNewPerson);

        tempPerson = selectedPerson!;
        selectedPerson = null;

        bool exit = false;
        while (!exit && selectedPerson is null)
        {
            exit = DisplayMenu(repo,
                               description,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle);
        }

        result = selectedPerson!;
        selectedPerson = tempPerson;
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

    private static int ProvideValidInt()
    {
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Incorrect input, please try again.");
        }
        return id;
    }
}