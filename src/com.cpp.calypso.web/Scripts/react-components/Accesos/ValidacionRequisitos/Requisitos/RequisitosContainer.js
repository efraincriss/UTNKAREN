import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../../Base/BaseWrapper";
import http from "../../../Base/HttpService";
import config from "../../../Base/Config";
import CabeceraRequisitos from "./CabeceraRequisitos";
import { Dialog } from 'primereact-v2/dialog';
import {
    MODULO_PROYECTO,
    MODULO_RECURSOS_HUMANOS,
    MODULO_ACCESO,
    CONTROLLER_VALIDACION_REQUISITO,
    CONTROLLER_COLABORADORES,
    FRASE_ERROR_SELECCIONE_ACCION_COLABORADOR,
    FRASE_ERROR_SELECCIONE_MOTIVO_BAJA
} from "../../../Base/Strings";
import {
    ACCION_COLABORADOR,
    CODIGO_BAJA,
    MOTIVO_BAJA,
} from "../../../Base/Constantes";
import RequisitosBusqueda from "./RequisitosBusqueda";
import RequisitosTable from "./RequisitosTable";
import RequisitosForm from "./RequisitosForm";

export default class RequisitosContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            colaborador: {},
            catalogo_acciones: [],
            catalogo_bajas: [],
            requisitos: [],
            colaboradorId: 0,
            bajaId: 0,
            requisito: {},
            ultimaBusqueda: {
                accion: 0,
                baja: 0
            },
            keyForm: 45,
        }
    }

    componentWillMount() {
        this.setState({
            colaboradorId: this.props.getParameterByName('colaboradorId')
        }, () => console.log(this.state.colaboradorId))
        
    }

    componentDidMount() {
        this.consultarDatos();
    }


    render() {
        return (
            <div>
                <CabeceraRequisitos
                    colaborador={this.state.colaborador}
                />

                <div className="row">
                    <div style={{ width: '100%' }}>
                        <div className="card">
                            <div className="card-body">

                                <div className="row">
                                    <div className="col">
                                        <RequisitosBusqueda
                                            catalogo_acciones={this.state.catalogo_acciones}
                                            catalogo_bajas={this.state.catalogo_bajas}
                                            bajaId={this.state.bajaId}
                                            submitBusqueda={this.submitBusqueda}
                                        />
                                    </div>
                                </div>

                                <hr />

                                <RequisitosTable
                                    requisitos={this.state.requisitos}
                                    mostrarUploadFile={this.mostrarUploadFile}
                                />

                                <Dialog header="Subir Archivo" visible={this.state.showUploadFile} width="500px" modal={true} onHide={this.ocultarUploadFile}>
                                    <RequisitosForm
                                        requisito={this.state.requisito}
                                        ColaboradorId={this.state.colaboradorId}
                                        showSuccess={this.props.showSuccess}
                                        showWarn={this.props.showWarn}
                                        blockScreen={this.props.blockScreen}
                                        unlockScreen={this.props.unlockScreen}
                                        ocultarUploadFile={this.ocultarUploadFile}
                                        submitBusqueda={this.submitBusqueda}
                                        setRequisito={this.setRequisito}
                                        showValidation={this.props.showValidation}
                                        key = {this.state.keyForm}
                                    />
                                </Dialog>


                            </div>
                        </div>
                    </div>


                </div>
            </div>
        )
    }

    submitBusqueda = (accion = this.state.ultimaBusqueda.accion, baja = this.state.ultimaBusqueda.baja) => {

        if (accion === 0) {
            this.props.showWarn(FRASE_ERROR_SELECCIONE_ACCION_COLABORADOR)
        } else if (accion === this.state.bajaId && baja === 0) {
            this.props.showWarn(FRASE_ERROR_SELECCIONE_MOTIVO_BAJA)
        } else {
            this.props.blockScreen();
            var updatedUltimaBusqueda = {
                accion,
                baja
            };
            this.setState({ultimaBusqueda: updatedUltimaBusqueda})
            var ColaboradorId = this.state.colaboradorId;
            var GrupoPersonalId = this.state.colaborador.GrupoPersonalId;
            var entity = {
                ColaboradorId: ColaboradorId,
                GrupoPersonalId: GrupoPersonalId,
                AccionId: accion,
                TipoBajaId: baja === 0 ? null : baja
            }
            let url = '';
            url = `/Accesos/ValidacionRequisito/ObtenerRequisitos`
            http.post(url, entity)
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({ requisitos: data.result })
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
    }

    setRequisito = requisito => {
        this.setState({requisito})
    }

    obtenerColaborador = () => {
        let url = '';
        url = `/Accesos/ValidacionRequisito/DetallesApi/${this.state.colaboradorId}`;
        return http.get(url);
    }


    catalogoMotivoBaja = () => {
        let url = '';
        url = `/Accesos/ValidacionRequisito/FilterCatalogo/?code=${MOTIVO_BAJA}`;
        return http.get(url);
    }


    catalogoAccionColaboradores = () => {
        let url = '';
        url = `/Accesos/ValidacionRequisito/FilterCatalogo/?code=${ACCION_COLABORADOR}`;
        return http.get(url);
    }

    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoAccionColaboradores(), this.catalogoMotivoBaja(), this.obtenerColaborador()])
            .then(function ([acciones, bajas, colaborador]) {
                self.setState({
                    catalogo_acciones: self.customBuildDropdown(acciones.data, 'nombre', 'Id'),
                    catalogo_bajas: self.props.buildDropdown(bajas.data, 'nombre', 'Id'),
                    colaborador: colaborador.data.result
                }, self.props.unlockScreen)
            })
            .catch((error) => {

                self.props.unlockScreen();
                console.log(error);
            });
    }

    customBuildDropdown = (data, nameField = 'name', valueField = 'Id') => {
        if (data.success === true) {
            return data.result.map(i => {
                if (i["codigo"] === CODIGO_BAJA) {
                    this.setState({ bajaId: i[valueField] })
                }
                return { label: i[nameField], value: i[valueField] }
            });
        } else if (data !== undefined) {

            return data.map(i => {
                return { label: i[nameField], value: i[valueField] }
            });
        }

        return {};
    }

    ocultarUploadFile = () => {
        this.setState({ showUploadFile: false, requisito: {}, keyForm: Math.random() })
    }

    mostrarUploadFile = requisito => {
        this.setState({ showUploadFile: true, requisito })
    }
}


const Container = Wrapper(RequisitosContainer);
ReactDOM.render(
    <Container />,
    document.getElementById('gestion_requisitos_container')
);