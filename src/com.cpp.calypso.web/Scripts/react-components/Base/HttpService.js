import axios from 'axios';
import config from './Config';

const http = axios.create({
    baseURL: config.baseURL,
    timeout: 200000,
});

http.interceptors.request.use(
    function (config) {
        //TODO: Token... 
        //if (!!abp.auth.getToken()) {
        //    config.headers.common['Authorization'] = 'Bearer ' + abp.auth.getToken();
        //}
 
        return config;
    },
    function (error) {
        return Promise.reject(error);
    }
);



http.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        
        console.error(error.response);

        if (!!error.response && !!error.response.data.error && !!error.response.data.error.message && error.response.data.error.details) {
            //title: error.response.data.error.message,
            //    content: error.response.data.error.details,
            console.error(error.response.data.error.detail);

            //Presentar errores... 
            abp.notify.error(error.response.data.error.detail, error.response.data.error.message);

        } else if (!!error.response && !!error.response.data.error && !!error.response.data.error.message) {
            //title: 'LoginFailed',
            //content: error.response.data.error.message,
            console.error(error.response.data.error.message);
            abp.notify.error(error.response.data.error.message);

        } else if (!error.response) {

            console.error('UnknownError');

            abp.notify.error('Se ha producido un error inesperado...');
        } else {

            abp.notify.error('Se ha producido un error inesperado...');
        }


        setTimeout(() => { }, 1000);

        return Promise.reject(error);
    }
);

export default http;