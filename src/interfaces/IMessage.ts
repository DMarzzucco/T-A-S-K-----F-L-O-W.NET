import { Request } from "express";
export interface IMessage {
    id: 'text';
    text: 'text';
}
export interface Payload { userId: string; }
export interface userPayload { id: string; }
export interface AuthenticateRequest extends Request { user?: userPayload; }