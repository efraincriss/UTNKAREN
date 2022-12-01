import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { Card } from 'primereact-v2/card';
import { Checkbox } from 'primereact-v2/checkbox';
import {
    TIPO_GRUPO_PERSONAL,
    PASSCRYPTO
} from "../Base/Constantes";
import QRCode from "qrcode.react";
import CryptoJS from "crypto-js";
export default class Fotografia extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            /*Generación QR */
            iseleccionado: null,

            //VALIDACION CÉDULA
            checked: false,

            loading: true,
            codigo_qr: [],

            //ERRORES
            errors: {},
            /*Generación QR */
            QrDialogE: false,
            DataColaborador: '',
            EncryptedData: '',
        }

  
        this.DescargarQR = this.DescargarQR.bind(this);
        this.MostrarDialogQrE = this.MostrarDialogQrE.bind(this);
        this.permitirvalidacioncedula = this.permitirvalidacioncedula.bind(this);
    }

    permitirvalidacioncedula() {
   
        this.setState({ loadingqr: true })
        if (this.props.id_colaborador > 0) {
    
    
          axios
            .post("/RRHH/Colaboradores/CreateValidacionCedula/", {
              id: this.props.id_colaborador
            })
            .then(response => {
              if (response.data == "OK") {
                this.setState({ checked: e.checked, loadingqr: false })
                this.props.showSuccess("Validación por cédula actualizado");
              } else {
                this.setState({ loadingqr: false })
                this.props.showWarning("Debe seleccionar un colaborador");
              }
    
            })
            .catch(error => {
              console.log(error);
              this.setState({ loadingqr: false })
              this.warnMessage("Algo salio mal!");
            });
        } else {
          this.props.showWarn("Ocurrió un error al actualizar campo validación");
    
        }
      }
    

    MostrarDialogQrE() {
console.log("DIALOGQR")
        if(this.props.id_colaborador>0){
        axios
          .post("/RRHH/Colaboradores/GetDataQr/", {
            Id: this.props.id_colaborador
          })
          .then(response => {
            var data = JSON.stringify(response.data.result);
            // Encriptar
            var ciphertext = CryptoJS.AES.encrypt(data, PASSCRYPTO);
            console.log(ciphertext.toString());
    
            // Decrypt
            var bytes = CryptoJS.AES.decrypt(ciphertext.toString(), PASSCRYPTO);
            var plaintext = bytes.toString(CryptoJS.enc.Utf8);
            console.log(plaintext);
    
            this.setState({
              QrDialog: true,
              loadingqr: false,
              EncryptedData: ciphertext.toString()
            });
    
          })
          .catch(error => {
            console.log(error);
          });
        }
    
    
    
        //this.setState({ loadingqr: true, checked: row.validacion_cedula, QrDialog: true });
    
      }
    


    componentDidMount() {
  
    }

    render() {
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.loading}>
                    <div>

                        <Card className="ui-card-shadow">
                            <div>

                                <div className="row">

                                    <div className="col-xs-12 col-md-6">
                                        <Card className="ui-card-shadow">
                                            <b>Información Colaborador</b><br /><br />
                                            <h6 className="text-gray-700">
                                                <b>No. de Identificación: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.numero_identificacion
                                                    : ""}
                                            </h6>
                                            <h6 className="text-gray-700">
                                                <b> Apellidos Nombres: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.nombres_apellidos
                                                    : ""}
                                            </h6>
                                            <h6 className="text-gray-700">
                                                <b>Destino: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.nombreestancia
                                                    : ""}
                                            </h6>
                                            <h6 className="text-gray-700">
                                                <b>Servicios: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.serviciosvigentes
                                                    : ""}
                                            </h6>
                                            <h6 className="text-gray-700">
                                                <b>Tiene Reservas Activas: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.tienereservaactiva
                                                    : ""}
                                            </h6>
                                        </Card><br />
                                        <Card className="ui-card-shadow">
                                            <b>Permite Validación por Cédula:</b><br />
                                            <br />

                                            <div className="row">
                                                <div className="col-xs-12 col-md-6"><label>(SI/NO)</label></div>
                                                <div className="col-xs-12 col-md-6">  <Checkbox checked={this.state.checked} onChange={this.permitirvalidacioncedula} />
                                                </div>

                                            </div>
                                        </Card><br />
                                        <Card className="ui-card-shadow">

                                            <h6 className="text-gray-700">
                                                <b>Fecha de Vigencia del QR: </b>{" "}
                                                {this.props.usuario != null
                                                    ? this.props.usuario.fechavigenciacolaboradorqr
                                                    : ""}
                                            </h6>
                                        </Card>
                                    </div>


                                    <div className="col-xs-12 col-md-4">
                                        <QRCode value={this.state.EncryptedData} size={260} id="FPImage1" />



                                        <div className="row" >
                                            {/*     
                                         <img href="./Views/LogosCPP/_cpp.png" id="FPImage1" height="350" width="350" /><br></br>
 
                                         <button
                                                onClick={() => this.DescargarQR()}
                                                type="button"
                                                className="btn btn-outline-primary"
                                                style={{ marginLeft: "3px" }}
                                            >
                                                Imprimir QR
                                        </button>*/}
                                            <button
                                                onClick={() => this.props.onHide()}
                                                type="button"
                                                className="btn btn-outline-primary"
                                                style={{ marginLeft: "3px" }}
                                            > Cancelar
                                         </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </Card>
                    </div>
                </BlockUi>
            </div >
        )
    }

     DescargarQR() {
        if (this.state.codigo_qr) {
            var element = document.createElement("a");
            element.setAttribute("href", document.getElementById("FPImage1").src);
            element.setAttribute(
                "download",
                this.state.iseleccionado != null
                    ? this.state.iseleccionado.nombres_apellidos + ".jpg"
                    : "QR.jpg"
            );

            element.style.display = "none";
            document.body.appendChild(element);

            element.click();

            document.body.removeChild(element);
        } else {
            abp.notify.error('Se debe generar el QR!', 'Error');
        }
    }


}