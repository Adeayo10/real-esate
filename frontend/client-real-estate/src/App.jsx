import React, { useEffect, useState, useRef } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import Login from './pages/LoginPage';
import AboutPage from './pages/AboutPage';
import ContactPage from './pages/ContactPage';
import ListingPage from './pages/ListingPage';
import RegisterPage from './pages/RegisterPage';
import { renewAuthToken } from './api/auth';
import './App.css'; 
import { ToastContainer } from 'react-toastify';

const App = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const isInitialCheck = useRef(true); 

  useEffect(() => {
    const checkAuth = async () => {
      const accessToken = localStorage.getItem('accessToken');
      const refreshTokenValue = localStorage.getItem('refreshToken');

      if (!accessToken && refreshTokenValue) {
        try {
          const response = await renewAuthToken({ accessToken, refreshToken: refreshTokenValue });
          localStorage.setItem('accessToken', response.accessToken);
          if (!isAuthenticated) setIsAuthenticated(true); 
        } catch (error) {
          console.error('Failed to refresh token:', error);
          if (isAuthenticated) setIsAuthenticated(false); 
        }
      } else if (accessToken) {
        if (!isAuthenticated) setIsAuthenticated(true); 
      } else {
        if (isAuthenticated) setIsAuthenticated(false); 
      }
    };

    if (isInitialCheck.current) {
      checkAuth();
      isInitialCheck.current = false;
    }
  }, [isAuthenticated]); 

  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route
            path="/login"
            element={
              isAuthenticated ? <Navigate to="/" /> : <Login />
            }
          />
          <Route
            path="/register"
            element={
              isAuthenticated ? <Navigate to="/" /> : <RegisterPage />
            }
          />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/contact" element={<ContactPage />} />
          <Route path="/listings" element={<ListingPage />} />
        </Routes>
      </Router>
      <ToastContainer />
    </>
  );
};

export default App;
