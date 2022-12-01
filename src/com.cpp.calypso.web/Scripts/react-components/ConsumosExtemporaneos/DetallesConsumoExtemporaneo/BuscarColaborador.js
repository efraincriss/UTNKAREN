import React from "react";

import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import config from "../../Base/Config";

import { CONTROLLER_CONSUMO_EXTEMPORANEO, MODULO_PROVEEDOR, CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO } from "../../Base/Strings";
import ColaboradorConsumoTable from "./ColaboradorConsumoTable";
import { Button, FormTextarea } from 'shards-react'
import { BounceLoader } from 'react-spinners';


export default class BuscarColaboradorConsumo extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            identificacion: '',
            nombres: '',
            errors: {},
            observaciones: ''
        }
    }


    render() {
        const { observaciones } = this.state;
        const { blocking } = this.props;
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="identificacion"
                                        label="N. Identificación"
                                        type="text"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.identificacion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.identificacion}
                                    />
                                </div>
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="nombres"
                                        label="Apellidos Nombres"
                                        type="text"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.nombres}
                                        onChange={this.handleChange}
                                        error={this.state.errors.nombres}
                                    />
                                </div>

                                <div className="col" style={{ paddingTop: '35px' }}>
                                    {blocking
                                        ?
                                        <BounceLoader
                                            // css={override}
                                            size={35}
                                            sizeUnit={"px"}
                                            color={'#20a8d8'}
                                            loading={blocking}
                                        />
                                        :
                                        <Button type="submit" pill >Buscar</Button>
                                    }
                                    
                                    &nbsp;
                                </div>
                            </div>

                        </form>
                    </div>
                </div>

                <div className="row" style={{ marginBottom: '3em' }}>
                    <div className="col">
                        <hr />
                        <p className="mb-2">
                            {(observaciones && `🗣 ${observaciones}`) || "Registra una observación para el consumo..."}
                        </p>
                        <FormTextarea
                            onChange={this.handleChangeObservaciones}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col" style={{ maxHeight: '300px', overflowY: 'scroll' }}>
                        <ColaboradorConsumoTable
                            data={this.state.data}
                            onConfirm={this.onConfirm}
                        />
                    </div>
                </div>
            </div>
        )
    }


    handleSubmit = (event) => {
        event.preventDefault();
        this.props.blockScreen();

        var identificacion = this.state.identificacion;
        var nombres = this.state.nombres;

        let url = `/${MODULO_PROVEEDOR}/${CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO}/BuscarColaborador`;


        http.get(url, {
            params: {
                identificacion: identificacion,
                nombres: nombres,
            }
        })
            .then(response => {
                console.log(response);
                let data = response.data;
                if (data.success === true) {
                    this.setState({ data: data.result })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeObservaciones = (e) => {
        this.setState({ observaciones: e.target.value });
    }

    onConfirm = (colaborador) => {
        console.log(colaborador)
        this.props.crearDetalleConsumo(colaborador.Id, this.state.observaciones);
    }
}