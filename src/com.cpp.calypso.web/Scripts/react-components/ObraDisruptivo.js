import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Dialog } from 'primereact/components/dialog/Dialog';
import { Growl } from 'primereact/components/growl/Growl';

import DisruptivoTable from './disruptivo_components/disruptivo_table';
import DisruptivoForm from './disruptivo_components/disruptivoForm';

export default class ObraDisruptivo extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			data_table: [],
			visible: false,
			table_key: 8957,
			catalogos: [],
			recursos: []
		};
		this.DataTable = this.DataTable.bind(this);
		this.LoadForm = this.LoadForm.bind(this);
		this.successMessage = this.successMessage.bind(this);
		this.warnMessage = this.warnMessage.bind(this);
		this.showSuccess = this.showSuccess.bind(this);
		this.showWarn = this.showWarn.bind(this);
		this.onHide = this.onHide.bind(this);
		this.GetCatalogos = this.GetCatalogos.bind(this);
		this.RenderButton = this.RenderButton.bind(this);
	}

	componentDidMount() {
		this.DataTable();
		this.GetCatalogos();
	}

	render() {
		return (
			<div>
				<Growl
					ref={(el) => {
						this.growl = el;
					}}
					position="bottomright"
					baseZIndex={1000}
				/>
				{this.RenderButton()}
				<hr />
				<DisruptivoTable
					catalogos={this.state.catalogos}
					recursos={this.state.recursos}
					key={this.state.table_key}
					data={this.state.data_table}
					updateTable={this.DataTable}
					showSuccess={this.successMessage}
					showWarn={this.warnMessage}
				/>

				<Dialog
					header="Gestión de disruptivos"
					visible={this.state.visible}
					width="850px"
					modal={true}
					onHide={this.onHide}
				>
					<DisruptivoForm
						catalogos={this.state.catalogos}
						recursos={this.state.recursos}
						showSuccess={this.successMessage}
						showWarn={this.warnMessage}
						updateTable={this.DataTable}
						onHide={this.onHide}
					/>
				</Dialog>
			</div>
		);
	}

	DataTable() {
		axios
			.post('/proyecto/ObraDisruptivo/IndexApi/' + document.getElementById('AvanceId').className, {})
			.then((response) => {
				this.setState({ data_table: response.data });
			})
			.catch((error) => {
				console.log(error);
				this.showWarn();
			});
	}

	LoadForm() {
		this.setState({ visible: true });
	}

	GetCatalogos() {
		axios
			.post('/proyecto/ObraDisruptivo/GetImproductividadCatalogoApi', {})
			.then((response) => {
				this.setState({ catalogos: response.data });
			})
			.catch((error) => {
				console.log(error);
			});

		axios
			.post('/proyecto/ObraDisruptivo/GetTipoRecursoApi', {})
			.then((response) => {
				this.setState({ recursos: response.data });
			})
			.catch((error) => {
				console.log(error);
			});
	}

	RenderButton() {
		return (
			<div className="row">
				<div className="col" />
				<div className="col" align="right">
					<button onClick={this.LoadForm} className="btn btn-outline-primary">
						Nuevo Disruptivo
					</button>
				</div>
			</div>
		);
	}

	showSuccess() {
		this.growl.show({
			life: 5000,
			severity: 'success',
			summary: 'Proceso exitoso!',
			detail: this.state.message
		});
	}

	showWarn() {
		this.growl.show({
			life: 5000,
			severity: 'error',
			summary: 'Error',
			detail: this.state.message
		});
	}

	successMessage(msg) {
		console.log(msg + 'aa');
		this.setState({ message: msg }, this.showSuccess);
	}

	warnMessage(msg) {
		console.log(msg + 'bb');
		this.setState({ message: msg }, this.showWarn);
	}

	onHide(event) {
		this.setState({ visible: false });
	}

	updateTableKey() {
		this.setState({ table_key: Math.random() });
	}
}
