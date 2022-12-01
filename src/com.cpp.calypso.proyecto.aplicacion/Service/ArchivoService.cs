using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ArchivoServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Archivo, ArchivoDto, PagedAndFilteredResultRequestDto>, IArchivoAsyncBaseCrudAppService
    {

        public ArchivoServiceAsyncBaseCrudAppService(
            IBaseRepository<Archivo> repository
        ) : base(repository)
        {

        }
        public int InsertArchivo(HttpPostedFileBase x)
        {
            if (x != null)
            {
                string fileName = x.FileName;
                string fileContentType = x.ContentType;
                byte[] fileBytes = new byte[x.ContentLength];
                var data = x.InputStream.Read(fileBytes, 0,
                Convert.ToInt32(x.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "AUX" + 1,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };

                var archivoid = Repository.InsertAndGetId(n);
                return archivoid;
            }
            else
            {
                return 0;
            }
        }
    }

}
