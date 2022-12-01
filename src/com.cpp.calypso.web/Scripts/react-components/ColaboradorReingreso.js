import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { Growl } from "primereact/components/growl/Growl";
import BlockUi from "react-block-ui";

import EditarInformacionGeneral from "./Colaboradores/EditarInformacionGeneralReingreso";
import EditarContacto from "./Colaboradores/EditarContacto";
import EditarCondicionesEmpleo from "./Colaboradores/EditarCondicionesEmpleo";
import EditarServicios from "./Colaboradores/EditarServicios";
import EditarCargaSocial from "./Colaboradores/EditarCargaSocial";
import EditarNomina from "./Colaboradores/EditarNomina";
import EditarContactoEmergencia from "./Colaboradores/EditarContactoEmergencia";

import {
  CODIGO_INCAPACIDAD_NOINCAPACITADO,
  CODIGO_SINIESTRO_NOINCAPACITADO,
  PARENTESCO_CONYUGE,
  PARENTESCO_CONVIVIENTE,
  ESTADO_CIVIL_CASADO,
  ESTADO_CIVIL_UNION,
} from "./Colaboradores/Codigos";
import { Card } from "primereact-v2/card";

export default class ReingresoColaborador extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      activeIndex: 0,
      id_colaborador: "",
      destino_estancia: 0,
      colaborador: [],
      contacto: [],
      cargas: [],
      nacionalidades: [],
      disabled: false,
      nro_legajo: 0,
      nro_sap: "No asignado",
      visible: false,
      loading: true,
      parroquias: [],
      provincias: [],
      ciudades: [],
      tiposCategorias: [],
      disable_button: false,
      //Catalogos
      //Informacion General
      tiposIdentificacion: [],
      generos: [],
      etnias: [],
      estados: [],
      tipoDiscapacidad: [],
      codigoSiniestro: [],
      formacionAca: [],
      tiposFormaciones: [],
      tiposTitulos: [],
      tiposNacionalidades: [],
      tiposIncapacidades: [],
      tiposDestino: [],
      tiposProyectos: [],
      tiposParentesco: [],
      //Nomina
      tiposDivisionPersonal: [],
      tiposContrato: [],
      tiposClaseContrato: [],
      tiposNomina: [],
      tiposPeriodoNomina: [],
      tiposFormaPago: [],
      tiposSubGrupo: [],
      tiposBancos: [],
      tiposCuentas: [],
      tiposSubDivisionP: [],
      tiposFuncion: [],
      tiposGrupo: [],
      tiposViaPago: [],
      tiposEncargadoPersonal: [],
      //Condiciones de Empleo
      gruposPersonal: [],
      tiposSectores: [],
      areas: [],
      vinculosLaborales: [],
      clases: [],
      encuadres: [],
      horarios: [],
      planBeneficios: [],
      planSalud: [],
      coberturaDependientes: [],
      planesBeneficios: [],
      asociaciones: [],
      aptosMedico: [],
      tiposSitios: [],
      contactos_emergencia: [],

      empresas: [],

      //Empresa
      empresa_id: 0,

      //Reingreso
      bloquearTabs: true,
    };
    this.childInfo = React.createRef();
    this.childContacto = React.createRef();
    this.childEmpleo = React.createRef();
    this.childServicios = React.createRef();
    this.childCargas = React.createRef();
    this.childNomina = React.createRef();

    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
    this.Regresar = this.Regresar.bind(this);
    this.ChangeTab = this.ChangeTab.bind(this);
    this.ColaboradorId = this.ColaboradorId.bind(this);
    this.ContantoInfo = this.ContantoInfo.bind(this);
    this.Siguiente = this.Siguiente.bind(this);
    this.CargasInfo = this.CargasInfo.bind(this);
    this.ContactosEmergenciaInfo = this.ContactosEmergenciaInfo.bind(this);

    this.GetNacionalidades = this.GetNacionalidades.bind(this);
    this.getFormSelectNacionalidad = this.getFormSelectNacionalidad.bind(this);
    this.GetProvincias = this.GetProvincias.bind(this);
    this.GetCiudades = this.GetCiudades.bind(this);
    this.GetParroquias = this.GetParroquias.bind(this);

    this.ConsultaColaborador = this.ConsultaColaborador.bind(this);
    this.ConsultaContacto = this.ConsultaContacto.bind(this);
    this.ConsultaCargaSocial = this.ConsultaCargaSocial.bind(this);
    this.ConsultaContactosEmergencia = this.ConsultaContactosEmergencia.bind(
      this
    );

    this.validarCedula = this.validarCedula.bind(this);
    this.validarEnvio = this.validarEnvio.bind(this);
    this.VerificaCedula = this.VerificaCedula.bind(this);
    this.VerificaPersonaJuridica = this.VerificaPersonaJuridica.bind(this);
    this.VerificaSectorPublico = this.VerificaSectorPublico.bind(this);

    this.CargaCategoriaEncargado = this.CargaCategoriaEncargado.bind(this);
    this.CargaNomina = this.CargaNomina.bind(this);
    this.getFormSelectGrupo = this.getFormSelectGrupo.bind(this);

    this.GetCatalogos = this.GetCatalogos.bind(this);
    this.cargarCatalogos = this.cargarCatalogos.bind(this);

    //Catalogos Nomina
    this.getFormSelectDivisionPersonal = this.getFormSelectDivisionPersonal.bind(
      this
    );
    this.getFormSelectTipoContrato = this.getFormSelectTipoContrato.bind(this);
    this.getFormSelectClaseContrato = this.getFormSelectClaseContrato.bind(
      this
    );
    this.getFormSelectTipoNomina = this.getFormSelectTipoNomina.bind(this);
    this.getFormSelectPeriodoNomina = this.getFormSelectPeriodoNomina.bind(
      this
    );
    this.getFormSelectFormaPago = this.getFormSelectFormaPago.bind(this);
    this.getFormSelectSubGrupo = this.getFormSelectSubGrupo.bind(this);
    this.getFormSelectBancos = this.getFormSelectBancos.bind(this);
    this.getFormSelectTipoCuenta = this.getFormSelectTipoCuenta.bind(this);
    this.getFormSelectViaPago = this.getFormSelectViaPago.bind(this);
    this.getFormSelectSubDivisionP = this.getFormSelectSubDivisionP.bind(this);
    this.getFormSelectFuncion = this.getFormSelectFuncion.bind(this);

    //Catalogos Informacion General
    this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
    this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
    this.getFormSelectEtnia = this.getFormSelectEtnia.bind(this);
    this.getFormSelectEstadoCivil = this.getFormSelectEstadoCivil.bind(this);
    this.getFormSelectTipoDiscapacidad = this.getFormSelectTipoDiscapacidad.bind(
      this
    );
    this.getFormSelectCodigoSiniestro = this.getFormSelectCodigoSiniestro.bind(
      this
    );
    this.getFormSelectFormacionEducativa = this.getFormSelectFormacionEducativa.bind(
      this
    );
    this.getFormSelectFormacion = this.getFormSelectFormacion.bind(this);
    this.getFormSelectTitulos = this.getFormSelectTitulos.bind(this);
    this.getFormSelectNacionalidades = this.getFormSelectNacionalidades.bind(
      this
    );
    this.getFormSelectCodigoIncapacidad = this.getFormSelectCodigoIncapacidad.bind(
      this
    );
    this.getFormSelectEncargadoPersonal = this.getFormSelectEncargadoPersonal.bind(
      this
    );
    this.getFormSelectDestinos = this.getFormSelectDestinos.bind(this);
    this.getFormSelectProyecto = this.getFormSelectProyecto.bind(this);
    this.getFormSelectParentesco = this.getFormSelectParentesco.bind(this);

    //Condiciones de Empleo
    this.getFormSelectGrupoPersonal = this.getFormSelectGrupoPersonal.bind(
      this
    );
    this.getFormSelectAreas = this.getFormSelectAreas.bind(this);
    this.getFormSelectVinculoLaboral = this.getFormSelectVinculoLaboral.bind(
      this
    );
    this.getFormSelectClase = this.getFormSelectClase.bind(this);
    this.getFormSelectEncuadre = this.getFormSelectEncuadre.bind(this);
    this.getFormSelectPlanBeneficios = this.getFormSelectPlanBeneficios.bind(
      this
    );
    this.getFormSelectPlanSalud = this.getFormSelectPlanSalud.bind(this);
    this.getFormSelectCoberturaDependiente = this.getFormSelectCoberturaDependiente.bind(
      this
    );
    this.getFormSelectPlanesBeneficios = this.getFormSelectPlanesBeneficios.bind(
      this
    );
    this.getFormSelectAsociaciones = this.getFormSelectAsociaciones.bind(this);
    this.getFormSelectAptoMedico = this.getFormSelectAptoMedico.bind(this);
    this.getFormSelectSitioTrabajo = this.getFormSelectSitioTrabajo.bind(this);
    this.getFormSelectSector = this.getFormSelectSector.bind(this);
  }

  componentDidMount() {
    console.log("SessionStorage", sessionStorage);

    this.ConsultaColaborador();
    this.GetCatalogos();
    this.GetNacionalidades();
    this.GetProvincias();
    this.GetCiudades();
    this.onLoad();
  }

  onLoad = () => {
    var esReingreso = sessionStorage.getItem("esReingreso");
    console.log("esReingreso");
    if (esReingreso) {
      this.setState({ bloquearTabs: true });
    }
  };
  cambiarIdColaboradorSession = (Id, IdAntiguo) => {
    console.log("SessionStorageAnterior", sessionStorage);
    console.log("NuevoIdReingreso", Id);
    sessionStorage.setItem("id_colaborador", Id);
    sessionStorage.setItem("id_colaboradorAntiguo", IdAntiguo);
    sessionStorage.setItem("esReingreso", false);
    console.log("SessionStorageNuevo", sessionStorage);
  };

  ConsultaColaborador() {
    axios
      .post("/RRHH/Colaboradores/GetColaboradorApi/", {
        id: sessionStorage.getItem("id_colaborador"),
      })
      .then((response) => {
        console.log("consulta col", response.data);
        //var data = JSON.parse(response.data);
        this.setState({
          colaborador: response.data,
          id_colaborador: sessionStorage.getItem("id_colaborador"),
          destino_estancia: response.data.catalogo_destino_estancia_id,
        });
        // console.log('de', this.state.colaborador);
        this.childInfo.current.getFotografia();
        this.state.colaborador.ContactoId > 0 ? this.ConsultaContacto() : "";
        this.ConsultaCargaSocial();
        // this.childServicios.current.ConsultaServicios();
        this.state.colaborador.catalogo_encargado_personal_id > 0
          ? this.CargaCategoriaEncargado(
              this.state.colaborador.catalogo_encargado_personal_id
            )
          : "";
        this.ConsultaContactosEmergencia();
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ConsultaContacto() {
    console.log("cOLABORADOR CONTACTO", this.state.colaborador);
    axios
      .post("/RRHH/Colaboradores/GetContactoApi/", {
        id: this.state.colaborador.ContactoId,
      })
      .then((response) => {
        //console.log('contacto',response.data)
        this.setState({
          contacto: response.data,
          key_form: Math.random(),
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ConsultaCargaSocial() {
    axios
      .post("/RRHH/Colaboradores/GetCargasApi/", {
        id: this.state.id_colaborador,
      })
      .then((response) => {
        //console.log(response.data)
        this.setState({
          cargas: response.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ConsultaContactosEmergencia() {
    axios
      .post("/RRHH/ContactoEmergencia/GetByColaboradorId/", {
        id: this.state.id_colaborador,
      })
      .then((response) => {
        console.log("contactos_emergencia", response.data.result);
        this.setState({
          contactos_emergencia: response.data.result,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  GetNacionalidades() {
    axios
      .post("/RRHH/Colaboradores/GetPaisesApi", {})
      .then((response) => {
        this.setState({ nacionalidades: response.data });
        this.getFormSelectNacionalidad();
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getFormSelectNacionalidad() {
    return this.state.nacionalidades.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  GetProvincias() {
    axios
      .get("/RRHH/Colaboradores/GetListaProvinciasApi")
      .then((response) => {
        this.setState({ provincias: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  GetCiudades() {
    axios
      .get("/RRHH/Colaboradores/GetListaCiudadesApi")
      .then((response) => {
        this.setState({ ciudades: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  GetParroquias() {
    axios
      .get("/RRHH/Colaboradores/GetListaParroquiasApi")
      .then((response) => {
        this.setState({ parroquias: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  activeTabs = (bool) => {
    this.setState({ bloquearTabs: bool });
    this.GetNacionalidades();
    this.GetProvincias();
    this.GetCiudades();
  };

  render() {
    return (
      <BlockUi tag="div" blocking={this.state.loading}>
        <Card title="REINGRESO COLABORADOR">
          <hr />
          <div>
            <Growl
              ref={(el) => {
                this.growl = el;
              }}
              position="bottomright"
              baseZIndex={1000}
            ></Growl>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="text">
                    <b>Tipo de Identificación:</b>{" "}
                    {this.state.colaborador.nombre_identificacion}{" "}
                  </label>
                </div>
                <div className="form-group">
                  <label htmlFor="text">
                    <b>Apellidos Nombres:</b>{" "}
                    {this.state.colaborador.nombres_apellidos}{" "}
                  </label>
                </div>
                <div className="form-group">
                  <label htmlFor="text">
                    <b>ID Legajo:</b>{" "}
                    {this.state.colaborador.numero_legajo_definitivo
                      ? this.state.colaborador.numero_legajo_definitivo
                      : this.state.colaborador.numero_legajo_temporal}{" "}
                  </label>
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="text">
                    <b>No. de Identificación:</b>{" "}
                    {this.state.colaborador.numero_identificacion}{" "}
                  </label>
                </div>
                <div className="form-group">
                  <label htmlFor="text">
                    <b>Estado:</b> {this.state.colaborador.estado}{" "}
                  </label>
                </div>
                <div className="form-group">
                  <label htmlFor="text">
                    <b>ID SAP:</b>{" "}
                    {this.state.colaborador.nro_sap
                      ? this.state.colaborador.nro_sap
                      : "No asignado"}{" "}
                  </label>
                </div>
              </div>
            </div>
            <hr />
            <ul className="nav nav-tabs" id="detalles_tabs" role="tablist">
              <li className="nav-item">
                <a
                  className="nav-link active"
                  id="info_general-tab"
                  data-toggle="tab"
                  href="#info_general"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Información General
                </a>
              </li>
              {!this.state.bloquearTabs && (
                <>
                  <li className="nav-item">
                    <a
                      className="nav-link"
                      id="contactos-tab"
                      data-toggle="tab"
                      href="#contactos"
                      role="tab"
                      aria-controls="home"
                      aria-expanded="true"
                    >
                      Direcciones
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className="nav-link"
                      id="condiciones_empleo-tab"
                      data-toggle="tab"
                      href="#condiciones_empleo"
                      role="tab"
                      aria-controls="home"
                      aria-expanded="true"
                    >
                      Condiciones de Empleo
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className="nav-link"
                      id="cargas_sociales-tab"
                      data-toggle="tab"
                      href="#cargas_sociales"
                      role="tab"
                      aria-controls="home"
                      aria-expanded="true"
                    >
                      Familiares
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className="nav-link"
                      id="nomina-tab"
                      data-toggle="tab"
                      href="#nomina"
                      role="tab"
                      aria-controls="home"
                      aria-expanded="true"
                    >
                      Nómina
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className="nav-link"
                      id="emergencia-tab"
                      data-toggle="tab"
                      href="#emergencia"
                      role="tab"
                      aria-controls="home"
                      aria-expanded="true"
                    >
                      Contactos de Emergencia
                    </a>
                  </li>
                </>
              )}
            </ul>

            <div className="tab-content" id="myTabContent">
              <div
                className="tab-pane fade show active"
                id="info_general"
                role="tabpanel"
                aria-labelledby="info_general-tab"
              >
                <EditarInformacionGeneral
                  ref={this.childInfo}
                  regresar={this.Regresar}
                  ChangeTab={this.ChangeTab}
                  ColaboradorId={this.ColaboradorId}
                  colaborador={this.state.colaborador}
                  validarCedula={this.validarCedula}
                  Siguiente={this.Siguiente}
                  getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                  CargaCategoriaEncargado={this.CargaCategoriaEncargado}
                  CargaNomina={this.CargaNomina}
                  disable_button={this.state.disable_button}
                  getFormSelectTipoIdent={this.getFormSelectTipoIdent}
                  getFormSelectGenero={this.getFormSelectGenero}
                  getFormSelectEtnia={this.getFormSelectEtnia}
                  getFormSelectEstadoCivil={this.getFormSelectEstadoCivil}
                  getFormSelectTipoDiscapacidad={
                    this.getFormSelectTipoDiscapacidad
                  }
                  getFormSelectCodigoSiniestro={
                    this.getFormSelectCodigoSiniestro
                  }
                  getFormSelectFormacionEducativa={
                    this.getFormSelectFormacionEducativa
                  }
                  getFormSelectFormacion={this.getFormSelectFormacion}
                  getFormSelectTitulos={this.getFormSelectTitulos}
                  getFormSelectNacionalidades={this.getFormSelectNacionalidades}
                  getFormSelectCodigoIncapacidad={
                    this.getFormSelectCodigoIncapacidad
                  }
                  getFormSelectEncargadoPersonal={
                    this.getFormSelectEncargadoPersonal
                  }
                  getFormSelectDestinos={this.getFormSelectDestinos}
                  getFormSelectProyecto={this.getFormSelectProyecto}
                  tiposIdentificacion={this.state.tiposIdentificacion}
                  estados={this.state.estados}
                  generos={this.state.generos}
                  tiposEncargadoPersonal={this.state.tiposEncargadoPersonal}
                  tiposNacionalidades={this.state.tiposNacionalidades}
                  //
                  NombreTipoIdentificacion={
                    this.state.colaborador != null
                      ? this.state.colaborador.nombre_identificacion
                      : ""
                  }
                  empresa_id={
                    this.state.colaborador != null
                      ? this.state.colaborador.empresa_id
                      : 0
                  }
                  empresas={this.state.empresas}
                  activeTabs={this.activeTabs}
                  cambiarIdColaboradorSession={this.cambiarIdColaboradorSession}
                  ConsultaColaborador={this.ConsultaColaborador}
                />
              </div>
              <div
                className="tab-pane fade show"
                id="contactos"
                role="tabpanel"
                aria-labelledby="contactos-tab"
                clas
              >
                <EditarContacto
                  ref={this.childContacto}
                  regresar={this.Regresar}
                  ChangeTab={this.ChangeTab}
                  colaborador={this.state.colaborador}
                  ContantoInfo={this.ContantoInfo}
                  contacto={this.state.contacto}
                  Siguiente={this.Siguiente}
                  getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                  provincias={this.state.provincias}
                  ciudades={this.state.ciudades}
                  parroquias={this.state.parroquias}
                  disable_button={this.state.disable_button}
                />
              </div>
              <div
                className="tab-pane fade show"
                id="condiciones_empleo"
                role="tabpanel"
                aria-labelledby="condiciones_empleo-tab"
              >
                <EditarCondicionesEmpleo
                  ref={this.childEmpleo}
                  regresar={this.Regresar}
                  successMessage={this.successMessage}
                  warnMessage={this.warnMessage}
                  ChangeTab={this.ChangeTab}
                  id_colaborador={this.state.id_colaborador}
                  colaborador={this.state.colaborador}
                  Siguiente={this.Siguiente}
                  disable_button={this.state.disable_button}
                  getFormSelectGrupoPersonal={this.getFormSelectGrupoPersonal}
                  getFormSelectAreas={this.getFormSelectAreas}
                  getFormSelectVinculoLaboral={this.getFormSelectVinculoLaboral}
                  getFormSelectClase={this.getFormSelectClase}
                  getFormSelectEncuadre={this.getFormSelectEncuadre}
                  getFormSelectPlanBeneficios={this.getFormSelectPlanBeneficios}
                  getFormSelectPlanSalud={this.getFormSelectPlanSalud}
                  getFormSelectCoberturaDependiente={
                    this.getFormSelectCoberturaDependiente
                  }
                  getFormSelectPlanesBeneficios={
                    this.getFormSelectPlanesBeneficios
                  }
                  getFormSelectAsociaciones={this.getFormSelectAsociaciones}
                  getFormSelectAptoMedico={this.getFormSelectAptoMedico}
                  getFormSelectSitioTrabajo={this.getFormSelectSitioTrabajo}
                  getFormSelectSector={this.getFormSelectSector}
                  getFormSelectEncargadoPersonal={
                    this.getFormSelectEncargadoPersonal
                  }
                  getFormSelectDestinos={this.getFormSelectDestinos}
                />
              </div>
              {/*<div className="tab-pane fade show" id="servicios" role="tabpanel" aria-labelledby="servicios-tab">
                            <EditarServicios ref={this.childServicios}
                                regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                Siguiente={this.Siguiente}
                                id_colaborador={this.state.id_colaborador}
                                destino_estancia={this.state.destino_estancia}
                                colaborador={this.state.colaborador}
                                disable_button={this.state.disable_button}
                            />
                                </div>*/}
              <div
                className="tab-pane fade show"
                id="cargas_sociales"
                role="tabpanel"
                aria-labelledby="cargas_sociales-tab"
              >
                <EditarCargaSocial
                  ref={this.childCargas}
                  regresar={this.Regresar}
                  successMessage={this.successMessage}
                  warnMessage={this.warnMessage}
                  ChangeTab={this.ChangeTab}
                  id_colaborador={this.state.id_colaborador}
                  cargas={this.state.cargas}
                  validarCedula={this.validarCedula}
                  Siguiente={this.Siguiente}
                  getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                  CargasInfo={this.CargasInfo}
                  getFormSelectTipoIdent={this.getFormSelectTipoIdent}
                  getFormSelectGenero={this.getFormSelectGenero}
                  getFormSelectEstadoCivil={this.getFormSelectEstadoCivil}
                  getFormSelectTipoDiscapacidad={
                    this.getFormSelectTipoDiscapacidad
                  }
                  getFormSelectNacionalidades={this.getFormSelectNacionalidades}
                  getFormSelectParentesco={this.getFormSelectParentesco}
                  tiposIdentificacion={this.state.tiposIdentificacion}
                  estados={this.state.estados}
                  generos={this.state.generos}
                  tiposParentesco={this.state.tiposParentesco}
                  colaborador={this.state.colaborador}
                  tiposNacionalidades={this.state.tiposNacionalidades}
                />
              </div>
              <div
                className="tab-pane fade show"
                id="nomina"
                role="tabpanel"
                aria-labelledby="nomina-tab"
              >
                <EditarNomina
                  ref={this.childNomina}
                  regresar={this.Regresar}
                  successMessage={this.successMessage}
                  warnMessage={this.warnMessage}
                  ChangeTab={this.ChangeTab}
                  id_colaborador={this.state.id_colaborador}
                  colaborador={this.state.colaborador}
                  getFormSelectGrupo={this.getFormSelectGrupo}
                  disable_button={this.state.disable_button}
                  tiposNomina={this.state.tiposNomina}
                  tiposPeriodoNomina={this.state.tiposPeriodoNomina}
                  tiposEncargadosPersonal={this.state.tiposEncargadoPersonal}
                  tiposContrato={this.state.tiposContrato}
                  getFormSelectDivisionPersonal={
                    this.getFormSelectDivisionPersonal
                  }
                  getFormSelectTipoContrato={this.getFormSelectTipoContrato}
                  getFormSelectClaseContrato={this.getFormSelectClaseContrato}
                  getFormSelectTipoNomina={this.getFormSelectTipoNomina}
                  getFormSelectPeriodoNomina={this.getFormSelectPeriodoNomina}
                  getFormSelectProyecto={this.getFormSelectProyecto}
                  getFormSelectFormaPago={this.getFormSelectFormaPago}
                  getFormSelectSubGrupo={this.getFormSelectSubGrupo}
                  getFormSelectBancos={this.getFormSelectBancos}
                  getFormSelectTipoCuenta={this.getFormSelectTipoCuenta}
                  getFormSelectViaPago={this.getFormSelectViaPago}
                  getFormSelectSubDivisionP={this.getFormSelectSubDivisionP}
                  getFormSelectFuncion={this.getFormSelectFuncion}
                  tiposViaPago={this.state.tiposViaPago}
                  tiposCategorias={this.state.tiposCategorias}
                />
              </div>
              <div
                className="tab-pane fade show"
                id="emergencia"
                role="tabpanel"
                aria-labelledby="emergencia-tab"
              >
                <EditarContactoEmergencia
                  regresar={this.Regresar}
                  ChangeTab={this.ChangeTab}
                  id_colaborador={this.state.id_colaborador}
                  colaborador={this.state.colaborador}
                  disable_button={this.state.disable_button}
                  contactos_emergencia={this.state.contactos_emergencia}
                  ContactosEmergenciaInfo={this.ContactosEmergenciaInfo}
                />
              </div>
            </div>
            <br />
            <div className="form-group">
              <div className="col">
                {/*<button
                  type="button"
                  onClick={this.validarEnvio}
                  className="btn btn-outline-primary"
                >
                  {" "}
                  Enviar
                </button>*/}
              </div>
            </div>
          </div>
        </Card>
      </BlockUi>
    );
  }

  CargaCategoriaEncargado(id) {
    axios
      .post("/RRHH/CategoriasEncargado/GetCategoriasPorEncargadoPersonalApi/", {
        id: id,
      })
      .then((response) => {
        if (response.data == []) {
          // this.warnMessage('No existe !!!');
        } else {
          this.setState({ tiposCategorias: response.data });
          this.getFormSelectGrupo();
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }

  CargaNomina(id) {
    this.childNomina.current.CargaDatosNomina(id);
  }

  getFormSelectGrupo() {
    return this.state.tiposCategorias.map((item) => {
      // console.log(item.Categoria.Id, item.Categoria.nombre)
      return (
        <option key={Math.random()} value={item.Categoria.Id}>
          {item.Categoria.nombre}
        </option>
      );
    });
  }

  validarEnvio() {
    console.log("envio");
    // axios.post("/RRHH/Colaboradores/GetExcelApi/")
    //     .then((response) => {
    //         console.log(response)
    //         if(response.data == "OK"){
    //             this.props.showSuccess("Excel generado");
    //         }else{
    //             this.props.showWarn("Algo salió mal");
    //         }
    //     })
    //     .catch((error) => {
    //         console.log(error);
    //     });
    var info = this.childInfo.current.handleValidation();
    var contacto = this.childContacto.current.handleValidation();
    var empleo = this.childEmpleo.current.handleValidation();
    var nomina = this.childNomina.current.handleValidation();
    console.log(info, contacto, empleo, nomina);
    if (info == false) {
      this.warnMessage("Complete Información General");
    } else if (contacto == false) {
      this.warnMessage("Complete Direcciones");
    } else if (empleo == false) {
      this.warnMessage("Complete Condiciones de Empleo");
    } else if (nomina == false) {
      this.warnMessage("Complete Nómina");
    } else if (
      info == true &&
      contacto == true &&
      empleo == true &&
      nomina == true
    ) {
      console.log(
        "colll",
        this.state.colaborador,
        this.state.colaborador.es_sustituto
      );
      var sustituto = this.ValidarExisteSustituto();
      var conyuge = this.ValidarEstadoCivil();

      if (sustituto == true && conyuge == true) {
        this.CambiaEstado();
      }

      // this.successMessage("Datos completos");
    } else {
      this.warnMessage("Algo salio mal.");
    }
  }

  ValidarEstadoCivil() {
    var estado = this.state.estados.filter(
      (c) =>
        c.Id == Number.parseInt(this.state.colaborador.catalogo_estado_civil_id)
    );
    if (estado.length > 0) {
      if (
        estado[0].codigo == ESTADO_CIVIL_CASADO ||
        estado[0].codigo == ESTADO_CIVIL_UNION
      ) {
        var conyuge = this.state.tiposParentesco.filter(
          (c) => c.codigo == PARENTESCO_CONYUGE
        );
        var conviviente = this.state.tiposParentesco.filter(
          (c) => c.codigo == PARENTESCO_CONVIVIENTE
        );
        this.ConsultaCargaSocial();
        if (this.state.cargas != "") {
          var existe = false;
          this.state.cargas.forEach((e) => {
            var parentesco = this.state.tiposParentesco.filter(
              (c) => c.Id == Number.parseInt(e.parentesco_id)
            );
            if (
              parentesco[0].codigo == conyuge[0].codigo ||
              parentesco[0].codigo == conviviente[0].codigo
            ) {
              existe = true;
            }
          });
          if (existe == true) {
            return true;
          } else {
            abp.notify.error(
              "Indique el CÓNYUGE O CONVIVIENTE/CONCUBINO en la pestaña de Familiares",
              "Error"
            );
            return false;
          }
        } else {
          abp.notify.error(
            "Indique el CÓNYUGE O CONVIVIENTE/CONCUBINO en la pestaña de Familiares",
            "Error"
          );
          return false;
        }
      } else {
        return true;
      }
    }
  }

  ValidarExisteSustituto() {
    if (this.state.colaborador.es_sustituto == true) {
      this.ConsultaCargaSocial();
      console.log("this.state.cargas", this.state.cargas);
      if (this.state.cargas != "") {
        var sustituto = false;
        this.state.cargas.forEach((e) => {
          if (e.por_sustitucion == true) {
            sustituto = true;
          }
        });
        if (sustituto == true) {
          return true;
        } else {
          abp.notify.error(
            "Indique el sustituto en la pestaña de Familiares",
            "Error"
          );
          return false;
        }
      } else {
        abp.notify.error(
          "Indique el sustituto en la pestaña de Familiares",
          "Error"
        );
        return false;
      }
    } else {
      return true;
    }
  }

  CambiaEstado() {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/EditEstadoColaboradorApi/", {
        id: this.state.id_colaborador,
        estado: "INFORMACION COMPLETA",
      })
      .then((response) => {
        console.log(response);
        abp.notify.success("Cambio estado a 'INFORMACION COMPLETA'", "Aviso");
        this.setState({ loading: false });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  Regresar() {
    return (window.location.href = "/RRHH/Colaboradores/Vista/");
  }

  Siguiente(id) {
    this.setState({ active: true });
    //console.log(id);
    switch (id) {
      case 1:
        this.setState({
          info_tab: false,
          contacto_tab: true,
          empleo_tab: false,
          servicio_tab: false,
          carga_tab: false,
          nomina_tab: false,
        });

        // window.scrollTo = "contactos";

        return;
      case 2:
        this.setState({
          info_tab: false,
          contacto_tab: false,
          empleo_tab: true,
          servicio_tab: false,
          carga_tab: false,
          nomina_tab: false,
        });
        // window.scrollTo = "condiciones_empleo";
        return;
      case 3:
        this.setState({
          info_tab: false,
          contacto_tab: false,
          empleo_tab: false,
          servicio_tab: true,
          carga_tab: false,
          nomina_tab: false,
        });
        // window.scrollTo = "servicios";
        return;
      case 4:
        this.setState({
          info_tab: false,
          contacto_tab: false,
          empleo_tab: false,
          servicio_tab: false,
          carga_tab: true,
          nomina_tab: false,
        });
        // window.scrollTo = "cargas_sociales";
        return;
      case 5:
        this.setState({
          info_tab: false,
          contacto_tab: false,
          empleo_tab: false,
          servicio_tab: false,
          carga_tab: false,
          nomina_tab: true,
        });
        // window.scrollTo = "nomina";
        return;
    }
  }

  ChangeTab() {
    //  this.childServicios.current.ConsultaServicios();
  }

  validarCedula(identificacion) {
    var estado = false;
    var valced = [];
    var provincia;
    // console.log(identificacion.length)
    if (identificacion == "2222222222") {
      estado = false;
    } else if (identificacion.length >= 10) {
      valced = identificacion.split("");
      // console.log('valced', valced)
      provincia = Number.parseInt(valced[0] + valced[1]);
      // console.log('provincia', provincia)
      if (provincia > 0 && provincia < 25) {
        if (Number.parseInt(valced[2]) < 6) {
          estado = this.VerificaCedula(valced);
          // console.log('VerificaCedula')
        }
        // else if (int.Parse(valced[2].ToString()) == 6) {
        //     estado = VerificaSectorPublico(valced);
        // }
        // else if (int.Parse(valced[2].ToString()) == 9) {

        //     estado = VerificaPersonaJuridica(valced);
        // }
      }
    }
    return estado;
  }

  VerificaCedula(validarCedula) {
    var aux = 0,
      par = 0,
      impar = 0,
      verifi;
    for (var i = 0; i < 9; i += 2) {
      aux = 2 * Number.parseInt(validarCedula[i]);
      if (aux > 9) aux -= 9;
      par += aux;
    }
    for (var i = 1; i < 9; i += 2) {
      impar += Number.parseInt(validarCedula[i]);
    }

    aux = par + impar;
    if (aux % 10 != 0) {
      verifi = 10 - (aux % 10);
    } else verifi = 0;
    if (verifi == Number.parseInt(validarCedula[9])) return true;
    else return false;
  }

  VerificaPersonaJuridica(validarCedula) {
    var aux = 0,
      prod,
      veri;
    veri =
      Number.parseInt(validarCedula[10]) +
      Number.parseInt(validarCedula[11]) +
      Number.parseInt(validarCedula[12]);
    if (veri > 0) {
      let coeficiente = [4, 3, 2, 7, 6, 5, 4, 3, 2];
      for (var i = 0; i < 9; i++) {
        prod = Number.parseInt(validarCedula[i]) * coeficiente[i];
        aux += prod;
      }
      if (aux % 11 == 0) {
        veri = 0;
      } else if (aux % 11 == 1) {
        return false;
      } else {
        aux = aux % 11;
        veri = 11 - aux;
      }

      if (veri == Number.parseInt(validarCedula[9])) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  VerificaSectorPublico(validarCedula) {
    var aux = 0,
      prod,
      veri;
    veri =
      Number.parseInt(validarCedula[9]) +
      Number.parseInt(validarCedula[10]) +
      Number.parseInt(validarCedula[11]) +
      Number.parseInt(validarCedula[12]);
    if (veri > 0) {
      let coeficiente = [3, 2, 7, 6, 5, 4, 3, 2];

      for (var i = 0; i < 8; i++) {
        prod = Number.parseInt(validarCedula[i]) * coeficiente[i];
        aux += prod;
      }

      if (aux % 11 == 0) {
        veri = 0;
      } else if (aux % 11 == 1) {
        return false;
      } else {
        aux = aux % 11;
        veri = 11 - aux;
      }

      if (veri == Number.parseInt(validarCedula[8])) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  ColaboradorId(data, id) {
    //console.log('entro a colaborador id');
    this.setState({ colaborador: data });
    this.setState({ id_colaborador: id });
  }

  ContantoInfo(data, id) {
    //console.log('entro a contacto');
    this.setState({ contacto: data });
    this.state.colaborador.ContactoId = id;

    var d = JSON.parse(data);
    console.log("entro a contacto");
    this.setState({ contacto: d });
    this.state.colaborador.ContactoId = id;
    console.log(this.state.colaborador.ContactoId);
  }

  CargasInfo(data) {
    // console.log('CargasInfo', data);
    this.setState({ cargas: data });
  }

  ContactosEmergenciaInfo(data) {
    // console.log('ContactosEmergenciaInfo', data);
    this.setState({ contactos_emergencia: data });
  }

  GetCatalogos() {
    let codigos = [];

    codigos = [
      //Informacion General
      "TIPOINDENTIFICACION",
      "GENERO",
      "ETNIA",
      "ESTADOCIVIL",
      "TIPODISCAPACIDAD",
      "CODIGOSINIESTRO",
      "FORMACIONEDUCATIVA",
      "FORMACION",
      "TITULO",
      "NACIONALIDADES",
      "CODIGOINCAPACIDAD",
      "ENCARGADO",
      "DESTINOS",
      "PROYECTO",
      //Condiciones de Empleo
      "GRUPOPERSONAL",
      "AREA",
      "VINCULOLABORAL",
      "CLASE",
      "ENCUADRE",
      "PLANDEBENEFICIOS",
      "OPCIONPLANSALUD",
      "COBERTURADEPENDIENTE",
      "PLANESDEBENEFICIOS",
      "ASOCIACION",
      "TIPOAPTOMEDICO",
      "SITIODETRABAJO",
      "SECTOR",
      "PARENTESCO",
      //Nomina
      "FUNCION",
      "TIPOCONTRATO",
      "CLASECONTRATO",
      "TIPONOMINA",
      "PERIODONOMINA",
      "FORMAPAGO",
      "SUBGRUPOCUARTIL",
      "BANCO",
      "TIPOCUENTA",
      "VIAPAGO",
      "DIVISIONPERSONAL",
      "SUBDIVISIONPERSONAL",
    ];

    axios
      .post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
      .then((response) => {
        //console.log(response.data);
        this.cargarCatalogos(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }

  cargarCatalogos(data) {
    data.forEach((e) => {
      var catalogo = JSON.parse(e);
      var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
      switch (codigoCatalogo) {
        //Informacion General
        case "TIPOINDENTIFICACION":
          this.setState({ tiposIdentificacion: catalogo });
          this.getFormSelectTipoIdent();
          return;
        case "NACIONALIDADES":
          this.setState({ tiposNacionalidades: catalogo });
          this.getFormSelectTipoIdent();
          return;
        case "GENERO":
          this.setState({ generos: catalogo });
          this.getFormSelectGenero();
          return;
        case "ETNIA":
          this.setState({ etnias: catalogo });
          this.getFormSelectEtnia();
          return;
        case "ESTADOCIVIL":
          this.setState({ estados: catalogo });
          this.getFormSelectEstadoCivil();
          return;
        case "TIPODISCAPACIDAD":
          this.setState({ tipoDiscapacidad: catalogo });
          this.getFormSelectTipoDiscapacidad();
          return;
        case "CODIGOSINIESTRO":
          this.setState({ codigoSiniestro: catalogo });
          this.getFormSelectCodigoSiniestro();
          return;
        case "FORMACIONEDUCATIVA":
          this.setState({ formacionAca: catalogo });
          this.getFormSelectFormacionEducativa();
          return;
        case "FORMACION":
          this.setState({ tiposFormaciones: catalogo });
          this.getFormSelectFormacion();
          return;
        case "TITULO":
          this.setState({ tiposTitulos: catalogo });
          this.getFormSelectTitulos();
          return;
        case "CODIGOINCAPACIDAD":
          this.setState({ tiposIncapacidades: catalogo });
          this.getFormSelectCodigoIncapacidad();
          return;
        case "ENCARGADO":
          this.setState({ tiposEncargadoPersonal: catalogo });
          this.getFormSelectEncargadoPersonal();
          return;
        case "DESTINOS":
          this.setState({ tiposDestino: catalogo });
          this.getFormSelectDestinos();
          return;
        case "PROYECTO":
          this.setState({ tiposProyectos: catalogo });
          this.getFormSelectProyecto();
          return;
        //Condiciones de empleo
        case "GRUPOPERSONAL":
          this.setState({ gruposPersonal: catalogo });
          this.getFormSelectGrupoPersonal();
          return;
        case "AREA":
          this.setState({ areas: catalogo });
          this.getFormSelectAreas();
          return;
        case "VINCULOLABORAL":
          this.setState({ vinculosLaborales: catalogo });
          this.getFormSelectVinculoLaboral();
          return;
        case "CLASE":
          this.setState({ clases: catalogo });
          this.getFormSelectClase();
          return;
        case "ENCUADRE":
          this.setState({ encuadres: catalogo });
          this.getFormSelectEncuadre();
          return;
        case "PLANDEBENEFICIOS":
          this.setState({ planBeneficios: catalogo });
          this.getFormSelectPlanBeneficios();
          return;
        case "OPCIONPLANSALUD":
          this.setState({ planSalud: catalogo });
          this.getFormSelectPlanSalud();
          return;
        case "COBERTURADEPENDIENTE":
          this.setState({ coberturaDependientes: catalogo });
          this.getFormSelectCoberturaDependiente();
          return;
        case "PLANESDEBENEFICIOS":
          this.setState({ planesBeneficios: catalogo });
          this.getFormSelectPlanesBeneficios();
          return;
        case "ASOCIACION":
          this.setState({ asociaciones: catalogo });
          this.getFormSelectAsociaciones();
          return;
        case "TIPOAPTOMEDICO":
          this.setState({ aptosMedico: catalogo });
          this.getFormSelectAptoMedico();
          return;
        case "SITIODETRABAJO":
          this.setState({ tiposSitios: catalogo });
          this.getFormSelectSitioTrabajo();
          return;
        case "SECTOR":
          this.setState({ tiposSectores: catalogo });
          this.getFormSelectSector();
          return;
        //Catalogos de Nomina
        case "FUNCION":
          this.setState({ tiposFuncion: catalogo });
          this.getFormSelectFuncion();
          return;
        case "TIPOCONTRATO":
          this.setState({ tiposContrato: catalogo });
          this.getFormSelectTipoContrato();
          return;
        case "CLASECONTRATO":
          this.setState({ tiposClaseContrato: catalogo });
          this.getFormSelectClaseContrato();
          return;
        case "TIPONOMINA":
          this.setState({ tiposNomina: catalogo });
          this.getFormSelectTipoNomina();
          return;
        case "PERIODONOMINA":
          this.setState({ tiposPeriodoNomina: catalogo });
          this.getFormSelectPeriodoNomina();
          return;
        case "FORMAPAGO":
          this.setState({ tiposFormaPago: catalogo });
          this.getFormSelectFormaPago();
          return;
        case "SUBGRUPOCUARTIL":
          this.setState({ tiposSubGrupo: catalogo });
          this.getFormSelectSubGrupo();
          return;
        case "BANCO":
          this.setState({ tiposBancos: catalogo });
          this.getFormSelectBancos();
          return;
        case "TIPOCUENTA":
          this.setState({ tiposCuentas: catalogo });
          this.getFormSelectTipoCuenta();
          return;
        case "VIAPAGO":
          this.setState({ tiposViaPago: catalogo });
          this.getFormSelectViaPago();
          return;
        case "DIVISIONPERSONAL":
          this.setState({ tiposDivisionPersonal: catalogo });
          this.getFormSelectDivisionPersonal();
          return;
        case "SUBDIVISIONPERSONAL":
          this.setState({ tiposSubDivisionP: catalogo });
          this.getFormSelectSubDivisionP();
          return;
        case "PARENTESCO":
          this.setState({ tiposParentesco: catalogo });
          this.getFormSelectParentesco();
          return;
        default:
          console.log(codigoCatalogo);
          return;
      }
    });
    this.setState({ loading: false });
  }

  getFormSelectParentesco() {
    return this.state.tiposParentesco.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  //Catalogos de Informacion General
  getFormSelectProyecto() {
    return this.state.tiposProyectos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEncargadoPersonal() {
    return this.state.tiposEncargadoPersonal.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectDestinos() {
    return this.state.tiposDestino.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectCodigoIncapacidad() {
    return this.state.tiposIncapacidades.map((item) => {
      if (item.codigo.replace(/ /g, "") != CODIGO_INCAPACIDAD_NOINCAPACITADO) {
        return (
          <option key={Math.random()} value={item.Id}>
            {item.nombre}
          </option>
        );
      }
    });
  }

  getFormSelectNacionalidades() {
    return this.state.tiposNacionalidades.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoIdent() {
    return this.state.tiposIdentificacion.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectGenero() {
    return this.state.generos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEtnia() {
    return this.state.etnias.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEstadoCivil() {
    return this.state.estados.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoDiscapacidad() {
    return this.state.tipoDiscapacidad.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectCodigoSiniestro() {
    return this.state.codigoSiniestro.map((item) => {
      if (item.codigo.replace(/ /g, "") != CODIGO_SINIESTRO_NOINCAPACITADO) {
        return (
          <option key={Math.random()} value={item.Id}>
            {item.nombre}
          </option>
        );
      }
    });
  }

  getFormSelectFormacionEducativa() {
    return this.state.formacionAca.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectFormacion() {
    return this.state.tiposFormaciones.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTitulos() {
    return this.state.tiposTitulos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  //Carga Catalogos de Nomina
  getFormSelectSubDivisionP() {
    return this.state.tiposSubDivisionP.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }
  getFormSelectFuncion() {
    return this.state.tiposFuncion.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectViaPago() {
    return this.state.tiposViaPago.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectDivisionPersonal() {
    return this.state.tiposDivisionPersonal.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoContrato() {
    return this.state.tiposContrato.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectClaseContrato() {
    return this.state.tiposClaseContrato.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoNomina() {
    return this.state.tiposNomina.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectPeriodoNomina() {
    return this.state.tiposPeriodoNomina.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectFormaPago() {
    return this.state.tiposFormaPago.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectSubGrupo() {
    return this.state.tiposSubGrupo.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectBancos() {
    return this.state.tiposBancos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoCuenta() {
    return this.state.tiposCuentas.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  //COndiciones de EMpleo
  getFormSelectSector() {
    return this.state.tiposSectores.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectSitioTrabajo() {
    return this.state.tiposSitios.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectGrupoPersonal() {
    return this.state.gruposPersonal.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectAreas() {
    return this.state.areas.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectVinculoLaboral() {
    return this.state.vinculosLaborales.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectClase() {
    return this.state.clases.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEncuadre() {
    return this.state.encuadres.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectPlanBeneficios() {
    return this.state.planBeneficios.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectPlanSalud() {
    return this.state.planSalud.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectCoberturaDependiente() {
    return this.state.coberturaDependientes.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectPlanesBeneficios() {
    return this.state.planesBeneficios.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectAsociaciones() {
    return this.state.asociaciones.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectAptoMedico() {
    return this.state.aptosMedico.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "Proceso exitoso!",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message,
    });
  }

  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }
}

ReactDOM.render(
  <ReingresoColaborador />,
  document.getElementById("content-editar-colaborador")
);
