using System;

namespace CleanArchitecture.Shared.Models.Requests
{
	public class MessageRequest
	{
		public Guid Id { get; set; }

		public string Message { get; set; }
	}
}
