import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

export default class SubirArchivo extends React.Component {
    constructor(props) {
        super(props);
        this.state ={
            visible: false,
            message: '',
            blocking: false,
          
          file:null
        }
        this.onFormSubmit = this.onFormSubmit.bind(this)
        this.onChange = this.onChange.bind(this)
        this.fileUpload = this.fileUpload.bind(this)
        this.successMessage = this.successMessage.bind(this)
                this.warnMessage = this.warnMessage.bind(this)
                this.showForm = this.showForm.bind(this);
                this.onHide = this.onHide.bind(this);

                this.showSuccess = this.showSuccess.bind(this);
                this.showWarn = this.showWarn.bind(this);
       
      }
      onFormSubmit(e){
        e.preventDefault() // Stop form submit
        this.fileUpload(this.state.file)
      }
      onChange(e) {
        this.setState({file:e.target.files[0]})
      }
      fileUpload(file){
       const formData = new FormData();
        formData.append('UploadedFile',file)
        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        }
        axios.post("./Proyecto/OfertaComercial/SubirArchivo/",formData,config)
        .then((response) => {
            console.log(response);
           this.successMessage("Cargado Correctamente")
           this.setState({blocking: false,visible:false})
    
        })
        .catch((error) => {
        console.log(error);    
        this.warnMessage("Error Subida")
        this.setState({blocking: false})
        });
        
    }
    
      render() {
        return (
          <form onSubmit={this.onFormSubmit}>

            <input type="file" 
                     onChange={this.onChange}
                     
                   />
            <button type="submit"
            
            className="btn btn-outline-primary"
            >Guardar</button>
          </form>
       )
      }
    
    showForm(){
        this.setState({visible: true})
    }
  
    onHide(event){
        this.setState({visible: false});
    }       
    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: '', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }
    successMessage(msg){
        this.setState({message: msg}, this.showSuccess)
    }

    warnMessage(msg){
        this.setState({message: msg}, this.showWarn)
    }
}
