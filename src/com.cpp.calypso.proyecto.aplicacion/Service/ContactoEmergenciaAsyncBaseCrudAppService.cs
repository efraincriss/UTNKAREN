using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ContactoEmergenciaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ContactoEmergencia, ContactoEmergenciaDto, PagedAndFilteredResultRequestDto>, IContactoEmergenciaAsyncBaseCrudAppService
    {
        public ContactoEmergenciaAsyncBaseCrudAppService(
            IBaseRepository<ContactoEmergencia> repository
            ) : base(repository)
        {
        }


        public List<ContactoEmergenciaDto> GetByColaboradorId(int colaboradorId)
        {
            var e = 1;
            var query = Repository.GetAll()
                .Where(o => o.ColaboradorId == colaboradorId && o.IsDeleted == false);

            var contactos = (from d in query
                               select new ContactoEmergenciaDto
                               {
                                   Id = d.Id,
                                   ColaboradorId = d.ColaboradorId,
                                   Nombres = d.Nombres,
                                   Identificacion = d.Identificacion,
                                   Relacion = d.Relacion,
                                   UrbanizacionComuna = d.UrbanizacionComuna,
                                   Direccion = d.Direccion,
                                   Telefono = d.Telefono,
                                   Celular = d.Celular,
                                   nombre_relacion = d.CatalogoRelacion.nombre
                               }).ToList();

            foreach (var c in contactos) {
                c.nro = e++;
            }

            return contactos;
        }

        public int CreateContacto(ContactoEmergenciaDto contacto)
        {
            var c = Mapper.Map<ContactoEmergenciaDto, ContactoEmergencia>(contacto);

            if (c.Id > 0)
            {
                var ce = Repository.Get(c.Id);
                ce.Nombres = c.Nombres;
                ce.Identificacion = c.Identificacion;
                ce.Relacion = c.Relacion;
                ce.UrbanizacionComuna = c.UrbanizacionComuna;
                ce.Direccion = c.Direccion;
                ce.Telefono = c.Telefono;
                ce.Celular = c.Celular;
                var result = Repository.Update(ce);
                return result.Id;
            }
            else {

                var existe = Repository.GetAll().Where(e => e.ColaboradorId == c.ColaboradorId && e.Identificacion == c.Identificacion && e.IsDeleted == false).FirstOrDefault();
                if(existe != null)
                {
                    return -1;
                }
                else
                {
                    var result = Repository.InsertAndGetId(c);
                    return result;
                }

                
            }
            
        }

        public string EliminarContacto(int id)
        {
            var contacto = Repository.Get(id);
            contacto.IsDeleted = true;

            Repository.Update(contacto);

            return "OK";
        }

    }
}
