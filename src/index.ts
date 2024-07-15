import { App } from "./app/main";

async function main() {
    const app = new App(3000)
    await app.listen()
}
main().catch(err => {
    console.log(`Error al iniciar el servidor ${err}`)
    process.exit(1)
})