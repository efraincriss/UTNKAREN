import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "./Base/Field-v2";
import wrapForm from "./Base/BaseWrapper";
import { Growl } from "primereact/components/growl/Growl";

class SelectOferta extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      proyectos: [],
      requerimientos: [],
      ofertas: [],
      contratos: [],
      ContratoId: 0,
      ProyectoId: 0,
      RequerimientoId: 0,
      OfertaId: 0,
      message: "",
      blockSubmit: false,

      //Nuevos
      errors: {},
    };

    this.getProyectos = this.getProyectos.bind(this);
    this.getRequerimientos = this.getRequerimientos.bind(this);
    this.getOfertas = this.getOfertas.bind(this);
    this.getContratos = this.getContratos.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChangeProyecto = this.handleChangeProyecto.bind(this);
    this.handleChangeRequerimientos = this.handleChangeRequerimientos.bind(
      this
    );
    this.handleChangeOfertas = this.handleChangeOfertas.bind(this);
    this.handleChangeContrato = this.handleChangeContrato.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
  }

  componentDidMount() {
    this.getContratos();
  }

  render() {
    return (
      <div>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>

        <BlockUi tag="div" blocking={this.state.blocking}>
          <form onSubmit={this.handleSubmit}>
            <div className="form-group">
              <Field
                name="ContratoId"
                required
                value={this.state.ContratoId}
                label="Contratos"
                options={this.state.contratos}
                type={"select"}
                filter={true}
                onChange={this.handleChangeContrato}
                error={this.state.errors.ContratoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>

            <div className="form-group">
              <Field
                name="ProyectoId"
                required
                value={this.state.ProyectoId}
                label="Proyectos"
                options={this.state.proyectos}
                type={"select"}
                filter={true}
                onChange={this.handleChangeProyecto}
                error={this.state.errors.ProyectoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>

            <div className="form-group">
              <Field
                name="RequerimientoId"
                required
                value={this.state.RequerimientoId}
                label="SR (Service Request)"
                options={this.state.requerimientos}
                type={"select"}
                filter={true}
                onChange={this.handleChangeRequerimientos}
                error={this.state.errors.RequerimientoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>

            <div className="form-group">
              <Field
                name="OfertaId"
                required
                value={this.state.OfertaId}
                label="Base RDO"
                options={this.state.ofertas}
                type={"select"}
                filter={true}
                onChange={this.handleChangeOfertas}
                error={this.state.errors.OfertaId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>

            <button
              type="submit"
              className="btn btn-outline-primary"
              disabled={this.state.blockSubmit}
            >
              Siguiente
            </button>
          </form>
        </BlockUi>
      </div>
    );
  }

  handleSubmit(event) {
    event.preventDefault();
    if (this.state.ContratoId == 0) {
      this.props.showWarn("Selecciona un contrato");
    } else if (this.state.ProyectoId == 0) {
      this.props.showWarn("Selecciona un proyecto");
    } else if (this.state.RequerimientoId == 0) {
      this.props.showWarn("Selecciona un trabajo");
    } else if (this.state.OfertaId == 0) {
      this.props.showWarn("Selecciona un base rdo");
    } else {
      if (document.getElementById("content").className == "WBS") {
        window.location.href = "/Proyecto/Wbs/IndexWbs/" + this.state.OfertaId;
      } else if (document.getElementById("content").className == "AvanceObra") {
        window.location.href =
          "/Proyecto/AvanceObra/Index/" + this.state.OfertaId;
      } else if (
        document.getElementById("content").className == "OrdenServicio"
      ) {
        window.location.href =
          "/Proyecto/OrdenServicio/Index/" + this.state.OfertaId;
      } else if (
        document.getElementById("content").className == "AvanceIngenieria"
      ) {
        console.log("entroavanceingenieria");
        window.location.href =
          "/Proyecto/AvanceIngenieria/Index/" + this.state.OfertaId;
      } else if (
        document.getElementById("content").className == "OrdenCompra"
      ) {
        window.location.href =
          "/Proyecto/OrdenCompra/Index/" + this.state.OfertaId;
      } else if (
        document.getElementById("content").className == "AvanceProcura"
      ) {
        window.location.href =
          "/Proyecto/AvanceProcura/Index/" + this.state.OfertaId;
      }
    }
  }

  handleChangeContrato = (name, value) => {
    this.setState(
      {
        ContratoId: value,
        ProyectoId: 0,
        proyectos: [],
        RequerimientoId: 0,
        requerimientos: [],
        ofertas: [],
        OfertaId: 0,
        blocking: true,
      },
      this.getProyectos
    );
  };

  handleChangeProyecto = (name, value) => {
    this.setState(
      {
        ProyectoId: value,
        RequerimientoId: 0,
        requerimientos: [],
        ofertas: [],
        OfertaId: 0,
        blocking: true,
      },
      this.getRequerimientos
    );
  };

  handleChangeRequerimientos = (name, value) => {
    this.setState(
      { RequerimientoId: value, OfertaId: 0, ofertas: [], blocking: true },
      this.getOfertas
    );
  };

  handleChangeOfertas = (name, value) => {
    this.setState({ OfertaId: value });
  };

  getContratos() {
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return {
            label: item.Codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ contratos: items, blocking: false });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getProyectos() {
    axios
      .post("/Proyecto/Proyecto/GetProyectosApi/" + this.state.ContratoId, {})
      .then((response) => {
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ proyectos: items, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getRequerimientos() {
    axios
      .post(
        "/Proyecto/Oferta/GetRequerimientosProyectoApi/" +
          this.state.ProyectoId,
        {}
      )
      .then((response) => {
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ requerimientos: items, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getOfertas() {
    axios
      .post("/Proyecto/Oferta/GetOfertasApi/", {
        id: this.state.RequerimientoId,
        tipo: document.getElementById("content").className,
      })
      .then((response) => {
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.version,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ ofertas: items, blocking: false });
      })
      .catch((error) => {
        console.log(error);
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
}
const Container = wrapForm(SelectOferta);
ReactDOM.render(<Container />, document.getElementById("content"));
