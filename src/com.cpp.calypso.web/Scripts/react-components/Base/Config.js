const config = {
  //apiUrl: "http://10.26.102.61/", //QA teiecwas104
  //appUrl: "http://10.26.102.61/",
  //apiUrl: "http://10.26.102.61:90/", //QA teiecwas104 Capacitaciones port 90
  //appUrl: "http://10.26.102.61:90/",
  // apiUrl: "http://45.35.14.178/",
  //appUrl: "http://45.35.14.178/",
  //apiUrl: "http://45.35.14.178:100/",
  //appUrl: "http://45.35.14.178:100/",

  //formFormatDate:"DD-MM-YYYY",*/
  apiUrl: "http://localhost:7090/",
  appUrl: "http://localhost:7090/",
  //apiUrl: "http://pdmis.teic.techint.net/", //Prod teiecwas102
  //appUrl: "http://pdmis.teic.techint.net/",//
  formatDate: "YYYY-MM-DD",
  formatTime: "HH:mm:ss",
  locale: "es",
  localization: {
    
    defaultLocalizationSourceName: "Cpp",
  },
  maxFileSize: 2000000,
};

module.exports = config;