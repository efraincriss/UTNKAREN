import React from "react";
import axios from "axios/index";

export default class RdoForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      fecha_rdo: "",
      block: false
    };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  render() {
    return (
      <div>
        <form onSubmit={this.handleSubmit}>
          <div className="form-group">
            <label>Fecha Rdo</label>
            <input
              type="date"
              id="no-filter"
              name="fecha_rdo"
              className="form-control"
              onChange={this.handleChange}
              value={this.state.fecha_rdo}
              required
            />
          </div>
          <button
            className="btn btn-outline-primary"
            type="submit"
            disabled={this.state.block}
          >
            Guardar
          </button>
        </form>
      </div>
    );
  }

  handleSubmit(event) {
    event.preventDefault();
    this.props.handleBlocking();
    axios
      .post("/Proyecto/RdoCabecera/CreateBaseRdo", {
        fecha_registro: this.state.fecha_rdo,
        RequerimientoId: document.getElementById("Id").className
      })
      .then(response => {
        if(response.data=="AVANCES"){

          abp.notify.info('La base Rdo Principal ya tiene avances registrados', 'Alerta');
          this.props.handleBlocking();
        }else{
          var ids = response.data.split(",");
          console.log(ids);
          this.props.handleBlocking();
          return window.location = `/Proyecto/RdoCabecera/Details/${ids[0]}`;
        }
        
      })
      .catch(error => {
        console.log(error);

        this.props.handleBlocking();
      });
  }

  handleChange(event) {
    event.stopPropagation();
    this.setState({ [event.target.name]: event.target.value });
  }
}
