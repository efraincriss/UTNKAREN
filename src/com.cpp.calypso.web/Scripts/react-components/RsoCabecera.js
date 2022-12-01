import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';


import { Growl } from 'primereact/components/growl/Growl';
import { Dialog } from 'primereact/components/dialog/Dialog';


import RdoCabeceraTable from './rso/rdo_cabecera_table';
import RdoForm from './rso/RdoForm';

class RdoCabecera extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            rdoCabeceras: [],
            rdoCabecerasDefinitivas: [],
            mensaje: '',
            blocking: true
        }

        this.getData = this.getData.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.showForm = this.showForm.bind(this);
        this.onHide = this.onHide.bind(this);
        this.handleBlocking = this.handleBlocking.bind(this);
    }

    componentWillMount() {
        this.getData();
    }


    render() {
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div>
                    <div className="row">
                        <div className="col-sm-12" align="right">
                            <button onClick={this.showForm} className="btn btn-outline-primary">Generar RSO</button>
                        </div>
                    </div>
                    <br />
                    <RdoCabeceraTable
                        data={this.state.rdoCabeceras}
                        showWarn={this.showWarn}
                        showSuccess={this.showSuccess}
                        updateData={this.getData}
                        Block={this.Block}
                        Unlock={this.Unlock}
                    />
                </div>

                <Dialog header="Ingreso de RSO" visible={this.state.visible} width="500px" height="400px" modal={true} onHide={this.onHide}>
                    <RdoForm rdoCabecerasDefinitivas={this.state.rdoCabecerasDefinitivas} onHide={this.onHide} updateData={this.getData} showWarn={this.showWarn} showSuccess={this.showSuccess} handleBlocking={this.handleBlocking} Block={this.Block}
                        Unlock={this.Unlock} />
                </Dialog>
            </BlockUi>
        )
    }

    getData() {
        axios.post("/proyecto/RsoCabecera/IndexApi/" + document.getElementById('content').className, {})
            .then((response) => {
                this.setState({ rdoCabecerasDefinitivas: [], rdoCabeceras: response.data, blocking: false });

                if (response.data != null && response.data.length > 0) {
                    var items = response.data.filter(c => c.es_definitivo).map((item) => {

                        return { label: 'RSO - ' + moment(item.fecha_rdo).format("DD/MM/YYYY") + " - " + item.version, dataKey: item.Id, value: item.Id };

                    });
                    this.setState({ rdoCabecerasDefinitivas: items });
                }
            })
            .catch((error) => {
                this.setState({ mensaje: 'No se pudo recuperar los datos!', blocking: false })
                this.showWarn();
            });
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.mensaje });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.mensaje });
    }

    showForm() {
        this.setState({ visible: true })
    }

    onHide() {
        this.setState({ visible: false })
    }

    handleBlocking() {
        this.setState({ blocking: !this.state.blocking })
    }
    Block = () => {
        this.setState({ blocking: true })
    }
    Unlock = () => {
        this.setState({ blocking: false })
    }
}

ReactDOM.render(
    <RdoCabecera />,
    document.getElementById('content')
);