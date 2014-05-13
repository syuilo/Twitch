using System.Collections.Generic;

namespace Twitch.Twitter.Application
{
	/// <summary>
	/// Twitter official API keys
	/// </summary>
	public static class OfficialKeys
	{
		public static Key Twitter_for_iPhone
		{
			get
			{
				return new Key
				{
					Name = "Twitter for iPhone",
					ConsumerKey = "IQKbtAYlXLripLGPWd0HUA",
					ConsumerSecret = "GgDYlkSvaPxGxC4X8liwpUoqKwwr3lCADbz8A7ADU"
				};
			}
		}

		public static Key Twitter_for_Android
		{
			get
			{
				return new Key
				{
					Name = "Twitter for Android",
					ConsumerKey = "3nVuSoBZnx6U4vzUxf5w",
					ConsumerSecret = "Bcs59EFbbsdF6Sl9Ng71smgStWEGwXXKSjYvPVt7qys"
				};
			}
			
		}

		public static Key Twitter_for_iPad
		{
			get
			{
				return new Key
				{
					Name = "Twitter for iPad",
					ConsumerKey = "CjulERsDeqhhjSme66ECg",
					ConsumerSecret = "IQWdVyqFxghAtURHGeGiWAsmCAGmdW3WmbEx6Hck"
				};
			}
			
		}

		public static Key Twitter_for_Mac
		{
			get
			{
				return new Key
				{
					Name = "Twitter for Mac",
					ConsumerKey = "3rJOl1ODzm9yZy63FACdg",
					ConsumerSecret = "5jPoQ5kQvMJFDYRNE8bQ4rHuds4xJqhvgNJM4awaE8"
				};
			}
			
		}

		public static Key Twitter_for_Windows_Phone
		{
			get
			{
				return new Key
				{
					Name = "Twitter for Windows Phone",
					ConsumerKey = "yN3DUNVO0Me63IAQdhTfCA",
					ConsumerSecret = "c768oTKdzAjIYCmpSNIdZbGaG0t6rOhSFQP0S5uC79g"
				};
			}
			
		}



		public static Key TweetDeck
		{
			get
			{
				return new Key
				{
					Name = "TweetDeck",
					ConsumerKey = "yT577ApRtZw51q4NPMPPOQ",
					ConsumerSecret = "3neq3XqN5fO3obqwZoajavGFCUrC42ZfbrLXy5sCv8"
				};
			}
			
		}

		public static Key Twitter_for_Android_Sign_Up
		{
			get
			{
				return new Key
				{
					Name = "Twitter for Android Sign-Up",
					ConsumerKey = "RwYLhxGZpMqsWZENFVw",
					ConsumerSecret = "Jk80YVGqc7Iz1IDEjCI6x3ExMSBnGjzBAH6qHcWJlo"
				};
			}
			
		}

		public static List<Key> keys = new List<Key>()
		{
			new Key()
			{
				Name = "Twitter for iPhone",
				ConsumerKey = "IQKbtAYlXLripLGPWd0HUA",
				ConsumerSecret = "GgDYlkSvaPxGxC4X8liwpUoqKwwr3lCADbz8A7ADU"
			},

			new Key()
			{
				Name = "Twitter for Android",
				ConsumerKey = "3nVuSoBZnx6U4vzUxf5w",
				ConsumerSecret = "Bcs59EFbbsdF6Sl9Ng71smgStWEGwXXKSjYvPVt7qys"
			},

			new Key()
			{
				Name = "Twitter for iPad",
				ConsumerKey = "CjulERsDeqhhjSme66ECg",
				ConsumerSecret = "IQWdVyqFxghAtURHGeGiWAsmCAGmdW3WmbEx6Hck"
			},

			new Key()
			{
				Name = "Twitter for Mac",
				ConsumerKey = "3rJOl1ODzm9yZy63FACdg",
				ConsumerSecret = "5jPoQ5kQvMJFDYRNE8bQ4rHuds4xJqhvgNJM4awaE8"
			},

			new Key()
			{
				Name = "Twitter for Windows Phone",
				ConsumerKey = "yN3DUNVO0Me63IAQdhTfCA",
				ConsumerSecret = "c768oTKdzAjIYCmpSNIdZbGaG0t6rOhSFQP0S5uC79g"
			},

			new Key()
			{
				Name = "TweetDeck",
				ConsumerKey = "yT577ApRtZw51q4NPMPPOQ",
				ConsumerSecret = "3neq3XqN5fO3obqwZoajavGFCUrC42ZfbrLXy5sCv8"
			},

			new Key()
			{
				Name = "Twitter for Android Sign-Up",
				ConsumerKey = "RwYLhxGZpMqsWZENFVw",
				ConsumerSecret = "Jk80YVGqc7Iz1IDEjCI6x3ExMSBnGjzBAH6qHcWJlo"
			},
		};
	}

}
