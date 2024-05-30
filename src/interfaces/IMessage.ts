import { Request } from "express";
import { Document, Types } from "mongoose";
export interface IMessage {
    id: 'text';
    text: 'text';
}
export interface Payload { userId: string; }
export interface userPayload { id: string; }
export interface AuthenticateRequest extends Request { user?: userPayload; }
export interface TaskDB extends Document {
    date: Date;
    title?: string | null;
    user?: Types.ObjectId | null;
}