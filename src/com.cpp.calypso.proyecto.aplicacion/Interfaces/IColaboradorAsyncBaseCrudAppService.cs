using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IColaboradorAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Colaborador, ColaboradorDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradorDto> Listar();
        bool buscarcolaborador(string cedula);

        Colaborador buscarcolaboradortcu(string tcu);
        UsuarioDto buscarusuarioporcedula(string cedula);

        List<DetallePreciario> items_ingenieria_contrato(int contratoid);

        int CrearColaborador(Colaborador colaborador);
        int EditColaborador(Colaborador e);
        bool DeleteColaborador(int id);
        List<ColaboradorDto> ListAll();

        List<CatalogoDto> ListTypesUser();


        

    }
}
