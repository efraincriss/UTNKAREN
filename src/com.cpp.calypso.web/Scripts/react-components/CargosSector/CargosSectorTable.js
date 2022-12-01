import React from 'react';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class CargosSectorTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Id: '',
            cargoId: '',
            sectorId: '',
            key_form: 23423,
        }
        this.generateButton = this.generateButton.bind(this);
        this.onHide = this.onHide.bind(this);
        this.showForm = this.showForm.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadCargosSector = this.LoadCargosSector.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={true} options={options}>
                    <TableHeaderColumn width={'6%'} dataField="nro" headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No.</TableHeaderColumn>
                    <TableHeaderColumn dataField="nombre_sector" headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Sector</TableHeaderColumn>
                    <TableHeaderColumn dataField="nombre_cargo" isKey={true} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cargo</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'20%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }

    generateButton(cell, row){
        return(
            <div style={{textAlign: 'center'}}>
                <button onClick={() => this.LoadCargosSector(row.Id)} className="btn btn-outline-primary btn-sm fa ">Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id)}} className="btn btn-outline-danger btn-sm fa" style={{marginLeft: '0.2em'}}>Eliminar</button>
            </div>
        )
    }

    LoadCargosSector(id){
        sessionStorage.setItem('id_CargosSector', id);
        return (
            window.location.href = "/RRHH/CargosSector/Edit/"
        );
    }

    Delete(id){
        axios.post("/RRHH/CargosSector/DeleteApiAsync/"+ id, {})
        .then((response) => {
            if(response.data=="OK"){
               this.props.showSuccess();
               this.props.getCargosSector();
            }else {
                this.props.showWarn();
            }
        })
        .catch((error) => {
            console.log(error);
        });
    }

    onHide() {
        this.setState({ visible: false })
    }

    showForm(){
        this.setState({visible: true})
    }

}