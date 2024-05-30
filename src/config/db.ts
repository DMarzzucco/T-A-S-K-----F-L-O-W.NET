import mongoose from "mongoose";

export const DB = async () => {
    try {
        await mongoose.connect('mongodb://localhost/mode');
        console.log("MONGOO IS START 😃")
    }
    catch (error) {
        console.log(error);
    }
}
