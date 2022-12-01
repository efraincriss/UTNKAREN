import React, { Component } from "react"
import { OrderList } from "primereact-v3.3/orderlist"
import { Button } from "primereact-v3.3/button"
import axios from "axios"

export class CorreosListaOrden extends Component {
  constructor(props) {
    super(props)
    this.state = {
      correos: props.correos,
    }

    this.listTemplate = this.listTemplate.bind(this)
  }

  componentWillReceiveProps(prevProps) {
    this.setState({
      correos: prevProps.correos,
    })
  }

  listTemplate(list) {
    return (
      <div className="p-clearfix">
        <div style={{ fontSize: "14px", margin: "15px 5px 0 0" }}>
          {list.nombres} - {list.correo}
        </div>
      </div>
    )
  }

  render() {
    return (
      <div>
        <div className="p-grid">
          <div className="p-col-12 p-md-6">
            <OrderList
              value={this.state.correos}
              dragdrop={true}
              itemTemplate={this.listTemplate}
              responsive={true}
              header="Lista de Distribución"
              listStyle={{ height: "20em" }}
              onChange={(e) => this.setState({ correos: e.value })}
            />
          </div>
        </div>
        <hr />
        <div className="row">
          <div className="col">
            <Button
              label="Guardar"
              className="p-button-outlined"
              onClick={() => this.guardarOrden(this.state.correos)}
              icon="pi pi-save"
            />
          </div>
        </div>
      </div>
    )
  }

  guardarOrden = (correos) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/ListaDistribucion/OrdenarCorroes", correos)
      .then((response) => {
        console.log(response.data)
        this.props.toggleOrdenarLista();
        this.props.getData();
      })
      .catch((error) => {
        console.log(error)
        this.props.unlockScreen();
      })
  }
}
