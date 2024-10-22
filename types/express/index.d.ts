declare namespace Express {
    interface Request {
        idUser: string;
        roleUser: string;
    }
    interface User {
        idUser: string;
        roleUser: string;
    }
}