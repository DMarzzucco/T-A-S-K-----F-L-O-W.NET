import { ArgumentsHost, Catch, ExceptionFilter, HttpException, HttpStatus } from "@nestjs/common";
import { Response } from "express";

@Catch()
export class GloablExeptionsFilter implements ExceptionFilter {

    catch(exception: any, host: ArgumentsHost) {

        const ctx = host.switchToHttp();
        const res = ctx.getResponse<Response>()

        const status = exception instanceof HttpException
            ? exception.getStatus()
            : HttpStatus.INTERNAL_SERVER_ERROR

        const message = exception instanceof HttpException
            ? (exception.getResponse() as string | object)
            : (exception as Error).message || "Uknowed error";

        res.status(status).json({ statusCode: status, message: message })
    }
}