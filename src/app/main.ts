import express, { Application } from "express";
import cors from "cors";
import cookieParser from "cookie-parser"
import { taskRoutes, authRoutes } from "../routes";
import { DB } from "../config/db";

export class App {
  private app: Application;

  constructor(private port?: string | number) {
    this.app = express();
    this.settings();
    this.middleware();
    this.Routes();
    DB()
  }
  private middleware() {
    this.app.use(cors({
      origin: "http://localhost:5173",
      credentials: true
    }))
    this.app.use(express.json());
    this.app.use(cookieParser());
  }
  private Routes(): void {
    this.app.use("/api", authRoutes);
    this.app.use('/api', taskRoutes)
  }
  settings() {
    this.app.set('port', this.port || process.env.PORT || 3000)
  }
  async listen() {
    await this.app.listen(this.app.get('port'))
    console.log(`Port Listen in ${this.app.get('port')}`)
  }
}