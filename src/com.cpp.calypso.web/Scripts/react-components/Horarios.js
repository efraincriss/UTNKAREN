import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {Growl} from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class Horarios extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            horarios: [],
            key_form: 23423,
        }
        this.GetHorarios= this.GetHorarios.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadHorario = this.LoadHorario.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetHorarios();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <div>
                  <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row">
                    <div className="col">
                        <BootstrapTable
                            data={this.state.horarios}
                            hover={true} 
                            pagination={true}
                            striped={false}
                            condensed={true}
                            options={options}
                        >
                            <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                            <TableHeaderColumn dataField="codigo" filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                            <TableHeaderColumn width={'20%'} dataField='nombre' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre</TableHeaderColumn>
                            <TableHeaderColumn dataField='h_inicio' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Hora Inicio</TableHeaderColumn>
                            <TableHeaderColumn dataField='h_fin' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Hora Fin</TableHeaderColumn>
                            <TableHeaderColumn dataField='nombre_estado' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Estado</TableHeaderColumn>
                            <TableHeaderColumn width={'17%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>
            </div>
        )
    }

    GetHorarios() {
        axios.post("/RRHH/Horario/GetHorariosApi/", {})
            .then((response) => {
                this.setState({ horarios: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    generateButton(cell, row){
        return(
            <div>
                {/* <button onClick={() => this.Detalles(row.Id)} className="btn btn-outline-success btn-sm">Ver</button> */}
                <button onClick={() => this.LoadHorario(row.Id)} className="btn btn-outline-primary btn-sm" style={{marginLeft: '0.2em'}}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id)}} className="btn btn-outline-danger btn-sm" style={{marginLeft: '0.2em'}}>Eliminar</button>
            </div>
        )
    }

    LoadHorario(id){
        sessionStorage.setItem('id_horario', id);
        return (
            window.location.href = "/RRHH/Horario/Edit/"
        );
    }

    Delete(id){
      

        axios.post("/RRHH/Horario/DeleteApiAsync/"+ id, {})
        .then((response) => {
            if(response.data=="OK"){

               this.showSuccess();
              this.GetHorarios();
            }else {

                this.showWarn();
            }
        })
        .catch((error) => {
            console.log(error);
        });
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

}

ReactDOM.render(
    <Horarios />,
    document.getElementById('content-horarios')
);