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
import axios from 'axios';

function HomePage() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userName, setUserName] = useState(null);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const [transactionType, setTransactionType] = useState('buy');
  const [propertyType, setPropertyType] = useState('home');
  const [searchKeyword, setSearchKeyword] = useState('');
  const navigate = useNavigate();

  const handleSearch = async () => {
    try {
      const response = await axios.get('/api/list/filterproperty', {
        params: {
          mode: transactionType,
          type: propertyType,
        }
      });

      const data = response.data.data || [];
      console.log("Filtered properties:", data);
      
      
      navigate('/listings', { state: { filteredProperties: data, showAllButton: true } });
    } catch (error) {
      console.error("Error fetching filtered properties:", error.response?.data || error);
      toast.error('Error fetching properties. Please check your input.', TOAST_OPTIONS);
    }
  };

  useEffect(() => {
    const accessToken = getAccessToken();
    if (!accessToken) {
      setIsLoggedIn(false);
      setLoading(false);
      return;
    }

    setIsLoggedIn(true);
    startTokenRefresh();

    const user = getUserNameFromToken();
    if (user) setUserName(user);
    setLoading(false);

    return () => stopTokenRefresh();
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
              <span className="user-name" onClick={() => setIsDropdownOpen(!isDropdownOpen)}>
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
          <select value={transactionType} onChange={(e) => setTransactionType(e.target.value)}>
            <option value="buy">Buy</option>
            <option value="rent">Rent</option>
          </select>
          <select value={propertyType} onChange={(e) => setPropertyType(e.target.value)}>
            <option value="home">Home</option>
            <option value="apartment">Apartment</option>
            <option value="villa">Villa</option>
          </select>
          <input
            type="text"
            placeholder="Search..."
            value={searchKeyword}
            onChange={(e) => setSearchKeyword(e.target.value)}
          />
          <button onClick={handleSearch}>Search</button>
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

      {/* <section className="propertyType">
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
      </section> */}

      <section className="reviews">
        <h2 style={{ marginBottom: '10px' }}>What Our Happy Clients Say</h2>
        <div className="review-cards">
          {[ 
            { name: 'Sarah M.', text: 'Amazing experience! Found my dream home within a week. Highly recommended!' },
            { name: 'James T.', text: 'Very professional team and a wide variety of listings. Loved the smooth process.' },
            { name: 'Priya R.', text: 'A fantastic platform that helped me sell my house quickly and easily!' },
            { name: 'Mark D.', text: 'They truly care about customer satisfaction. I felt guided every step of the way.' },
            { name: 'Emily S.', text: 'Easy-to-use platform and wonderful customer support. Would use again!' },
            { name: 'Liam B.', text: 'Quick, smooth, and transparent property dealings. Impressive service!' }
          ].map((review, index) => (
            <div className="reviewCard" key={index}>
              <div className="review-profile"></div>
              <p>"{review.text}"</p>
              <h4>- {review.name}</h4>
            </div>
          ))}
        </div>
        <p className="scroll-hint">Scroll to view more →</p>
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
