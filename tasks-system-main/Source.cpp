#include<iostream>
#include<fstream>
#include<list>
#include<string>
#include<algorithm>

using namespace std;

#define DATA_FILE_PATH "data/data.txt"                   // path of file in pc

struct task                                              // task details
{
    string task_title, desc, start_date, ending_date, emp_name;
    bool task_done = 0;
    int prio, emp_id;
};

list <task> emp[200]; 

void addToTextFile(task data, string filePath)        // function to put data in the file
{
    ofstream empTasks;
    empTasks.open(filePath, ios::out | ios::app);
    empTasks << "Task Data: \n\n";
    empTasks <<"Task title: " <<data.task_title <<"\t\t\t\t " << "Task descreption: " <<data.desc <<  "\n\n" 
        << "Employee name: " <<data.emp_name << "\t\t\t\t " << "Employee id: " <<data.emp_id << "\n\n"
        <<"Start date: " <<data.start_date << "\t\t\t\t " << "End date: " << data.ending_date << "\n\n" 
        << "Task priority: " << data.prio << "\t\t\t\t " << "Task state: " << data.task_done << "\n\n ";
    empTasks.close();
}

void Sort(list<task>& emp, task new_task)         // function to sort list of tasks depending on task priority
{
    list <task>::iterator it2;
    
        int size_before = emp.size();             // size before add task to the list
        for (it2 = emp.begin(); it2 != emp.end(); it2++)
        {
            if (new_task.prio > it2->prio)
            {
                emp.insert(it2, 1, new_task);     // insert task before the address of iterator if its priority                                              
                break;                            // is bigger than iterator priority
            }
        }
        if (size_before == emp.size())            // if size didnt change then we will add task at the end of list
        {                                         // because it has the smallest priority
            emp.push_back(new_task);
        }
}

void input_task()                                // input data of tasks
{
    task new_task;
    int id, numOfTasks;
    cout << "Enter number of tasks you want to add: ";
    cin >> numOfTasks;
    cout << endl;
    
    for (int i = 0; i < numOfTasks; i++)
    {
        cout << "Enter your task title: ";
        cin >> new_task.task_title;
        cout << "\n\n";

        cout << "Enter your task Description: ";
        cin.ignore();
        getline(cin, new_task.desc);
        cout << "\n\n";

        cout << "Enter your task assigning date: ";
        cin.ignore();
        getline(cin, new_task.start_date);
        cout << "\n\n";

        cout << "Enter your task deadline date: ";
        cin.ignore();
        getline(cin, new_task.ending_date);
        cout << "\n\n";

        cout << "Enter the priority level(1:10): ";
        while(true)                                          // check that priority is between 1 and 10
        {
            cin >> new_task.prio;
            if (new_task.prio >= 1 && new_task.prio <= 10)
            {
                break;
            }
            else 
            {
                cout << "invalid number!! enter the priority level(1:10): \n";
            }
        }
        cout << "\n\n";

        cout << "Enter employee name:  ";
        cin.ignore();
        getline(cin, new_task.emp_name);
        cout << "\n\n";

        cout << "Enter employee ID: ";
        cin >> id;
        new_task.emp_id = id;
        cout << "\n\n";

        Sort(emp[id], new_task);                           // sort list after add new task
        addToTextFile(new_task, DATA_FILE_PATH);           // add task to our file     
        cout <<"**************************************";
        cout << "\n\n";
     }
}

void display_emp_list(int id)                                 // display list of tasks for a specific employee
{
    list<task>::iterator it = emp[id].begin();

    while (it != emp[id].end()) {

        cout << " Task Title: " << it->task_title << endl;
        cout << " Employee name: " << it->emp_name << endl;
        cout << " Task's Description: " << it->desc << endl;
        cout << " Strat date: " << it->start_date << endl;
        cout << " Ending date: " << it->ending_date << endl;
        cout << " Level: " << it->prio << endl;
        cout << " Done: " << it->task_done;
        cout << "\n\n";
        cout << "**************************************";
        cout << "\n\n";
        it++;
    }

}
void edit(int id, string task_name, int new_prio)           // function to edit priority of specific task of an employee 
{
    list<task>::iterator it;
    for (it = emp[id].begin(); it != emp[id].end(); it++)
    {
        if (it->task_title == task_name) {                  // search for task name in the list
            it->prio = new_prio;
            Sort(emp[id], *it);                             // after edit sort list according to new priority            
            emp[id].erase(it);                              // delete task of old priority
            break;
        }
    }
}

void Delete_Task(int id)                                    // function to delete task of sepcific employee
{   
        emp[id].begin()->task_done = 1;                 
        emp[id].erase(emp[id].begin());
        cout << "Congratulations!\n\n";
        cout << "The next task is: ";
        cout << emp[id].begin()->task_title << "\n\n";
        cout << "**************************************" << endl;
}

int main()
{

    for (;;)
    {
        int number_of_tasks = 0, option, id, index;
        cout << "Enter what you want to do! " << "\n";
        cout << "1- Read information of new task" << "\n";
        cout << "2- Display list of tasks of any employee" << "\n";
        cout << "3- Delete and remaining of taskany task" << "\n";
        cout << "4- Edit priority" << "\n";
        cout << "5- Exit \n";

        cin >> option;
        cout << "\n\n";

        switch (option)
        {
        case 1:
        {
            input_task();
            number_of_tasks++;
            break;
        }
        case 2:
        {
            cout << "Enter the id of employee who wants your information: ";
            cin >> id;
            cout << "\n\n";
            display_emp_list(id);
            break;
        }

        case 3:
        {
            int id;
            cout << "Enter the id of task which finished: ";
            cin >> id;
            cout << "\n\n";
            Delete_Task(id);
            break;
        }
        case 4:
        {
            int newpriority;
            string title;
            cout << "Enter the id of employee: ";
            cin >> id;
            cout << "\n";
            cout << "Enter the title of the task: ";
            cin >> title;
            cout << "\n";
            cout << "Enter the new priority: ";
            cin >> newpriority;  
            cout << "\n";
            edit(id, title, newpriority);
        }

        case 5:
        {
            break;
        }
        default:
        {
            cout << "Plese, Enter a correct choice: " << "\n";
            break;
        }

        }
        if (option == 5)
        {
            cout << "Thank you very much!!" << "\n";
            break;
        }

    }
    return 0;
}