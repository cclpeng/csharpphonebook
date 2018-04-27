using System;

/* Later changes: use a hash table
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
			num = -1;
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
		private Contact[] ar; //making this a hash table
		int size;
		int nelem;

		public Book()
		{
			size = 3;
			nelem = 0;
			ar = new Contact[size];         //if you do Contact[]ar,
			//even tho same name as ln 49, it's a DIFFERENT VARIABE
			for(int i = 0; i < size; i++)
				ar[i] = new Contact();
		}

		public void formatnum(out long num)
		{
			string number = Console.ReadLine();
			num = Convert.ToInt64(number);
		}

		public int scorestring(string last)
		{
			char[] ch;
			int len, sum = 0;
			ch = last.ToCharArray();
			len = last.Length;           //*** .Length not .length()
			for(int i = 0; i < len; i++)
				sum += ch[i];
			return sum;
		}

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
		}

		public void resize()
		{
			//double size
			//create a new array...how to overwrite Book's array??
			//go through ar[i].getName() != nulls and rehash them with new size
			size *= 2;
			Contact[] temp = new Contact[size];
			for(int i = 0; i < size; i++)
				temp[i] = new Contact();
			Console.WriteLine("eh?");
			//rehash from old ar to temp
			for(int i = 0; i < size/2; i++)
				if(ar[i].getNumber() != -1) //rehash for filled slots
				{
					int index = hashfunc(ar[i].getLname(), temp);
					temp[index].copyContact(ar[i]);
				}
			Console.WriteLine("Still okay");
			ar = new Contact[size];					//think this worked
			//just need to create new array first then copy
			for(int i = 0; i < size; i++)
			{
				ar[i] = new Contact();
				ar[i].copyContact(temp[i]);
				Console.WriteLine("SStill okay");
			}
			Console.WriteLine("I lived thru reisze");
		} //resize()

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

			if(nelem >= (size / 2))
			{
				resize();
				Console.WriteLine("I resized and size is now {0}", size);
			}
			nelem++;

			int j = hashfunc(last, ar);
			ar[j].copyContact(contact);
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

			Contact c = new Contact();
			ar[i].copyContact(c);
		} // deleteContact()

		public void displayAll(int sz)
		{
			for(int i = 0; i < sz; i++)
			{
				if(ar[i].getFname() != "")
				{
					Console.WriteLine("{0}, {1}, {2}", 
				 		ar[i].getFname(), ar[i].getLname(), 
				 		ar[i].getNumber());
				}
			}	
		} // displayAll()

		public int search()
		{	
			int score;
			string first, last;
			Console.WriteLine("Enter first name:");
			first = Console.ReadLine();
			Console.WriteLine("Enter last name:");
			last = Console.ReadLine();
			//what we can do is...increment i for the i^2 until
			//either found item or ar[i] is null. cuz if ar[i] is 
			//null, then we havent found the contact and we reached
			//the end of the search pretty much. Because of
			//rehashings from addcontact(), table will always have
			//empty spaces
			score = scorestring(last);
			int ind = score % size;
			if(String.Compare(ar[ind].getFname(), first) == 0)
			{
				if(String.Compare(ar[ind].getLname(), last) == 0) 
					return ind;
			}

			else
			{
				for(int i = 1; ar[ind].getNumber() != -1; i++) //no empty space
				{
					if(String.Compare(ar[ind].getFname(), first) == 0)
						if(String.Compare(ar[ind].getLname(), last) == 0) 
							return ind;
					ind = (score + i*i) % size;
				} //for each quadratic probe that's not empty

				return -1;	//if still here, didnt find it
			} // else

			return -1;
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