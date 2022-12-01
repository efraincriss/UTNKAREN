import React from "react";
import ContactoCard from "./ContactoCard";


export default class ContactosContainer extends React.Component {
    constructor(props) {
        super(props)
    }



    render() {
        return (
            <div className="row" style={{ marginTop: '2em' }}>
                <div className="col">
                    <div className="row">
                        {this.renderCards(this.props.contactos)}
                    </div>
                </div>
            </div>
        )
    }

    renderCards = (contactos) => {
        return contactos.map(contacto => {
            let random = this.randomNumer()
            let flagClass = this.cardClassStyle(random)
            return (
                <ContactoCard
                    key={contacto.Identificacion+"_key"}
                    contacto={contacto}
                    flagClass={flagClass}
                />
            )
        })
    }

    randomNumer = () => {
        return Math.floor(Math.random() * 6) + 1;
    }

    cardClassStyle = (number) => {
        switch (number) {
            case 1:
                return "cyan"
            case 2:
                return "indigo"
            case 3:
                return "purple"
            case 4:
                return "pink"
            case 5:
                return "red"
            case 6:
                return "orange"
            default:
                return "teal"
        }
    }
}