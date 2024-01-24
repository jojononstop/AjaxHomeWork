namespace Ajax0122.Models.Dtos
{
	public class UserDtos
	{
		public string? Name { get; set; }
		public string? Email { get; set; }
		public int Age { get; set; } = 29;

		public IFormFile? Avator { get; set; }
	}
}
