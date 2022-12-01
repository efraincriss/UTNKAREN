import React from 'react';
import ReactDOM from 'react-dom';
import { Growl } from 'primereact/components/growl/Growl';

//import CumplimientoRequisitosTable from './CumplimientoRequisitos/CumplimientoRequisitosTable';
import CrearCumplimientoRequisitos from './CumplimientoRequisitos/CrearCumplimientoRequisitos';
//import EditarCumplimientoRequisitos from './CumplimientoRequisitos/EditarCumplimientoRequisitos';

export default class CumplimientoRequisitos extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            table: false,
            crear: true,
            editar: false,
            key_form: 23423,
        }

        this.childEditar = React.createRef();

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);

        this.Siguiente = this.Siguiente.bind(this);
    }

    componentDidMount() {
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="tab-content" id="myTabContent" style={{ border: 'none' }}>
                    <div className={this.state.table == true ? "tab-pane fade show active" : "tab-pane fade show"} id="table" role="tabpanel">
                        {/* <CumplimientoRequisitosTable
                            Regresar={this.Regresar}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}
                            Siguiente={this.Siguiente}
                        /> */}
                    </div>
                    <div className={this.state.crear == true ? "tab-pane fade show active" : "tab-pane fade show"} id="crear" role="tabpanel">
                        <CrearCumplimientoRequisitos
                            Regresar={this.Regresar}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}
                            Siguiente={this.Siguiente}
                        />
                    </div>
                    <div className={this.state.editar == true ? "tab-pane fade show active" : "tab-pane fade show"} id="editar" role="tabpanel">
                        {/* <EditarCumplimientoRequisitos ref={this.childEditar}
                            Regresar={this.Regresar}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}
                            Siguiente={this.Siguiente}
                        /> */}
                    </div>
                </div>
            </div >
        )
    }

    Siguiente(id) {
        console.log(id);
        switch (id) {
            case 1:
                this.setState({
                    table: false,
                    crear: true,
                    editar: false
                });

                window.scrollTo = "crear";
                return;
            case 2:
            this.setState({
                table: false,
                crear: false,
                editar: true
            });
                window.scrollTo = "editar";
                this.childEditar.current.ConsultaServicio();
                return;

        }
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/ColaboradorRequisito/Index/"
        );
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
    <CumplimientoRequisitos />,
    document.getElementById('content-cumple-requisitos')
);