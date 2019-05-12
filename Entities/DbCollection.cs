using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entities
{
	public static class DbCollection
	{
		private const string EntitiesNamespace = "Entities.Entities";

		public static IEnumerable<string> All()
		{
			return Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(_ => _.IsClass && _.Namespace == EntitiesNamespace)
				.Select(_ => _.Name)
				.ToList();
		}
	}
}
