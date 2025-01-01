using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web_Api.Enums
{
	public class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (schema.Enum != null && schema.Enum.Count > 0)
			{
				schema.Type = "string"; schema.Format = null;
				var enumNames = Enum.GetNames(context.Type);
				schema.Enum.Clear();

				foreach (var enumName in enumNames)
				{
					schema.Enum.Add(new OpenApiString(enumName));
				}
			}
		}
	}
}