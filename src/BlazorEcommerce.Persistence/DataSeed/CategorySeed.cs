using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorEcommerce.Persistence.DataSeed;

public class CategorySeed : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        //builder.HasData(
        //        new Category
        //        {
        //            Id = 1,
        //            Name = "Books",
        //            Url = "books",
        //            CreatedUtc = new DateTime(new DateOnly(2024, 11, 1), new TimeOnly(0, 0, 0), DateTimeKind.Utc)
        //        },
        //        new Category
        //        {
        //            Id = 2,
        //            Name = "Movies",
        //            Url = "movies",
        //            CreatedUtc = new DateTime(new DateOnly(2024, 11, 1), new TimeOnly(0, 0, 0), DateTimeKind.Utc)
        //        },
        //        new Category
        //        {
        //            Id = 3,
        //            Name = "Video Games",
        //            Url = "video-games",
        //            CreatedUtc = new DateTime(new DateOnly(2024, 11, 1), new TimeOnly(0, 0, 0), DateTimeKind.Utc)
        //        }
        //        );
    }
}
