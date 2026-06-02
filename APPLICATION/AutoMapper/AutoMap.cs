using AutoMapper;
using APPLICATION.DTO;
using DOMAIN.Entities;

namespace APPLICATION.AutoMapper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            // Entity → DetailDTO
            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<Product, ProductPatchDTO>().ReverseMap();
        }
    }
}