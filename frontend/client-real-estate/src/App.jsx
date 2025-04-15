import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import property1 from './assets/property1.jpg';
import property2 from './assets/property2.jpg';
import property3 from './assets/property3.jpg';
import "./App.css";
import Login from './components/Login';
import Signup from './components/Signup';
import PropertyList from "./components/PropertyList";




const Home = ({ user, handleLogout }) => {
    const [dropdownVisible, setDropdownVisible] = useState(false);

    const toggleDropdown = () => {
        setDropdownVisible(!dropdownVisible);
    };

    return (
        <div className="container">
            <nav className="navbar">
                <h2>Property Listing</h2>
                <ul>
                    <li><a href="#">Home</a></li>
                    <li><a href="#">Listing</a></li>
                    <li><a href="#">About</a></li>
                    <li><a href="#">Contact</a></li>
                </ul>
                
                {user ? (
                    <div className="user-info">
                        <span onClick={toggleDropdown} className="user-name">
                            {user.firstName}
                        </span>
                        {dropdownVisible && (
                            <div className="dropdown">
                                <button onClick={handleLogout}>Logout</button>
                            </div>
                        )}
                    </div>
                ) : (
                    <button onClick={() => window.location.href = "/login"}>Sign Up / Login</button>
                )}
            </nav>

            {/* Hero Section */}
            <header className="hero">
                <h1>Find Your Dream Home Today!</h1>
                <p>Browse thousands of properties with ease.</p>
                <div className="search-bar">
                    <select>
                        <option>Buy</option>
                        <option>Rent</option>
                    </select>
                    <select>
                        <option>Home</option>
                        <option>Apartment</option>
                        <option>Villa</option>
                    </select>
                    <input type="text" placeholder="Search..." />
                    <button>Search</button>
                </div>
            </header>

            {/* Explore Listings */}
            <section className="listings">
                <h2>Explore Available Listings</h2>
                <div className="listing-cards">
                    <div className="card">
                        <img src={property1} alt="Dublin Apartment" />
                        <h3>3 BHK Apartment in Dublin</h3>
                        <p>$250,000</p>
                    </div>
                    <div className="card">
                        <img src={property2} alt="Cork Apartment" />
                        <h3>1 BHK Apartment in Cork</h3>
                        <p>$100,000</p>
                    </div>
                    <div className="card">
                        <img src={property3} alt="Galway Apartment" />
                        <h3>2 BHK Apartment in Galway</h3>
                        <p>$150,000</p>
                    </div>
                </div>
                <Link to="/property-listing">
                    <button>See more...</button>
                </Link>
            </section>

            {/* About Us */}
           
            <section className="about">
                <h2>About Us</h2>
                <p>
                    Welcome to our real estate platform, where we connect buyers, sellers, and investors with the finest properties in the market. Whether you're looking for a cozy apartment, a luxurious villa, or a commercial space, we provide a seamless experience with expert guidance at every step. Our commitment to transparency, reliability, and customer satisfaction ensures that you find the perfect property that suits your needs and budget. Explore our listings and let us help you turn your real estate dreams into reality!
                </p>
            </section>

            <section className="propertyType">
                <h2>Find Your Perfect Property Type</h2>
                <div className="listing-cards">
                    <button className="card touch-card">
                        <img src={property1} alt="Town House" />
                        <h3>Town Houses</h3>
                    </button>
                    <button className="card touch-card">
                        <img src={property2} alt="Apartment" />
                        <h3>Apartments</h3>
                    </button>
                    <button className="card touch-card">
                        <img src={property3} alt="Villa" />
                        <h3>Villas</h3>
                    </button>
                </div>
            </section>

            <section className="reviews">
                <h2>What Our Happy Clients Say</h2>
                <div className="review-cards">
                    <div className="reviewCard">
                        <div className="review-profile"></div>
                        <p>"Amazing experience! Found my dream home within a week. Highly recommended!"</p>
                        <h4>- Sarah M.</h4>
                    </div>
                    <div className="reviewCard">
                        <div className="review-profile"></div>
                        <p>"Very professional team and a wide variety of listings. Loved the smooth process."</p>
                        <h4>- James T.</h4>
                    </div>
                    <div className="reviewCard">
                        <div className="review-profile"></div>
                        <p>"A fantastic platform that helped me sell my house quickly and easily!"</p>
                        <h4>- Priya R.</h4>
                    </div>
                    <div className="reviewCard">
                        <div className="review-profile"></div>
                        <p>"They truly care about customer satisfaction. I felt guided every step of the way."</p>
                        <h4>- Mark D.</h4>
                    </div>
                </div>
            </section>



            {/* Footer */}
            <footer className="footer">
                <p>© 2025 Property Listing. All rights reserved.</p>
                <div className="footer-links">
                    <a href="#">Privacy Policy</a>
                    <a href="#">Terms of Service</a>
                    <a href="#">Support</a>
                </div>
            </footer>
        </div>
    );
};

const App = () => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const storedUser = localStorage.getItem("loggedInUser");
        if (storedUser) {
            setUser(JSON.parse(storedUser));
        }
    }, []);

    const handleLogin = (userData) => {
        localStorage.setItem("loggedInUser", JSON.stringify(userData));
        setUser(userData);
    };

    const handleLogout = () => {
        localStorage.removeItem("loggedInUser");
        setUser(null);
    };

    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home user={user} handleLogout={handleLogout} />} />
                <Route path="/login" element={<Login onLogin={handleLogin} />} />
                <Route path="/signup" element={<Signup onSignup={handleLogin} />} />
                <Route path="/property-listing" element={<PropertyList />} />
            </Routes>
        </Router>
    );
};

export default App;
