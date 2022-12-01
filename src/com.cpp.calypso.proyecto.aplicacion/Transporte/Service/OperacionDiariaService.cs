using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Newtonsoft.Json.Linq;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class OperacionDiariaAsyncBaseCrudAppService : AsyncBaseCrudAppService<OperacionDiaria, OperacionDiariaDto, PagedAndFilteredResultRequestDto>, IOperacionDiariaAsyncBaseCrudAppService
    {
        public IBaseRepository<OperacionDiaria> Repository { get; }

        public OperacionDiariaAsyncBaseCrudAppService(
            IBaseRepository<OperacionDiaria> repository
            ) : base(repository)
        {
            Repository = repository;
        }

        public JArray Sync(int version, JArray registrosJson, List<int> usuariosId)
        {
            throw new NotImplementedException();
        }
    } }

