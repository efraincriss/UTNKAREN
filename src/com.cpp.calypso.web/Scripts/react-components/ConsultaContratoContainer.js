import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import http from "./Base/HttpService"
import { CONTROLLER_CARPETA, MODULO_DOCUMENTOS, FRASE_CONTRATO_ELIMINADO, FRASE_ERROR_GLOBAL } from "./Base/Strings"

import CurrencyFormat from "react-currency-format";
import Field from './Base/Field-v3'
import wrapForm from "./Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { Card } from "primereact-v3.3/card";
import { ScrollPanel } from 'primereact-v3.3/scrollpanel';

import { InputText } from "primereact-v3.3/inputtext";
import { TabView, TabPanel } from "primereact-v3.3/tabview";
import { Jodit } from "jodit";
import Highlighter from "react-highlight-words";
class ConsultaContratoContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},

      carpetas: [],
      carpetaId: 0,

      catalogoEstadosContrato: [],
      catalogoEstadoContratoId: 0,

      tipoDocumentos: [],
      tipoDocumento: "",

      documentos: [],
      documentoId: 0,

      secciones: [],
      seccionId: [],
      palabra: "",

      soloTitulos: false,

      data: [],

      verContenido: false,
      contenido: '',


    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.loadData();
    this.loadCatalogos();
    this.jodit = new Jodit(this.editorRef, {
      height: "450", width: "1000",
      toolbar: false
    });
    this.jodit.value = "";
  }

  mostrarForm = (row) => {
    this.jodit.value = row.Contenido
    this.setState({ contenido: row.Contenido, verContenido: true });

  }
  ocultarForm = () => {
    this.setState({ verContenido: false });

  }
  loadData = () => {
    axios
      .get("/Documentos/Carpeta/ObtenerTodas")
      .then((response) => {
        console.log('Carpetas', response)
        var items = response.data.result.map(item => {
          return { label: item.NombreCorto, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ carpetas: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
      });
  }
  obtenerCarpetas = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/Carpeta/ObtenerTodas`
    return http.get(url)
  }
  obtenerEstadosContrato = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/Carpeta/ObtenerCatalogoEstados`
    return http.get(url)
  }

  loadCatalogos = () => {
    axios
      .post("/Documentos/Documento/GetByCodeApi/?code=TIPO_DOCUMENTO_CONTR", {})
      .then(response => {

        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.codigo };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ tipoDocumentos: items });

      })
      .catch(error => {
        console.log(error);
      });

  }
  loadDocumentos = (tipoDocumento) => {
    this.props.blockScreen();
    this.setState({ documentoId: 0, seccionId: 0, secciones: [], documentos: [] });
    axios
      .post("/Documentos/Documento/ObtenerDocumentosporTipo", {
        tipoDocumento: tipoDocumento,
        carpetaId: this.state.carpetaId
      })
      .then((response) => {
        console.log('documentosCarpeta', response)
        var items = response.data.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ documentos: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }

  loadDocumentosporCarpeta = (carpetaId) => {
    
    this.setState({ documentoId: 0, seccionId: 0, secciones: [], documentos: [] });
    axios
      .post("/Documentos/Documento/ObtenerDocporCarpeta", {
        carpetaId: carpetaId
      })
      .then((response) => {
        console.log('documentosCarpeta', response)
        var items = response.data.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ documentos: items });
        
      })
      .catch((error) => {
        console.log(error);
        
      });
  }

  loadSeccionesPorCarpeta = (carpetaId) => {
    this.props.blockScreen();
    this.setState({ seccionId: 0, secciones: [] });
    axios
      .post("/Documentos/Documento/ObtenerSeccionesporCarpeta", {
        carpetaId: carpetaId
      })
      .then((response) => {
        console.log('SeccionesporDocumento', response)
        var items = response.data.map(item => {
          return { label: item.NombreSeccion, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ secciones: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }
  
  loadSecciones = (documentoId) => {
    this.props.blockScreen();
    this.setState({ seccionId: 0, secciones: [] });
    axios
      .post("/Documentos/Documento/ObtenerSeccionesporDocumentoId", {
        documentoId: documentoId
      })
      .then((response) => {
        console.log('SeccionesporDocumento', response)
        var items = response.data.map(item => {
          return { label: item.NombreSeccion, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Todos", value: "0", dataKey: 0 });
        this.setState({ secciones: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }


  handlesubmit = (event) => {
    event.preventDefault();
    if (!this.isValid()) {
      return;
    } else {
      this.props.blockScreen();

      axios
        .post("/Documentos/Documento/ObtenerSeccionesporFiltros", {
          carpetaId: this.state.carpetaId,
          tipoDocumento: this.state.tipoDocumento,
          documentoId: this.state.documentoId,
          seccionId: this.state.seccionId,
          palabra: this.state.palabra,
          soloTitulos: this.state.soloTitulos
        })
        .then((response) => {
          console.log('SeccionesFiltros', response)

          this.setState({ data: response.data });
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  }

  isValid = () => {
    const errors = {};

    if (this.state.carpetaId == 0) {
      errors.carpetaId = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };
  onChangeValueCarpeta = (name, value) => {
    this.setState({
      [name]: value,
    });
    this.loadDocumentosporCarpeta(value);
    this.loadSeccionesPorCarpeta(value);
  };
  onChangeValueTipoDocumento = (name, value) => {
    this.setState({
      [name]: value,
    });
    this.loadDocumentos(value);
  };
  onChangeDocumentos = (name, value) => {
    this.setState({
      [name]: value,
    });
    this.loadSecciones(value);
  };
  handlechangecheck = (e) => {
    console.log('event', e);
    this.setState({ soloTitulos: e.checked })

  }

  contenidoFormat = (cell, row) => {
    return (
      <Highlighter
        highlightStyle={{ backgroundColor: 'yellow' }}
        searchWords={[this.state.palabra]}
        autoEscape={true}
        textToHighlight={cell}
      />);

  }


  generarBotones = (cell, row) => {
    return (
      <div>


        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver Contenido"
        >
          <i className="fa fa-search"></i>
        </button>

      </div>
    );
  };


  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "black" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    );
  }
  render() {
    const options = {
      sizePerPage: 10,
      expandBy: 'row',  // Currently, available value is row and column, default is row
      noDataText: "No existen datos registrados",
      sizePerPageList: [
        {
          text: "10",
          value: 10,
        },
        {
          text: "20",
          value: 20,
        },
      ],
      
      paginationShowsTotal: this.renderShowsTotal, // Accept bool or function
    };

    return (

      <>
        <Card>
          <form onSubmit={this.handlesubmit}>
            <div className="row">
              <div className="col">
                <Field
                  name="carpetaId"
                  required
                  value={this.state.carpetaId}
                  label="Contratos"
                  options={this.state.carpetas}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueCarpeta}
                  error={this.state.errors.carpetaId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
                <Field
                  name="tipoDocumento"

                  value={this.state.tipoDocumento}
                  label="Tipo Documento"
                  options={this.state.tipoDocumentos}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueTipoDocumento}
                  error={this.state.errors.tipoDocumento}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="documentoId"

                  value={this.state.documentoId}
                  label="Documento"
                  options={this.state.documentos}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeDocumentos}
                  error={this.state.errors.documentoId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
                <Field
                  name="seccionId"

                  value={this.state.seccionId}
                  label="Sección"
                  options={this.state.secciones}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.seccionId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>

            <div className="row">
              <div className="col">
                <Field
                  name="palabra"
                  label="Buscar por palabra"

                  type="text"
                  edit={true}
                  readOnly={false}
                  value={this.state.palabra}
                  onChange={this.handleChange}
                  error={this.state.errors.palabra}
                />
              </div>

              <div className="col">

                <Field
                  name="soloTitulos"
                  label="Solo Títulos"
                  labelOption=" (Si /No)"
                  type={"checkbox"}
                  readOnly={false}
                  edit={true}
                  value={this.state.soloTitulos}
                  onChange={this.handleChange}
                  error={this.state.errors.soloTitulos}
                />
              </div>


            </div>
            <div className="row">
              <div className="col-6">
                <button

                  type="submit"
                  className="btn btn-outline-primary"
                >
                  Consultar
                </button>
                {" "}
                <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={this.onHide}
                >
                  Limpiar
                </button>
              </div>
              <div className="col"></div>
            </div>
          </form>

          <hr />
          <TabView className="tabview-custom">
            <TabPanel header="Documentos">
              <BootstrapTable
                data={this.state.data}
                hover={true}
                pagination={true}
                options={options}
              >
                <TableHeaderColumn
                  dataField="Id"
                  hidden
                  isKey
                  width={"6%"}
                  tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                >
                  Id
                </TableHeaderColumn>
                <TableHeaderColumn
                  dataField="any"
                  dataFormat={this.Secuencial}
                  width={"6%"}
                  tdStyle={{
                    whiteSpace: "normal",
                    fontSize: "10px",
                  }}
                  thStyle={{
                    whiteSpace: "normal",
                    fontSize: "10px",
                  }}
                >
                  Nº
                </TableHeaderColumn>

                <TableHeaderColumn

                  width={"8%"}
                  dataField="nombreDocumento"
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  dataSort={true}
                >
                  Documento
                </TableHeaderColumn>
                <TableHeaderColumn
                  width={"10%"}
                  dataField="NombreSeccion"
                  dataFormat={this.contenidoFormat}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal",textAlign: "justify", fontSize: "10px" }}
                  thStyle={{ whiteSpace: "normal", textAlign: "justify",fontSize: "10px" }}
                  dataSort={true}
                >
                  Sección
                </TableHeaderColumn>
                <TableHeaderColumn
                  dataField="Contenido_Plano"
                  dataFormat={this.contenidoFormat}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal",textAlign: "justify", fontSize: "10px" }}
                  thStyle={{ whiteSpace: "normal",textAlign: "justify", fontSize: "10px" }}
                  dataSort={true}
                >
                  Contenido
                </TableHeaderColumn>

                <TableHeaderColumn
                  width={"6%"}
                  dataField="NumeroPagina"
                  tdStyle={{ whiteSpace: "normal" }}
                  thStyle={{ whiteSpace: "normal" }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  dataSort={true}
                >
                  Num página
                </TableHeaderColumn>
                <TableHeaderColumn

                  width={"6%"}
                  dataFormat={this.generarBotones}
                  thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                >
                  Opciones
                </TableHeaderColumn>
              </BootstrapTable>

            </TabPanel>

          </TabView>
        </Card>
        <div>
        </div>
        <Dialog
          header="Contenido"
          modal={true}

          visible={this.state.verContenido}
          style={{ width: "950px" }}
          onHide={this.ocultarForm}
        >
          <ScrollPanel style={{ width: '100%', height: '480px' }}>
            <textarea ref={ref => this.editorRef = ref}
              onChange={this.handleChangeContenido}
              value={this.state.contenido}

            />
          </ScrollPanel>
        </Dialog>
      </>
    );

  }
  handleChangeContenido = (event) => {
    console.log('event', event)
    //this.setState{this.state.data.Contenido:}
  }
  EnviarFormulario = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
      if (this.state.action == "create") {
        const formData = new FormData();
        formData.append("Id", 0);
        formData.append("ArchivoId", null);
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        );
        formData.append(
          "fecha_orden_servicio",
          this.state.fecha_orden_servicio
        );
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os);
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        );
        formData.append(
          "monto_aprobado_ingeniería",
          this.state.monto_aprobado_ingeniería
        );
        formData.append(
          "monto_aprobado_subcontrato",
          this.state.monto_aprobado_subcontrato
        );
        formData.append("EstadoId", this.state.EstadoId);
        formData.append("version_os", this.state.version_os);
        formData.append("comentarios", this.state.comentarios);
        formData.append("anio", this.state.anio);
        formData.append("referencias_po", this.state.referencias_po);
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        } else {
          formData.append("UploadedFile", null);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        };
        axios
          .post("/Proyecto/OrdenServicio/FGetCreate", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto");
              this.setState({ action: "list" });
              this.GetList();
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error");
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            );
          });
      } else {
        const formData = new FormData();
        formData.append("Id", this.state.Id);
        formData.append("ArchivoId", null);
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        );
        formData.append(
          "fecha_orden_servicio",
          this.state.fecha_orden_servicio
        );
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os);
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        );
        formData.append(
          "monto_aprobado_ingeniería",
          this.state.monto_aprobado_ingeniería
        );
        formData.append(
          "monto_aprobado_subcontrato",
          this.state.monto_aprobado_subcontrato
        );
        formData.append("EstadoId", this.state.EstadoId);
        formData.append("version_os", this.state.version_os);
        formData.append("comentarios", this.state.comentarios);
        formData.append("anio", this.state.anio);
        formData.append("referencias_po", this.state.referencias_po);
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        } else {
          formData.append("UploadedFile", null);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        };

        axios
          .post("/Proyecto/OrdenServicio/FGetEdit", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto");
              this.setState({ action: "list" });
              this.GetList();
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error");
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            );
          });
      }
    }
  };
  EnviarFormularioDetalle = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValidDetalle()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
      if (this.state.action_detalles == "create") {
        axios
          .post("/Proyecto/OrdenServicio/FDCreateDetalle", {
            Id: 0,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(error, "Error");
          });
      } else {
        axios
          .post("/Proyecto/OrdenServicio/FDEditDetalle", {
            Id: this.state.Idd,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
              "Error"
            );
          });
      }
    }
  };

  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };

  MostrarFormulario() {
    this.setState({ visible: true });
  }

  onHide = () => {
    this.setState({
      carpetaId: 0, tipoDocumento: "", documentoId: 0, seccionId: 0, palabra: "", verContenido: false, contenido: '', soloTitulos: false,
    });
  };

  handleChange(event) {

    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile,
        });
      }
    } else {
      console.log(event)
      const value = event.target.type === "checkbox" ? event.target.checked : event.target.value;
      this.setState({ [event.target.name]: value });
    }
  }

  OcultarFormulario = () => {
    this.setState({ visible: false });
  };
}
const Container = wrapForm(ConsultaContratoContainer);
ReactDOM.render(<Container />, document.getElementById("content"));
