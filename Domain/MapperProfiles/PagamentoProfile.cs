using AutoMapper;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;
using Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos;
using Domain.Commands.v1.Pagamentos.CancelarPagamento;
using Domain.Commands.v1.Pagamentos.ConfirmarPagamento;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using Domain.Enums;
using Infrastructure.Data.Models;

namespace Domain.MapperProfiles
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<PagamentoModel, CriarPagamentoCommandResponse>();
            CreateMap<PagamentoModel, BuscarPagamentoPorIdCommandResponse>();
            CreateMap<PagamentoModel, BuscarPagamentoPorUsuarioCommandResponse>();
            CreateMap<PagamentoModel, BuscarTodosPagamentosCommandResponse>();
            CreateMap<PagamentoModel, CancelarPagamentoCommandResponse>();
            CreateMap<PagamentoModel, ConfirmarPagamentoCommandResponse>();

            CreateMap<CriarPagamentoCommand, PagamentoModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => StatusPagamento.Pendente))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
