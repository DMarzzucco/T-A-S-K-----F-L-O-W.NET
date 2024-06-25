import mongoose from "mongoose";

const userModel = new mongoose.Schema({
    username: {
        type: String,
        require: true,
        trim: true,
        unique: true
    },
    fullname:{
        type:String,
        require:true,
        trim:true,
    },
    email: {
        type: String,
        require: true,
        trim: true,
        unique: true
    },
    password: {
        type: String,
        require: true
    }
}, { timestamps: true })

export default mongoose.model('User', userModel);