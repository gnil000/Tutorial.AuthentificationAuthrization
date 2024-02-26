namespace Tutorial.Authentification.MiniGame.Models
{
	public class Pet
	{
		public bool IsAlive { get; private set; } = true;

		public string MasterLogin { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public PetType TypePet { get; set; } = PetType.None;
		public DateTime FeedingTime { get; set; } = DateTime.Now;
		public int Hunger { get; set; } = 100;

		private const int food = 10;

		public bool Feding()
		{
			HungerCalculate();
			FeedingTime = DateTime.Now;
			if (Hunger + food > 100)
			{
				Hunger = 100;
				return true;
			}
			if (Hunger == 0)
			{
				IsAlive = false;
				return false;
			}
			Hunger += food;
			return true;
		}

		public void HungerCalculate()
		{
			var time = (int)DateTime.Now.Subtract(FeedingTime).TotalMinutes;
			Hunger = 100 - food * time;
			if(Hunger <= 0)
				IsAlive = false;
		}

		public enum PetType
		{
			None,
			Cat,
			Dog, 
			Dino
		}
	}
}
