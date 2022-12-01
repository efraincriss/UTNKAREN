import React from 'react';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class CategoriasEncargadoTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Id: '',
            categoriaId: '',
            encargadoId: '',
            key_form: 23423,
        }
        this.generateButton = this.generateButton.bind(this);
        this.onHide = this.onHide.bind(this);
        this.showForm = this.showForm.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadCategoriasEncargado = this.LoadCategoriasEncargado.bind(this);
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
                    <TableHeaderColumn dataField="nombre_encargado" headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Encargado de Personal</TableHeaderColumn>
                    <TableHeaderColumn  dataField="nombre_categoria" isKey={true} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Categoría</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'20%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }

    generateButton(cell, row){
        return(
            <div style={{textAlign: 'center'}}>
                <button onClick={() => this.LoadCategoriasEncargado(row.Id)} className="btn btn-outline-primary btn-sm fa">Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id)}} className="btn btn-outline-danger btn-sm fa" style={{marginLeft: '0.2em'}}>Eliminar</button>
            </div>
        )
    }

    LoadCategoriasEncargado(id){
        sessionStorage.setItem('id_CategoriasEncargado', id);
        return (
            window.location.href = "/RRHH/CategoriasEncargado/Edit/"
        );
    }

    Delete(id){
        axios.post("/RRHH/CategoriasEncargado/DeleteApiAsync/"+ id, {})
        .then((response) => {
            if(response.data=="OK"){
               this.props.showSuccess();
               this.props.GetCategoriasEncargado();
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