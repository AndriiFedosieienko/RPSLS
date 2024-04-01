using System.ComponentModel.DataAnnotations.Schema;

namespace RPSLS.Domain.Entities
{
	public class Rule
	{
		public int Id { get; set; }

		public int WinnerChoiceId { get;set; }

		public int LooserChoiceId { get; set; }

		[ForeignKey("WinnerChoiceId")]
		public Choice WinnerChoice { get; set; }

		[ForeignKey("LooserChoiceId")]
		public Choice LooserChoice { get; set; }
	}
}
