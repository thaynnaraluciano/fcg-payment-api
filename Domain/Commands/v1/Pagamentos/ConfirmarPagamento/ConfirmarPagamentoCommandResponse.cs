using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands.v1.Pagamentos.ConfirmarPagamento
{
    public class ConfirmarPagamentoCommandResponse
    {
        public Guid Id { get; set; }
        public StatusPagamento Status { get; set; }
    }
}
