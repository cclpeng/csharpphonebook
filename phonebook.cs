using System;

//add, remove, search, and display
/* most basic: array of contact structs, search just loops through
 * add appends to end of array, remove must loop thru entire array O(n)
 * asks user for first name, last name, number manual enter
 */

/* Later changes: pick different more suitable data structure to implement
 * like hash table (search is O(1))
 * ask for a file name, if file given, read file into the data structure
 * Make a makefile
 */
namespace Phonebook
{
	class Contact	
	{
		private string fname, lname;
		private long num;

		public Contact()
		{ 
			fname = ""; 
			lname = ""; 
			num = 0;
		}

		public Contact(string f, string l, long n) 
		{
			fname = f;
			lname = l;
			num = n;
		}

		public string getFname() { return fname; }
		public string getLname() { return lname; }
		public long getNumber() { return num; }
		public void copyContact(Contact rhs)
		{
			fname = rhs.fname;
			lname = rhs.lname;
			num = rhs.num;
		}
	} //end class Contact

	class Book
	{
		private Contact[] ar;
		int index;

		public Book()
		{
			index = 0;
			ar = new Contact[100];         //if you do Contact[]ar,
			//even tho same name as ln 49, it's a DIFFERENT VARIABE
			for(int i = 0; i < 100; i++)
				ar[i] = new Contact();
		}

		public void formatnum(out long num)
		{
			string number = Console.ReadLine();
			num = Convert.ToInt64(number);
		}

		public void addContact()
		{
			long num;
			string first, last;
			Console.WriteLine("Enter the first name:");
			first = Console.ReadLine();
			Console.WriteLine("Enter the last name: ");
			last = Console.ReadLine();
			Console.WriteLine("Enter the phone number: ");
			formatnum(out num); // you need out not ref because
			//num has no value yet. ur giving it a 
			//value inside formatnum()

			Contact contact = new Contact(first, last, num);
			ar[index++].copyContact(contact);
		}

		public void deleteContact()
		{
			int i;
			i = search();
			if(i == -1)
			{
				Console.WriteLine("Contact not found.");
				return;
			}

			for(int j = i; j < index; j++)
				ar[j].copyContact(ar[j + 1]);
			index--;
		} // deleteContact()

		public void displayAll(int sz)
		{
			for(int i = 0; i < sz; i++)
			{
				Console.WriteLine("{0}, {1}, {2}", 
				 ar[i].getFname(), ar[i].getLname(), 
				 ar[i].getNumber());
			}
			
		}

		public int search()
		{	
			int i, found = 0;
			string first, last;
			Console.WriteLine("Enter first name:");
			first = Console.ReadLine();
			Console.WriteLine("Enter last name:");
			last = Console.ReadLine();
			for(i = 0; i < index; i++)
				if(String.Compare(first, ar[i].getFname()) == 0 
					&& String.Compare(last, ar[i].getLname()) == 0)
				{
					found = 1;
					break;
				}
				
			if(found == 1)
				return i;
			return -1;
		}

		public int prompt()
		{
			string ch;
			Console.WriteLine("Choose an option:");
			Console.WriteLine("1: Add Contact");
			Console.WriteLine("2: Delete Contact");
			Console.WriteLine("3: Search for a Contact");
			Console.WriteLine("4: Display Book");
			Console.WriteLine("5: Quit");
			ch = Console.ReadLine();
			return Convert.ToInt32(ch);
		}

		public int loop()
		{
			int choice = 0;
			while(choice != 5)
			{
				choice = prompt();
				switch(choice)
				{
					case 1:
						addContact();
						break;
					case 2:
						deleteContact();
						break;
					case 3:
						int i = search();
						if(i == -1)
							Console.WriteLine("Contact not found");
						else
							Console.WriteLine("The number is {0}", ar[i].getNumber());
						break;
					case 4:
						displayAll(index);
						break;
					case 5:
						break;
				} //switch statement
			} //while loop to keep getting choices

			return index;
		} // loop() function
	} // end class Book

	class Execute
	{
		static void Main()
		{
			Book book = new Book();
			int size = book.loop();
			// book.ar[0].copyContact(a); *CANT do this cuz ar is private
			book.displayAll(size);
		}
	}
}