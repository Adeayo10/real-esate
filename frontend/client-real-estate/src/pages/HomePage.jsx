import React, { useEffect, useState } from 'react';
import { getAccessToken,isTokenValid, startTokenRefresh, stopTokenRefresh } from '../utils/token'; // Assuming these functions are defined in a separate file
import { logoutUser } from '../api/auth'; // Assuming this function is defined in a separate file
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TOAST_OPTIONS } from '../utils/constants';
import { storeUserIdFromToken } from '../utils/constants';



function HomePage() {
  const [isLoggedIn, setIsLoggedIn] = useState(false); 

  useEffect(() => {
    const accessToken = getAccessToken(); // 
    setIsLoggedIn(!!accessToken);

    if (accessToken) {
      startTokenRefresh();
    }

    return () => {
      stopTokenRefresh();
    };
  }, []);

  const handleLogout = async (e) => {
    e.preventDefault();
    const userId = storeUserIdFromToken() // Assuming userId is stored in localStorage
    const accessToken = getAccessToken(); // Function to get the access token from local storage or cookies
    
    // if (!accessToken) {
    //   toast.error('Access token not found. Unable to log out.');
    //   return;
    // }

    // if (!userId) {
    //   toast.error('User ID not found. Unable to log out.');
    //   return;
    // }

    if (!isTokenValid(accessToken)) {
      toast.error('Session expired. Please log in again.');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      window.location.href = '/login';
      return;
    }

    try {
      console.log('Logging out user with ID:', userId);
      await logoutUser(userId, false); // Call logoutUser with userId and useCookies=false
      toast.success('Logged out successfully.', TOAST_OPTIONS);
    } catch (error) {
      toast.error('Error logging out. Please try again.', TOAST_OPTIONS);
      console.error('Logout error:', error);
    }
  };

  return (
    <div className="landing-container">
      <nav className="navbar">
        <h1 className="logo">PROPERTY L!STING</h1>
        <ul className="nav-links">
          <li><a href="/listings">Listings</a></li>
          <li><a href="/about">About</a></li>
          <li><a href="/contact">Contact</a></li>
          {!isLoggedIn && <li><a href="/register">Register</a></li>}
          {!isLoggedIn && <li><a href="/login">Login</a></li>}
          {isLoggedIn && <li><a href="#" onClick={handleLogout}>Logout</a></li>}
        </ul>
      </nav>
      
      <header className="hero">
        <div className="hero-content">
          <h1>Welcome to Our Website</h1>
          <p>Your one-stop destination for amazing experiences.</p>
          <a href="/listings" className="cta-button">Explore Listings</a>
        </div>
      </header>
      
      <section className="features">
        <div className="feature">
          <h2>Quality Services</h2>
          <p>We provide top-notch services that you can rely on.</p>
        </div>
        <div className="feature">
          <h2>Trusted by Thousands</h2>
          <p>Join a community of happy customers.</p>
        </div>
        <div className="feature">
          <h2>24/7 Support</h2>
          <p>We are here to assist you anytime.</p>
        </div>
      </section>
    </div>
  );
}

export default HomePage;
