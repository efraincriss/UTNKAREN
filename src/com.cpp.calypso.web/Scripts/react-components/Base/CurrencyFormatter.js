import React  from 'react';
import CurrencyFormat from 'react-currency-format';
 
export default function currencyFormatter(cell, row, props) {
    return <CurrencyFormat value = { cell } displayType = { 'text'} thousandSeparator = { true} />
}