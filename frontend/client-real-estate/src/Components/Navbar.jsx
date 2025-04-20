import React from 'react';
import { Link } from 'react-router-dom';
import '../App.css';

function Navbar({ isLoggedIn, userName, isDropdownOpen, setIsDropdownOpen, handleLogout }) {
  return (
    <nav className="navbar">
      <h1 className="property_listings"><Link to="/">Real Estate</Link></h1>
      <ul className="nav-links">
        <li><Link to="/listings">Listings</Link></li>
        <li><Link to="/about">About</Link></li>
        <li><Link to="/contact">Contact</Link></li>
        {!isLoggedIn && <li><Link to="/register">Register</Link></li>}
        {!isLoggedIn && <li><Link to="/login">Login</Link></li>}
        {isLoggedIn && (
          <div className="user-menu">
            <span
              className="user-name"
              onClick={() => setIsDropdownOpen(!isDropdownOpen)}
            >
              Hello! {userName} ▼
            </span>
            {isDropdownOpen && (
              <div className="dropdown-menu">
                <button onClick={handleLogout}>Logout</button>
              </div>
            )}
          </div>
        )}
      </ul>
    </nav>
  );
}

export default Navbar;