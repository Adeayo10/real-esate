import React, { useEffect, useState } from 'react';
import { getAccessToken, getUserNameFromToken, isTokenValid, startTokenRefresh, stopTokenRefresh } from '../utils/token';
import { logoutUser } from '../api/auth';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TOAST_OPTIONS, storeUserIdFromToken } from '../utils/constants';
import property1 from '../assets/property1.jpg';
import property2 from '../assets/property2.jpg';
import property3 from '../assets/property3.jpg';
import { Link, useNavigate } from 'react-router-dom';
import '../App.css';

function HomePage() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userName, setUserName] = useState(null);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  useEffect(() => {
    const accessToken = getAccessToken();
    if (!accessToken) {
      console.warn('Access token not found. Guest mode.');
      setIsLoggedIn(false);
      setLoading(false);
      return;
    }

    setIsLoggedIn(true);
    startTokenRefresh();

    const user = getUserNameFromToken();
    if (user) {
      setUserName(user);
    }

    setLoading(false);

    return () => {
      stopTokenRefresh();
    };
  }, []);

  const handleLogout = async (e) => {
    e.preventDefault();
    const userId = storeUserIdFromToken();
    const accessToken = getAccessToken();

    if (!isTokenValid(accessToken)) {
      toast.error('Session expired. Please log in again.');
      localStorage.clear();
      window.location.href = '/login';
      return;
    }

    try {
      await logoutUser(userId, false);
      toast.success('Logged out successfully.', TOAST_OPTIONS);
      localStorage.clear();
      navigate('/login');
    } catch (error) {
      toast.error('Error logging out. Please try again.', TOAST_OPTIONS);
      console.error('Logout error:', error);
    }
  };

  if (loading) return <div className="loader">Loading...</div>;

  return (
    <div className="landing-container">
      <nav className="navbar">
        <h1 className="property_listings">PROPERTY LISTING</h1>
        <ul className="nav-links">
          <li><a href="/listings">Listings</a></li>
          <li><a href="/about">About</a></li>
          <li><a href="/contact">Contact</a></li>
          {!isLoggedIn && <li><a href="/register">Register</a></li>}
          {!isLoggedIn && <li><a href="/login">Login</a></li>}
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
          <button onClick={() => navigate('/listings')}>Search</button>
        </div>
      </header>
      <section className="listings">
        <h2 style={{ marginTop: 25 }}>Explore Available Listings</h2>
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
        <Link to="/listings">
  <button className="buttonpadding">See more...</button>
</Link>

      </section>
      <section className="propertyType">
        <h2 style={{ marginTop: 25 }}>Find Your Perfect Property Type</h2>
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
        <h2 style={{ marginBottom: '10px' }}>What Our Happy Clients Say</h2>
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
}

export default HomePage;
