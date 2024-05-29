import React from "react"
import { Link } from "react-router-dom";
// import '../App.scss'
const Header: React.FC = () => {
    return (
        <header className="header">
            <Link to={"/"} className="logo">
                <i className="fa-solid fa-car"></i> <span>AutoMotris</span>
            </Link>
            <nav className="menu">
                <ul className="menu-list">
                    <li className="menu-list_item" style={{ display: 'inline-block' }}>
                        <a href="#">Home</a>
                    </li>
                    <li className="menu-list_item" style={{ display: 'inline-block' }}>
                        <a href="#">Features</a>
                    </li>
                    <li className="menu-list_item" style={{ display: 'inline-block' }}>
                        <a href="#">Price</a>
                    </li>
                    <li className="menu-list_item" style={{ display: 'inline-block' }}>
                        <a href="#">Contact</a>
                    </li>
                    <li className="menu-list_item" style={{ display: 'inline-block' }}>
                        <a href="#" className="book-apointment">Book Apointment</a>
                    </li>
                </ul>
            </nav>
        </header>
    )
}

export default Header;