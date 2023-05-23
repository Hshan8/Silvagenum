using SilvagenumData;
using SilvagenumLogic;

internal class Program
{
    public static Person? selectedPerson = null;
    public static IRepo activeRepo = new DummyRepo();
    delegate void Del();

    private static void Main()
    {
        Person? selectedPerson = null;
        Person? returnedPerson = null;
        Console.WriteLine("Welcome to the Silvagenum app!");

        string finalDescription;
        string initialDescription = "Enter the number to choose an option:\n"
                                     + "\n 1 - Display the full list of persons in the current repo"
                                     + "\n 2 - Search person by ID"
                                     + "\n 3 - Search person by name"
                                     + "\n 4 - Add a new person";

        Del displayAllHandle = delegate () { DisplayAll(activeRepo.GetAll(), "The repository is empty."); };
        Del searchIdHandle = delegate () { returnedPerson = SearchById("Enter the id to search for:"); };
        Del searchNameHandle = delegate () { returnedPerson = SearchByName(); };
        Del addNewHandle = delegate () { returnedPerson = AddNewPerson(); };

        bool exit = false;
        while (!exit)
        {
            if (selectedPerson is null)
            {
                finalDescription = initialDescription
                                   + "\n\n 0 - Exit the app";
                exit = DisplayMenu(finalDescription,
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
                exit = DisplayMenu(finalDescription,
                               displayAllHandle,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle,
                               InteractWith);
            }
            selectedPerson = returnedPerson ?? selectedPerson;
        }
        Console.WriteLine("See you!");
    }

    private static bool DisplayMenu(string description, params Del[] delegates)
    {
        Console.WriteLine(description);
        while (true)
        {
            int input = ProvideValidInt(maxValid: delegates.Length);
            if (input == 0)
            {
                return true;
            }

            delegates[input - 1].Invoke();
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

    private static Person? SearchById(string description, List<Person>? list = null)
    {
        Console.WriteLine(description);

        Person? person = null;
        if (list == null)
        {
            person = activeRepo.Get(ProvideValidInt());
        }
        else
        {
            person = list.Find(x => x.Id == ProvideValidInt());
        }

        if (person != null)
        {
            Console.WriteLine($"Person found: {person}");
        }
        else
        {
            Console.WriteLine("No person found!");
        }
        return person;
    }

    private static Person? SearchByName()
    {
        Console.WriteLine("Enter the name to search for (case insensitive):");

        List<Person>? list = activeRepo.Get(ProvideValidString());

        if (list is null || list?.Count == 0)
        {
            Console.WriteLine("No person found!");
            return null;
        }

        if (list?.Count == 1)
        {
            Console.WriteLine($"Person found: {list[0]}");
            return list[0];
        }

        Console.WriteLine($"Found multiple results:");
        DisplayAll(list!);

        Console.WriteLine("\n 1 - Select a person from the list by id"
                          + "\n 0 - Back to the main menu");

        if (ProvideValidInt(maxValid: 1) == 1)
        {
            return SearchById("Enter the id to select from the list:", list: list);
        }
        return null;
    }

    private static Person AddNewPerson()
    {
        Console.WriteLine("Enter the (first) name of the new person:");
        string name = ProvideValidString();

        Console.WriteLine("Select the gender of the new person - [m]ale or [f]emale:");
        Gender gender = SelectGender();

        Person newPerson = new(name, gender);
        activeRepo.Add(newPerson);
        Console.WriteLine($"New person added: {newPerson}");
        return newPerson;
    }

    private static Gender SelectGender()
    {
        while (true)
        {
            switch (ProvideValidString())
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

    private static void InteractWith()            //selectedPerson
    {
        bool wasEdited = false;
        string description;

        Del detailsHandle = delegate () { selectedPerson!.ShowDetails(); };
        Del editHandle = delegate () { wasEdited = EditSelected(); };
        Del deleteHandle = delegate () { wasEdited = DeleteSelected(); };

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
                exit = DisplayMenu(description,
                                   detailsHandle,
                                   editHandle,
                                   deleteHandle);
            }
        }
    }

    private static bool EditSelected()            //selectedPerson
    {
        string? fatherName;
        string? motherName;
        string? birthDateString;
        string? deathDateString;
        string description;
        bool wasEdited;
        bool wasEditedExport = false;

        Del nameHandle = delegate () { wasEdited = EditName(false); };
        Del surnameHandle = delegate () { wasEdited = EditName(true); };
        Del birthHandle = delegate () { wasEdited = EditDate(false); };
        Del deathHandle = delegate () { wasEdited = EditDate(true); };
        Del motherHandle = delegate () { wasEdited = EditParent(Gender.female); };
        Del fatherHandle = delegate () { wasEdited = EditParent(Gender.male); };
        Del childrenHandle = delegate () { wasEdited = EditChildren(); };

        bool exit = false;
        while (!exit)
        {
            fatherName = selectedPerson!.Father?.ToString();
            motherName = selectedPerson!.Mother?.ToString();
            birthDateString = selectedPerson!.BirthDate?.ToString();
            deathDateString = selectedPerson!.DeathDate?.ToString();
            description = $"You're editing {selectedPerson}, choose the info to edit:"
                             + $"\n 1 - Name: {selectedPerson.Name}"
                             + $"\n 2 - Surname: {selectedPerson.Surname}"
                             + $"\n 3 - Birth date: {birthDateString ?? "undefined"}"
                             + $"\n 4 - Death date: {deathDateString ?? "undefined"}"
                             + $"\n 5 - Mother: {motherName ?? "undefined"}"
                             + $"\n 6 - Father: {fatherName ?? "undefined"}"
                             + $"\n 7 - Children: {selectedPerson.Children.Count}"
                             + "\n\n 0 - Back to interaction menu";
            wasEdited = false;

            while (!exit && !wasEdited)
            {
                exit = DisplayMenu(description,
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

    private static bool DeleteSelected()            //selectedPerson
    {
        Console.WriteLine($"Are you sure you want to delete {selectedPerson}? This is irreversible. [y/n]");
        while (true)
        {
            switch (ProvideValidString().ToLower())
            {
                case "y":
                    activeRepo.Delete(selectedPerson!);
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

    private static bool EditName(bool editSurname)          //selectedPerson
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

    private static bool EditDate(bool editDeath)            //selectedPerson
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

    private static bool EditParent(Gender gender)            //selectedPerson
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

        parent = ProvidePerson();
        selectedPerson.SetOrDeleteParent(parent);
        Console.WriteLine($"{parent} set as new {parentType} of {selectedPerson}.");
        return true;
    }

    private static bool EditChildren()            //selectedPerson
    {
        int childCount;
        string deleteDescription;
        bool result = false;
        Person child;

        while (true)
        {
            childCount = selectedPerson!.Children.Count;
            deleteDescription = childCount > 0 ? "\n 2 - Remove a person from the list of children" : "";

            Console.WriteLine($"{selectedPerson} currently has {childCount} children.");
            Console.WriteLine("\n 1 - Add a new child"
                              + $"{deleteDescription}"
                              + "\n 0 - Back to the edit menu");

            switch (ProvideValidInt(maxValid: childCount > 0 ? 2 : 1))
            {
                case 1:
                    Console.WriteLine("Select the source of the new child:");
                    selectedPerson.Children.Add(ProvidePerson());
                    result = true;
                    break;
                case 2:
                    Console.WriteLine("The current list of children:");
                    DisplayAll(selectedPerson!.Children);
                    child = SearchById("Enter the id of the child to remove from the list:", list: selectedPerson!.Children);
                    if (child != null)
                    {
                        child.SetOrDeleteParent(selectedPerson.Gender);
                        result = true;
                    }
                    break;
                case 0:
                    return result;
                default:
                    Console.WriteLine("Incorrect input, please try again.");
                    break;
            }
        }
    }

    private static Person ProvidePerson()           //possible null output
    {
        Person? returnedPerson = null;
        string description = " 1 - Search person by ID"
                             + "\n 2 - Search person by name"
                             + "\n 3 - Add a new person";
        Del searchIdHandle = delegate () { returnedPerson = SearchById("Enter the id to search for:"); };
        Del searchNameHandle = delegate () { returnedPerson = SearchByName(); };
        Del addNewHandle = delegate () { returnedPerson = AddNewPerson(); };

        bool exit = false;
        while (!exit && returnedPerson is null)
        {
            exit = DisplayMenu(description,
                               searchIdHandle,
                               searchNameHandle,
                               addNewHandle);
        }
        return returnedPerson;
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