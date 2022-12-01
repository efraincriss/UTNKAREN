import React from "react"
import {
  MODULO_DOCUMENTOS,
  CONTROLLER_CONSULTA_CONTRATO,
} from "../../Base/Strings"
import config from "../../Base/Config"
import { Button } from "primereact-v2/button"
import Field from "../../Base/Field-v2"

export class CabeceraConsulta extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
        data: {},
    errors: {}
    }
  }

  render() {
    return (
      <div className="row">
        <div style={{ width: "100%" }}>
          <div className="card">
            <div className="card-body">
              <div className="row" align="right">
                <div className="col">
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary"
                    onClick={() => this.redireccionar("REGRESAR", 0)}
                  >
                    Regresar
                  </button>
                </div>
              </div>
              <hr />

              <div className="row">
                <div className="col-xs-12 col-md-6">
                  <h6 className="text-gray-700">
                    <b>Código Contrato:</b>{" "}
                    {this.props.carpeta.Codigo ? this.props.carpeta.Codigo : ""}
                  </h6>
                </div>

                <div className="col-xs-12 col-md-6">
                  <h6 className="text-gray-700">
                    <b>Nombre Contrato:</b>{" "}
                    {this.props.carpeta.NombreCorto
                      ? this.props.carpeta.NombreCorto
                      : ""}
                  </h6>
                </div>
              </div>
              <hr />
              <div className="row">
                <div className="col">
                  <Field
                    name="Codigo"
                    value={this.state.data.Codigo}
                    label="* Código"
                    edit={true}
                    readOnly={this.state.data.Id > 0}
                    type="text"
                    onChange={this.handleChange}
                    error={this.state.errors.Codigo}
                  />
                </div>

                <div className="col">
                  <Field
                    name="Nombre"
                    value={this.state.data.Nombre}
                    label="* Nombre"
                    type="text"
                    edit={true}
                    readOnly={false}
                    onChange={this.handleChange}
                    error={this.state.errors.Nombre}
                  />
                </div>
              </div>

              <div className="row">
                <div className="col">
                  <Field
                    name="TipoDocumentoId"
                    label="* Tipo Documento"
                    type="select"
                    options={this.props.catalogoTipoDocumento}
                    edit={true}
                    readOnly={false}
                    value={this.state.data.TipoDocumentoId}
                    onChange={this.onChangeDropdown}
                    error={this.state.errors.TipoDocumentoId}
                    placeholder="Seleccionar..."
                  />
                </div>
                <div className="col">
                  <Field
                    name="CantidadPaginas"
                    value={this.state.data.CantidadPaginas}
                    label="* Número Páginas (Doc. Físico)"
                    type="NUMBER"
                    edit={true}
                    readOnly={false}
                    onChange={this.handleChange}
                    error={this.state.errors.CantidadPaginas}
                  />
                </div>
              </div>

              <div className="row">
                <div className="col">
                  <Field
                    name="DocumentoPadreId"
                    label="Pertenece a"
                    type="select"
                    options={this.props.documentosAnexos}
                    edit={true}
                    readOnly={false}
                    value={this.state.data.DocumentoPadreId}
                    onChange={this.onChangeDropdown}
                    error={this.state.errors.DocumentoPadreId}
                    placeholder="Seleccionar..."
                  />
                </div>
              </div>

              <div className="row align-items-center">
                <div className="col">
                  <Fragment>
                    <Checkbox
                      inputId="cb1"
                      checked={this.state.data.EsImagen}
                      onChange={(e) => this.handleChange(e)}
                      name="EsImagen"
                    />
                    <label htmlFor="cb1" className="p-checkbox-label">
                      Es imagen?
                    </label>
                  </Fragment>
                </div>

                <div className="col"></div>
              </div>

              <hr />
              <div
                className="row"
                style={{ marginTop: "0.4em", marginLeft: "0.1em" }}
              >
                <Button
                  label="Buscar"
                  className="p-button-outlined"
                  onClick={() => this.handleSubmit()}
                  icon="pi pi-save"
                />
                <Button
                  style={{ marginLeft: "0.4em" }}
                  label="Cancelar"
                  className="p-button-outlined"
                  onClick={() => this.props.onHideFormulario()}
                  icon="pi pi-ban"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  redireccionar = (accion, id) => {
    if (accion === "REGRESAR") {
      window.location.href = `${config.appUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_CONSULTA_CONTRATO}`
    }
  }
}
