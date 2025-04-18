import React, { useState, useEffect } from 'react';
import { loginUser } from '../api/auth';
import { startTokenRefresh } from '../utils/token'; // Assuming this function is defined in a separate file
import '../Auth.css';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TOAST_OPTIONS } from '../utils/constants';
import { getAccessToken } from '../utils/token'; // Assuming this function is defined in a separate file

const Login = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const accessToken = getAccessToken();
    if (accessToken) {
      console.warn('Access token found in localStorage. Redirecting to home.');
      navigate('/');
      return;
    }
  }, [navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      const loginData = { 
        email, 
        password
      };

      const useBearerToken = true; // Always use bearer token
      const response = await loginUser({ ...loginData, useBearerToken });
      console.log('Login successful:', response);
      toast.success(`${response.message} Welcome`, TOAST_OPTIONS);
      startTokenRefresh(); // Start token refresh after successful login
      
      setTimeout(() => {
        navigate('/'); // Redirect to home page after 2 seconds
      }, 5000); // Adjust the delay as needed
    } catch (err) {
      setError(err.message || 'An error occurred');
      toast.error(err.message || 'An error occurred!', TOAST_OPTIONS);
    }
  };

  return (
    <div className="auth-wrapper">
      <div className="auth-container">
        <form onSubmit={handleSubmit} className="auth-form">
          <h2>Login</h2>
          {error && <p className="error">{error}</p>}
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <label>
            <input
              type="checkbox"
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
            />
            Remember Me
          </label>
          <button type="submit">Login</button>
          <div className="auth-toggle">
            <p>Don't have an account? <a href="/register">Register here</a></p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
