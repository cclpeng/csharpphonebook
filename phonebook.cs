using System;

/* Changes to be made: Display alphabetically?
 * Check inputs are valid or else theyll make it crash :/
 * ask for a file name, if file given, read file into the data structure
 * Make a makefile
 */
namespace Phonebook
{
	class Contact	
	{
		private string fname, lname; //first, last names
		private long num;            //phone number

		public Contact()  
		{ 
			fname = ""; 
			lname = ""; 
			num = -1;
		} //default constructor

		public Contact(string f, string l, long n) 
		{
			fname = f;
			lname = l;
			num = n;
		} //constructor for when we have the info separately and are ready

		public string getFname() { return fname; }
		public string getLname() { return lname; }
		public long getNumber() { return num; }
		public void copyContact(Contact rhs)
		{
			fname = rhs.fname;
			lname = rhs.lname;
			num = rhs.num;
		} //using this function to basically do contact a = contact b
	} //end class Contact

	class Book
	{
		private Contact[] ar; //making this a hash table
		private int size;     //total (m)
		private int nelem;    //current # of elements (n)
							  //resizing when n/m == 1/2, half full
		public Book()
		{
			size = 3;
			nelem = 0;
			ar = new Contact[size];         //if you do Contact[]ar,
			//even tho same name as ln 49, it's a DIFFERENT VARIABE
			for(int i = 0; i < size; i++)
				ar[i] = new Contact();
		} //constructor()

		public void formatnum(out long num)
		{
			string number = Console.ReadLine();
			num = Convert.ToInt64(number);
		} //formatnum() just converts the user input string into a long

		public int scorestring(string last)
		{
			char[] ch;
			int len, sum = 0;
			ch = last.ToCharArray();
			len = last.Length;           //*** .Length not .length()
			for(int i = 0; i < len; i++)
				sum += ch[i];
			return sum;
		} //scorestring() how we use last name string as the key

		public int hashfunc(string last, Contact[] ar) //returns an available place
		{
			int score, ind;
			score = scorestring(last);
			ind = score % size;
			if(ar[ind].getNumber() == -1) //this spot is empty, return it
				return ind;
			for(int i = 1; ar[ind].getNumber() != -1; i++)
				ind = (score + i*i) % size;
			return ind;	
		} //hashfunc() applies the hash function and quadratic probing

		public void resize()
		{
			size *= 2;
			Contact[] temp = new Contact[size];
			for(int i = 0; i < size; i++)
				temp[i] = new Contact();
			
			//rehash from old ar to temp
			for(int i = 0; i < size/2; i++)
				if(ar[i].getNumber() != -1) //rehash for filled slots
				{
					int index = hashfunc(ar[i].getLname(), temp);
					temp[index].copyContact(ar[i]);
				}
			
			ar = new Contact[size];					//think this worked
			//just need to create new array first then copy
			for(int i = 0; i < size; i++)
			{
				ar[i] = new Contact();
				ar[i].copyContact(temp[i]);
			}
		} //resize() resizes Book's ar to 2x and rehashes everything

		public void prompt2(out string f, out string l)
		{
			Console.WriteLine("Enter the first name:");
			f = Console.ReadLine();
			Console.WriteLine("Enter the last name: ");
			l = Console.ReadLine();
		} //prompt for the first and last name used in 2 functions

		public void addContact()
		{
			long num;
			string first, last;
			prompt2(out first, out last);
			Console.WriteLine("Enter the phone number: ");
			formatnum(out num); 

			Contact contact = new Contact(first, last, num);

			if(nelem >= (size / 2))
				resize(); //double the size of ar, and rehash elements

			int j = hashfunc(last, ar); //find a slot in the table 
			ar[j].copyContact(contact); 
			nelem++;	  // update the # of elements
		}

		public void deleteContact()
		{
			int i;
			i = search();  //call search function to see if exists
			if(i == -1)
			{
				Console.WriteLine("Contact not found.");
				return;
			}

			Contact c = new Contact(); //create contact w default values
			ar[i].copyContact(c);      //copying over deletes old values
		} // deleteContact()

		public void displayAll(int sz)
		{
			for(int i = 0; i < sz; i++)
			{
				if(ar[i].getFname() != "")
				{
					Console.WriteLine("{0} {1}, {2}", 
				 		ar[i].getFname(), ar[i].getLname(), 
				 		ar[i].getNumber());
				}
			}	
		} // displayAll()

		public int search()
		{	
			int score;
			string first, last;
			prompt2(out first, out last); //ask user for first and last names

			score = scorestring(last);
			int ind = score % size;
			if(String.Compare(ar[ind].getFname(), first) == 0)
			{
				if(String.Compare(ar[ind].getLname(), last) == 0) 
					return ind;
			}

			for(int i = 1; ar[ind].getNumber() != -1; i++) 
			{			   //while no empty spaces
				if(String.Compare(ar[ind].getFname(), first) == 0)
					if(String.Compare(ar[ind].getLname(), last) == 0) 
						return ind;
				ind = (score + i*i) % size;	//calculate the next index to check
			} //for each quadratic probe that's not empty

			return -1;	//if still here, didnt find it
		} // search()

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
		} // prompt()

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
						displayAll(size);
						break;
					case 5:
						break;
				} //switch statement
			} //while loop to keep getting choices

			return size;
		} // loop() function
	} // end class Book

	class Execute
	{
		static void Main()
		{
			Book book = new Book();
			int size = book.loop();
			// book.ar[0].copyContact(a); *CANT do this cuz ar is private
			Console.WriteLine("The phonebook at the end looks like this:");
			book.displayAll(size);
		}
	} //class execute
}