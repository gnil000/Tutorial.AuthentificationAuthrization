using Tutorial.Authentification.MiniGame.Models;
using static Tutorial.Authentification.MiniGame.Models.Pet;

namespace Tutorial.Authentification.MiniGame.Storage
{
    public class Pets
    {
        private List<Pet> _pets;

        public Pets()
        {
            _pets = new();
        }

        public Pet? GetPet(string masterLogin)
        {
            var pet = _pets.FirstOrDefault(x => x.MasterLogin == masterLogin);
            if(pet == null)
            {
                return null;
            }
            pet.HungerCalculate();
			return pet;
        }

        public bool AddPet(string masterLogin, PetType petType, string petName)
        {
            if (GetPet(masterLogin) == null)
            {
                _pets.Add(new Pet() { MasterLogin = masterLogin, TypePet = petType, Name = petName });
                return true;
            }
            return false;
        }

		public Pet? Feeding(string masterLogin)
		{
			var pet = _pets.FirstOrDefault(x => x.MasterLogin == masterLogin);
            if(pet == null)
                return null;
            pet.Feding();
            return pet;
		}
	}
}
