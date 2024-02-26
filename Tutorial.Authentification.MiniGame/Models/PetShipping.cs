using System.Text.Json.Serialization;
using static Tutorial.Authentification.MiniGame.Models.Pet;

namespace Tutorial.Authentification.MiniGame.Models
{
	public class PetShipping
	{
		public string Name { get; set; } = string.Empty;

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PetType TypePet { get; set; } = PetType.None;
	}
}
