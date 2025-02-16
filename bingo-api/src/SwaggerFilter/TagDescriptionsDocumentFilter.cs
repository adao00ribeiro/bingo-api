using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace bingo_api.src.SwaggerFilter;

public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = new List<OpenApiTag> {
            new OpenApiTag { Name = "Identity", Description = "" },
            new OpenApiTag { Name = "Seller", Description = "" },
            new OpenApiTag { Name = "Room", Description = "" },
            new OpenApiTag { Name = "Prize", Description = "" },
            new OpenApiTag { Name = "Round", Description = "" },
            new OpenApiTag { Name = "RoomSeller", Description = "" },
            new OpenApiTag { Name = "Punter", Description = "" },
            new OpenApiTag { Name = "Card", Description = "" },
            new OpenApiTag { Name = "CardBuy", Description = "" },
            new OpenApiTag { Name = "CardWinner", Description = "" },
            new OpenApiTag { Name = "Recharge", Description = "" },

        };
    }
}
