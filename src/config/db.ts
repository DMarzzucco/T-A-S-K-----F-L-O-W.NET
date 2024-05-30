import mongoose from "mongoose";

export const DB = async () => {
    try {
        await mongoose.connect('mongodb://localhost/mode');
        console.log(`servidor ${DB} conected `)
    }
    catch (error) {
        console.log(error);
    }
}