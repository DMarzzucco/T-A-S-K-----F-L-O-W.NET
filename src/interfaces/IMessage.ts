import { Request } from "express";
import { Document, Types } from "mongoose";
export interface IMessage {
    id: string;
    text: string;
}
export interface Payload { id: string; }
export interface userPayload { id: string; }
export interface AuthenticateRequest extends Request { user?: userPayload; }
export interface TaskDB extends Document {
    date: Date;
    title?: string | null;
    user?: Types.ObjectId | null;
}
export interface ErrorResponse { error: string }
export interface ValidationErrorResponse { errors: { message: string }[]; }
