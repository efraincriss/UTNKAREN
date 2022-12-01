using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Service
{
    public class SeccionAsyncBaseCrudAppService : AsyncBaseCrudAppService<Seccion, SeccionDto, PagedAndFilteredResultRequestDto>, ISeccionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<ImagenSeccion> _imagenSeccionRepository;
        public SeccionAsyncBaseCrudAppService(IBaseRepository<Seccion> repository, IBaseRepository<ImagenSeccion> imagenSeccionRepository) : base(repository)
        {
            _imagenSeccionRepository = imagenSeccionRepository;
        }

        public List<EstructuraArbol> GenerarArbol(int documentoId)
        {
            var i = Repository.GetAll()
                .Include(o => o.SeccionPadre)
                .Where(c => c.SeccionPadre == null)
                .Where(o => o.DocumentoId == documentoId);

            var items_reordenados = (from e in i.ToList()
                                     orderby Convert.ToInt32(e.Codigo.Replace(".", ""))
                                     select e).ToList();

            var Lista = new List<EstructuraArbol>();
            foreach (var x in items_reordenados)// i.ToList()
            {
                var item = GenerarNodos(x);
                Lista.Add(item);
            }
            return Lista;
        }

        public EstructuraArbol GenerarNodos(Seccion seccion)
        {
            List<Seccion> hijos = ObtenerHijos(seccion.Codigo, seccion.DocumentoId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<EstructuraArbol>();
                var items_reordenados = (from e in hijos.ToList()
                                         orderby Convert.ToInt32(e.Codigo.Replace(".", ""))
                                         select e).ToList();
                foreach (var h in items_reordenados)
                {
                    var lhijos = GenerarNodos(h);
                    lista_hijos.Add(lhijos);
                }
                return new EstructuraArbol()
                {
                    key = seccion.Id,
                    label = seccion.NombreSeccion + "   - pag. " + seccion.NumeroPagina,
                    data = seccion.NombreSeccion + "," + seccion.NumeroPagina,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    children = lista_hijos,
                    selectable = true,
                    draggable = true,
                    droppable = true,
                };
            }
            else
            {
                return new EstructuraArbol()
                {
                    key = seccion.Id,
                    label = seccion.NombreSeccion + "   - pag. " + seccion.NumeroPagina,
                    data = seccion.NombreSeccion + "," + seccion.NumeroPagina,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    children = new List<EstructuraArbol>(),
                    selectable = true,
                    draggable = true,
                    droppable = true,
                };

            }
        }

        public List<Seccion> ObtenerHijos(string codigo_padre, int documentoId)
        {
            var items = Repository.GetAll()
                .Include(o => o.SeccionPadre)
                .Where(o => o.DocumentoId == documentoId)
                .Where(o => o.SeccionPadre.Codigo == codigo_padre)
                .ToList();
            return items;
        }

        public async Task<bool> CrearSeccionAsync(SeccionDto dto)
        {
            var entity = Mapper.Map<Seccion>(dto);
            if (!entity.SeccionPadreId.HasValue)
            {
                entity.Codigo = GenerarCodigoPadresPrimerNivel(entity.DocumentoId);
            }
            else
            {
                var seccionPadre = Repository.Get(entity.SeccionPadreId.Value);
                entity.Codigo = GenerarCodigosHijos(seccionPadre, entity.DocumentoId);
            }
            if (dto.Contenido != null && dto.Contenido.Length > 0)
            {
                 byte[] data = Convert.FromBase64String(dto.Contenido);
                 string decodedString = Encoding.UTF8.GetString(data);
                  entity.Contenido = decodedString;
                //entity.Contenido = dto.Contenido;
            }
            else
            {
                entity.Contenido = "";
            }
            await Repository.InsertAsync(entity);
            return true;
        }

        public bool ActualizarSeccion(SeccionDto dto)
        {
            var entity = Mapper.Map<Seccion>(dto);
            if (dto.Contenido != null && dto.Contenido.Length > 0)
            {
                byte[] data = Convert.FromBase64String(dto.Contenido);
                string decodedString = Encoding.UTF8.GetString(data);


                entity.Contenido = decodedString;
            }
            else
            {
                entity.Contenido = "";
            }
            Repository.Update(entity);
            return true;
        }

        public ResultadoEliminacionResponse EliminarSeccion(int id)
        {
            var tieneHijos = Repository.GetAll()
                .Where(o => o.SeccionPadreId == id).ToList();
            var imagenesSeccion = _imagenSeccionRepository.GetAll().Where(c => c.SeccionId == id).ToList();
            if (imagenesSeccion.Count > 0)
            {
                foreach (var imagenseccion in imagenesSeccion)
                {
                    _imagenSeccionRepository.Delete(imagenseccion.Id);

                }
            }

            if (tieneHijos.Count() > 0)
            {

                foreach (var seccion in tieneHijos)
                {
                    Repository.Delete(seccion.Id);

                }
                /*return new ResultadoEliminacionResponse
                {
                    Eliminado = false,
                    Error = "La sección seleccionada tiene contenido asociado"
                };*/

            }
          
            Repository.Delete(id);



            return new ResultadoEliminacionResponse
            {
                Eliminado = true,
                Error = ""
            };

        }
        public ResultadoEliminacionResponse EliminarImagen(int id)
        {

            _imagenSeccionRepository.Delete(id);
            return new ResultadoEliminacionResponse
            {
                Eliminado = true,
                Error = ""
            };

        }


        public SeccionDto ObtenerSeccionPorId(int id)
        {
            var seccion = Repository.Get(id);
            var seccionDto = Mapper.Map<SeccionDto>(seccion);

            var imagenesPorSeccion = _imagenSeccionRepository.GetAll().Where(s => s.SeccionId == seccion.Id).ToList();
            seccionDto.Imagenes = imagenesPorSeccion;

            return seccionDto;
        }

        public string GenerarCodigosHijos(Seccion seccionPadre, int documentoId)
        {

            int count = Repository.GetAll()
                .Where(o => o.DocumentoId == documentoId)
                .Where(o => o.SeccionPadreId == seccionPadre.Id)
                .Count();

            count = count + 1;

            return seccionPadre.Codigo + count + ".";
        }

        public string GenerarCodigoPadresPrimerNivel(int documentoId)
        {
            int count = Repository.GetAll()
                .Where(o => o.DocumentoId == documentoId)
                .Where(o => o.SeccionPadre == null)
                .Count();

            count = count + 1;

            return count + ".";
        }

        public bool GuardarArbolDragDrop(List<EstructuraArbol> data)
        {
            if (data != null && data.Count > 0)
            {
                int contador = 1;
                foreach (var item in data)
                {
                    var padre = Repository.Get(item.key);
                    padre.SeccionPadreId = null;
                    padre.Codigo = contador + ".";
                    var resultado = Repository.Update(padre);
                    if (item.children != null && item.children.Count > 0)
                    {
                        var x = this.GuardarHijoDrag(item.children, padre);
                    }
                    contador++;
                }
            }
            return true;
        }

        public string GuardarHijoDrag(List<EstructuraArbol> data, Seccion padre)
        {
            if (data != null && data.Count > 0)
            {
                var contador = 1;
                foreach (var item in data)
                {
                    var seccionHija = Repository.Get(Int32.Parse("" + item.key));
                    seccionHija.SeccionPadreId = padre.Id;
                    seccionHija.Codigo = padre.Codigo + contador + ".";
                    var resultado = Repository.Update(seccionHija);
                    if (item.children != null && item.children.Count > 0)
                    {
                        var x = GuardarHijoDrag(item.children, seccionHija);
                    }
                    contador++;
                }
            }
            return "OK";
        }

        public List<SeccionDto> ObtenerSeccionesFiltros(int carpetaid, string tipoDocumento, int DocumentoId, int SeccionId, string palabra, bool soloTitulos)
        {
            var secciones = Repository.GetAllIncluding(c => c.Documento, c => c.Documento.TipoDocumento);
            if (carpetaid > 0)
            {
                secciones = secciones.Where(c => c.Documento.CarpetaId == carpetaid);
            }
            if (DocumentoId > 0)
            {
                secciones = secciones.Where(c => c.DocumentoId == DocumentoId);
            }
            if (tipoDocumento.Length > 0)
            {
                secciones = secciones.Where(c => c.Documento.TipoDocumento.codigo == tipoDocumento);
            }
            if (SeccionId > 0)
            {
                secciones = secciones.Where(c => c.Id == SeccionId);
            }
            if (palabra.Length > 0)
            {
                if (soloTitulos)
                {
                    secciones = secciones.Where(c => c.NombreSeccion.Contains(palabra));
                }
                else
                {
                    secciones = secciones.Where(c => c.NombreSeccion.Contains(palabra) || c.Contenido.Contains(palabra));
                }
            }

            var data = secciones.ToList();
            var dataDto = new List<SeccionDto>();
            foreach (var seccion in data)
            {
                var seccionDto = Mapper.Map<SeccionDto>(seccion);
                seccionDto.nombreDocumento = seccion.Documento.Nombre;
                seccionDto.NombreSeccion = obtenerNombreSeccionPath(seccion.Id);
                if (palabra.Length > 0)
                {
                    //seccionDto.contenidoCorto = seccion.Contenido_Plano.Substring(this.PositionInicial(seccion.Contenido_Plano, palabra), seccion.Contenido_Plano.Length > 10 ? 10 : seccion.Contenido_Plano.Length);

                }
                dataDto.Add(seccionDto);
            }
            return dataDto;
        }

        public string obtenerNombreSeccionPath(int seccionId)
        {
            var path = obtenerNombreSeccionesRecursivo(seccionId);
            var pathArray = path.Split(',');
            Array.Reverse(pathArray);
            var pathString = String.Join(" / ", pathArray);
            return pathString;
        }

        public string obtenerNombreSeccionesRecursivo(int seccionId)
        {
            var seccion = Repository.Get(seccionId);
            string path = "";

            if (seccion.SeccionPadreId.HasValue)
            {
                path = seccion.NombreSeccion + "," + obtenerNombreSeccionesRecursivo(seccion.SeccionPadreId.Value);
                return path;
                
            } else
            {
                return path += seccion.NombreSeccion;
            }
        }

        public void actualizarSeccion()
        {
            var query = Repository.GetAll().ToList();
            int count = 0;
            foreach (var item in query)
            {

                var s = Repository.Get(item.Id);
                string cadenaSinTags = Regex.Replace(s.Contenido, "<.*?>", string.Empty).Replace("&lt;", string.Empty).Replace("&gt;", string.Empty);
                s.Contenido_Plano = cadenaSinTags;
                Repository.Update(s);
                count++;

            }
        }
        private int PositionInicial(string contenidoPlano, string searchString)
        {
            string s1 = contenidoPlano;
            string s2 = searchString;
            bool b = s1.Contains(s2);
            if (b)
            {
                int index = s1.IndexOf(s2);
                if (index >= 0)
                {
                    return index;
                }

            }
            return 0;
        }

    }
}
