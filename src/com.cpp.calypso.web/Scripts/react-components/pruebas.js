import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { Button } from "primereact-v3.3/button";
import CryptoJS from "crypto-js";
import Highlighter from "react-highlight-words";
class TreeDemo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      nodes: null,
      content: '',
      expandedKeys: {}
    };

    this.toggleMovies = this.toggleMovies.bind(this);
  }
  encrypt3des = (contenido) => {
    var key = "00f74597de203655b1ebf5f410f10ebi1u22hhvdsavhg4hg3jbjh23bj3bsjhoasi2i32kkj2h32g";
    var useHashing = true;
    if (useHashing) {
      key = CryptoJS.MD5(key).toString();
      key += key.substring(1, 16);
      console.log(key);
    }
    var textWordArray = CryptoJS.enc.Utf8.parse(contenido);
    var keyHex = CryptoJS.enc.Utf8.parse(key);
    //var iv = String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0);
    var iv = "IV3des";
    console.log('IV', iv);
    var ivHex = CryptoJS.enc.Utf8.parse(iv);
    var options = { mode: CryptoJS.mode.ECB, padding: CryptoJS.pad.Pkcs7, iv: ivHex };
    var encrypted = CryptoJS.TripleDES.encrypt(textWordArray, keyHex, options);
    var base64String = encrypted.toString();
    console.log('base64: ' + base64String + '\n');
    return base64String;
  }
  decrypt = (str) => {
    console.log('string', str);
    var KEY = "00f74597de203655b1ebf5f410f10ebi";//32 bit

    var IV = "00f74597de203655";//16 bits
    var key = CryptoJS.enc.Utf8.parse(KEY);
    var iv = CryptoJS.enc.Utf8.parse(IV);
    var encryptedHexStr = CryptoJS.enc.Hex.parse(str);
    var srcs = CryptoJS.enc.Base64.stringify(encryptedHexStr);
    var decrypt = CryptoJS.AES.decrypt(srcs, key, {
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    var decryptedStr = decrypt.toString(CryptoJS.enc.Utf8);
    console.log('decryptedStr', decryptedStr.toString());

    return decryptedStr.toString();
  }
  decrypt3des = (key, base64String) => {
    var useHashing = true;
    if (useHashing) {
      key = CryptoJS.MD5(key).toString();
      key += key.substring(1, 16);
      console.log(key);
    }
    var iv = String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0) + String.fromCharCode(0);
    var ivHex = CryptoJS.enc.Utf8.parse(iv);
    var keyHex = CryptoJS.enc.Utf8.parse(key);
    var options = { mode: CryptoJS.mode.ECB, padding: CryptoJS.pad.Pkcs7, iv: ivHex };
    var decrypted = CryptoJS.TripleDES.decrypt({
      ciphertext: CryptoJS.enc.Base64.parse(base64String)
    }, keyHex, options);
    console.log('decrypted: ' + decrypted.toString(CryptoJS.enc.Utf8));
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
  toggleMovies() {
    console.log(this.state.expandedKeys);

    let expandedKeys = { ...this.state.expandedKeys };

    this.state.nodes.forEach(product => {
      if (expandedKeys[product.key]) delete expandedKeys[product.key];
      else expandedKeys[product.key] = true;
    });

    this.setState({ expandedKeys: expandedKeys });
    console.log(this.state.expandedKeys);
  }

  componentDidMount() {

  }
  GetSyncSecciones = () => {
    axios
      .post("/Proyecto/Contrato/GetActualizarAzure", {})
      .then(response => {
        console.log(response.data);
        console.log(response.data);
        abp.notify.success("GENERADO CORRECTAMENTE", "SECCIONES");
      })
      .catch(error => {
        console.log(error);
        abp.notify.error("Existe un inconveniente", error);
      });
  };
  GetOutlook2 = () => {
    axios
      .post("/Proyecto/Contrato/GetOpenOutlook2", {})
      .then(response => {
        console.log(response.data);
      })
      .catch(error => {
        console.log(error);
      });
  };
  GetOutlook1 = () => {
    axios
      .post("/Proyecto/Contrato/GetOpenOutlook", {})
      .then(response => {
        console.log(response.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  GetAPI = () => {
    var objO = new ActiveXObject('Outlook.Application');
    var objNS = objO.GetNameSpace('MAPI');
    var mItm = objO.CreateItem(0);
    mItm.Display();
    mItm.To = "hola";
    mItm.Subject = "hola";
    mItm.Body = "hola";
    mItm.GetInspector.WindowState = 2;
  };
  render() {
    return (
      <div>
        asdsd
        <Button onClick={this.GetOutlook2} label="Outlook" />
        <Button onClick={this.GetOutlook1} label="GetOutlook1" />
   
      </div>
    );
  }
}
ReactDOM.render(<TreeDemo />, document.getElementById("content-prueba"));
