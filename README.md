# Trainee_Test_ASP_LINQ_Algorithm
## 1. Task for ASP.NET programmer
It is necessary to write a small web application that will display a list of authors, allow them to be edited and added.
The author has the following attributes:
• Last name, First name, Patronymic - text fields (all fields are required);
•	Date of Birth
• List of books - each book has its own attributes (title, genre, number of pages)
The data must be stored in a database that must conform to third normal form.
1.1 Requirements for the result
The result should be a working application and a script to create the database.
Adding books must be implemented without reloading the page, through a pop-up form. Saving all the entered data occurs by 
clicking on the "Save" button, before that, data about the added books should not get into the database.
The use of MVC Razor, jQuery, Entity Framework will be a plus.
1.2 Input parameters
The editing form should correspond to the prototype in Fig. 1

## 2.	LINQ
Group the unique words of same length in a given sentence, sort the groups in increasing order and display the groups, the word count and the words of that length.

Example:
Input:
“The “C# Professional” course includes the topics I discuss in my CLR via C# book and 
teaches how the CLR works thereby showing you how to develop applications and reusable components for the .NET Framework.”
Output:
Words of length: 1, Count: 1
        I
Words of length: 2, Count: 4
        in
        my
        C#
        to
Words of length: 3, Count: 9
        The
        "C#
        the
        CLR
        via
        and
        how
        you
        for
Words of length: 4, Count: 2
        book
        .NET
Words of length: 5, Count: 1
        works
Words of length: 6, Count: 2
        course
        topics
Words of length: 7, Count: 5
        discuss
        teaches
        thereby
        showing
        develop
Words of length: 8, Count: 2
        includes
        reusable
Words of length: 10, Count: 2
        components
        Framework.
Words of length: 12, Count: 1
        applications
Words of length: 13, Count: 1
        Professional"

## 3. Write a program to remove duplicates from a sorted int[]

INPUT: int[] {1,2,3,4,4,56}

OUTPUT: int [] {1,2,3,4,56}

You are not allowed to modify an existing array.
You are not allowed to use LINQ.

Think about time and space complexity.

## 4. Write a method to find the nth Fibonacci number
in the Fibonacci sequence both iterative and recursive ways. 
Describe the time and space complexity (O) of each implementation.
