import App from "./app/main";
import { DB } from "./config/db";

DB();
const port = process.env.PORT || 3000;

App.listen(port, () => {
    console.log(`Server listening port ${port}`)
});
