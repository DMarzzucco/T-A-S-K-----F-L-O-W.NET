import App from "./main";

const port = process.env.PORT || 3000;

App.listen (port, () =>{
    console.log (`Server listening port ${port}`)
});
