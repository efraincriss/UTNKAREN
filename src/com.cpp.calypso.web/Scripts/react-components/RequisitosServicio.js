import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import BlockUi from 'react-block-ui';

export default class RequisitoServicio extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            requisitos: [],
            key_form: 23423,
            loading: true,
        }
        this.GetRequisitos = this.GetRequisitos.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadRequisito = this.LoadRequisito.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetRequisitos();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
            <div>
                <div className="row">
                    <div className="col">
                        <BootstrapTable
                            data={this.state.requisitos}
                            hover={true} 
                            pagination={true}
                            striped={false}
                            condensed={true}
                            options={options}
                        >
                            <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                            <TableHeaderColumn dataField='servicio' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Tipo Servicio</TableHeaderColumn>
                            <TableHeaderColumn dataField='requisito' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Requisito</TableHeaderColumn>
                            <TableHeaderColumn dataField='nombre_obligatorio' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Obligatorio</TableHeaderColumn>
                            <TableHeaderColumn width={'17%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>
            </div>
            </BlockUi>
        )
    }

    GetRequisitos() {
        axios.post("/RRHH/RequisitoServicio/GetRequisitosApi/", {})
            .then((response) => {
                this.setState({ requisitos: response.data })
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    generateButton(cell, row){
        return(
            <div>
                {/* <button onClick={() => this.Detalles(row.Id)} className="btn btn-outline-success btn-sm">Ver</button> */}
                <button onClick={() => this.LoadRequisito(row.Id)} className="btn btn-outline-primary btn-sm" style={{marginLeft: '0.2em'}}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id);}} className="btn btn-outline-danger btn-sm" style={{marginLeft: '0.2em'}}>Eliminar</button>
            </div>
        )
    }

    LoadRequisito(id){
        sessionStorage.setItem('id_requisito', id);
        return (
            window.location.href = "/RRHH/RequisitoServicio/Edit/"
        );
    }

    Delete(id){
        axios.post("/RRHH/RequisitoServicio/DeleteApiAsync/", {id: id})
        .then((response) => {
            abp.notify.success("Requisito eliminado!", "Aviso");
            this.GetRequisitos()
        })
        .catch((error) => {
            console.log(error);
        });
    }

}

ReactDOM.render(
    <RequisitoServicio />,
    document.getElementById('content-requisitos-servicio')
);